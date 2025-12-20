using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.GPUSort;
using static Utils;
using Random = System.Random;

#region calculate card in hand
public class SimplifiedCard
{
    public CardType type;
    public double numberValue;
    public OperationEnum? operatorValue;
    public int position;
    public bool isProcessedNumber;
    public int priority;
    /*
     * priority
     * 3 = Parentheses
     * 2 = *,/
     * 1 = +,-
     * 0 = not an operator
     */

    public SimplifiedCard(double numberValue, OperationEnum? operatorValue, int position)
    {
        this.type = operatorValue == null ? CardType.Number : CardType.Operator;
        this.numberValue = numberValue;
        this.operatorValue = operatorValue;
        this.position = position;
        this.isProcessedNumber = false;

        if (this.type == CardType.Operator)
        {
            switch (this.operatorValue)
            {
                case OperationEnum.Multiply:
                case OperationEnum.Divide:
                    this.priority = 2;
                    break;

                case OperationEnum.Plus:
                case OperationEnum.Minus:
                    this.priority = 1;
                    break;
            }
        }
        else
        {
            this.priority = 0;
        }
    }
    public override string ToString()
    {
        if (type == CardType.Number)
            return isProcessedNumber
                ? $"({(numberValue % 1 == 0 ? numberValue.ToString("0") : numberValue.ToString("0.00"))})"
                : (numberValue % 1 == 0 ? numberValue.ToString("0") : numberValue.ToString("0.00"));
        else
            return operatorValue.ToString();

    }
}

public static class PlayCardCalculation
{
    public static List<SimplifiedCard> GameObjectToSimplifiedCard(List<GameObject> handEquation)
    {
        List<SimplifiedCard> simplified = new List<SimplifiedCard>();
        for (int i = 0; i < handEquation.Count; i++)
        {
            SimplifiedCard sCard = new SimplifiedCard(
                handEquation[i].GetComponent<Card>() != null ? handEquation[i].GetComponent<Card>().GetFaceValue() : 0,
                handEquation[i].GetComponent<Operatorcard>() != null ? handEquation[i].GetComponent<Operatorcard>().GetOperator() : null,
                i);

            simplified.Add(sCard);
        }

        return simplified;
    }


    public static List<object[]> EvaluateEquation(List<GameObject> cards, ParenthesesMode mode)
    {
        if (ValidationHand(cards) < 0)
        {
            return null;
        }

        // Convert to simplified struct
        simplified = GameObjectToSimplifiedCard(cards);

        // Apply parentheses collapsing
        ApplyParenthesesMode(simplified, mode);
        List<object[]> resultLog = new List<object[]>();

        for (int priorityOrder = 3; priorityOrder > 0; priorityOrder--)
        {
            for (int i = 1; i < simplified.Count - 1; i += 2)
            {
                if (simplified[i].priority == priorityOrder)
                {
                    double left = simplified[i - 1].numberValue;
                    double right = simplified[i + 1].numberValue;

                    if (simplified[i].operatorValue == OperationEnum.Divide && right == 0)
                    {
                        Debug.Log($"div by 0 on operator position {simplified[i].position}");
                        return null;
                    }

                    double result = simplified[i].operatorValue switch
                    {
                        OperationEnum.Plus => left + right,
                        OperationEnum.Minus => left - right,
                        OperationEnum.Multiply => left * right,
                        OperationEnum.Divide => Math.Round(left / right, 2),
                        _ => throw new Exception("Unknown operator")
                    };

                    // Collapse result
                    simplified[i - 1].numberValue = result;
                    simplified[i - 1].isProcessedNumber = true;
                    int operatorPosition = simplified[i].position;

                    simplified.RemoveAt(i + 1);
                    simplified.RemoveAt(i);

                    resultLog.Add(new object[] { BuildEquationString(simplified), operatorPosition });


                    i -= 2;
                }
            }
        }

        resultLog.Add(new object[] { "END", simplified[0].numberValue });
        return resultLog;
    }

    private static void ApplyParenthesesMode(List<SimplifiedCard> cards, ParenthesesMode mode)
    {
        switch (mode)
        {
            case ParenthesesMode.DoFrontOperationFirst:
                cards[1].priority = 3;
                break;
            case ParenthesesMode.DoMiddleOperationFirst:
                cards[3].priority = 3;
                break;
            case ParenthesesMode.DoLastOperationFirst:
                cards[5].priority = 3;
                break;
            case ParenthesesMode.DoMiddleOperationLast:
                cards[1].priority = 3;
                cards[5].priority = 3;
                break;
        }
    }


    private static string BuildEquationString(List<SimplifiedCard> cards)
    {
        return string.Join("", cards);
    }
    #endregion

    #region Get all equation possibility
    static List<string> stringPosibleOperator = new List<string>();

    public static List<SimplifiedCard> simplified { get; private set; }

    public static Dictionary<double, List<string>> GetMostFrequentResults(List<double> numbers, List<OperationEnum> posibleOperators)
    {
        stringPosibleOperator = new List<string>();
        if (posibleOperators.Contains(OperationEnum.Plus)) { stringPosibleOperator.Add("+"); }
        if (posibleOperators.Contains(OperationEnum.Minus)) { stringPosibleOperator.Add("-"); }
        if (posibleOperators.Contains(OperationEnum.Multiply)) { stringPosibleOperator.Add("*"); }
        if (posibleOperators.Contains(OperationEnum.Divide)) { stringPosibleOperator.Add("/"); }

        List<string> resultCount = new List<string>();

        var resultCounts = new Dictionary<double, List<string>>();

        // Step 1: Get all 4-number combinations
        var combinations = GetCombinations(numbers, 4);

        foreach (var combo in combinations)
        {
            // Step 2: Get all permutations of 4 numbers
            foreach (var nums in GetPermutations(combo, 4))
            {
                // Step 3: All operator combinations
                foreach (var ops in GetOperatorCombinations())
                {
                    // Step 4: Apply all parenthesis placements
                    var expressions = GetParenthesizedExpressions(nums, ops);
                    foreach (var expr in expressions)
                    {
                        try
                        {
                            var value = Evaluate(expr);
                            if (!double.IsNaN(value) && !double.IsInfinity(value))
                            {
                                double rounded = Math.Round(value, 2);

                                if (!resultCounts.ContainsKey(rounded))
                                {
                                    resultCounts[rounded] = new List<string>();
                                }
                                resultCounts[rounded].Add(expr);
                            }
                        }
                        catch { /* Invalid expressions, e.g., divide by zero */ }
                    }
                }
            }
        }

        return resultCounts;
    }

    static double Evaluate(string expression)
    {
        var table = new DataTable();
        object result = table.Compute(expression, "");
        return Convert.ToDouble(result);
    }

    static List<string> GetParenthesizedExpressions(double[] nums, string[] ops)
    {
        string a = nums[0].ToString("0.##");
        string b = nums[1].ToString("0.##");
        string c = nums[2].ToString("0.##");
        string d = nums[3].ToString("0.##");

        string op1 = ops[0];
        string op2 = ops[1];
        string op3 = ops[2];

        return new List<string>
        {
            $"{a}{op1}{b}{op2}{c}{op3}{d}",
            $"({a}{op1}{b}){op2}{c}{op3}{d}",
            $"{a}{op1}({b}{op2}{c}){op3}{d}",
            $"{a}{op1}{b}{op2}({c}{op3}{d})",
            $"({a}{op1}{b}){op2}({c}{op3}{d})"
        };
    }
    static IEnumerable<string[]> GetOperatorCombinations()
    {
        foreach (var op1 in stringPosibleOperator)
            foreach (var op2 in stringPosibleOperator)
                foreach (var op3 in stringPosibleOperator)
                    yield return new[] { op1, op2, op3 };
    }

    static IEnumerable<T[]> GetPermutations<T>(IList<T> list, int length)
    {
        if (length == 1)
            return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(o => !t.Contains(o)),
                        (t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
    }

    static List<List<T>> GetCombinations<T>(List<T> list, int length)
    {
        var result = new List<List<T>>();
        void Recurse(int start, List<T> current)
        {
            if (current.Count == length)
            {
                result.Add(new List<T>(current));
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                current.Add(list[i]);
                Recurse(i + 1, current);
                current.RemoveAt(current.Count - 1);
            }
        }
        Recurse(0, new List<T>());
        return result;
    }
    #endregion


    #region get target number base on GetMostFrequentResults() with 
    public static Dictionary<double, List<string>> GetAnswerByDifficulty(Dictionary<double, List<string>> resultCounts, double difficulty, double maxAnswerRange, bool isPositiveOnly)
    {
        if (resultCounts == null || resultCounts.Count == 0)
            throw new ArgumentException("Result counts are empty.");

        if (difficulty < 0 || difficulty > 1)
            throw new ArgumentOutOfRangeException(nameof(difficulty), "Difficulty must be between 0 and 1.");

        // only get round number
        var filtered = resultCounts
            .Where(kv => Math.Abs(kv.Key % 1) < 0.0001)
    .OrderByDescending(kv => kv.Value.Count)
    .ToList();

        //remove the answer that are too high or low
        filtered.RemoveAll(n => n.Key >= maxAnswerRange);
        if (isPositiveOnly)
        {
            filtered.RemoveAll(n => n.Key <= 0);
        } else
        {
            filtered.RemoveAll(n => n.Key <= -maxAnswerRange);
        }

            // Determine target index from percentile
            int index = (int)Math.Round(difficulty * (filtered.Count - 1));
        int targetCount = filtered[index].Value.Count();

        // Get all entries with the same count as target
        var sameCountKeys = filtered
            .Where(kv => kv.Value.Count() == targetCount)
            .Select(kv => kv.Key)
            .ToList();

        // Randomly pick one from them
        Random rng = new Random();
        int randIndex = rng.Next(sameCountKeys.Count);

        double key = sameCountKeys[randIndex];
        List<string> value = resultCounts[key];

        return new Dictionary<double, List<string>>() { { key, value } };
    }


    #endregion

    #region hand Validation
    public static int ValidationHand(List<GameObject> cards)
    {
        

        int numberCount = 0;
        // Validation check if first and last Gameobject have Card component
        if (cards.Count % 2 == 0 || cards[0].GetComponent<Card>() == null || cards[^1].GetComponent<Card>() == null)
        {
            //wrong order, the number must come first and last
            return -1;
        }

        // Validation check if the list got the number and operator zip in together
        for (int i = 0; i < cards.Count; i++)
        {
            if (i % 2 == 0 && cards[i].GetComponent<Card>() == null ||
                i % 2 == 1 && cards[i].GetComponent<Operatorcard>() == null)
            {
                //wrong order, operator and number must be zipped between each other
                return -2;
            }

            if (i % 2 == 0 && cards[i].GetComponent<Card>() != null)
            {
                numberCount++;
            }
        }

        return numberCount;
    }
    #endregion
}

