using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.GPUSort;
using static Utils;
using Random = System.Random;

#region calculate card in hand

public class EquationObject
{
    public List<double> numbers;
    public List<OperationEnum> operationEnums;
    public ParenthesesMode parenthesesMode;
    public EquationObject(List<double> numbers, List<OperationEnum> operationEnums, ParenthesesMode parenthesesMode)
    {
        this.numbers = numbers;
        this.operationEnums = operationEnums;
        this.parenthesesMode = parenthesesMode;
    }

    public double GetAnswer()
    {
        List<double> tempNumbers = new List<double>(numbers);
        List<OperationEnum> tempOperations = new List<OperationEnum>(operationEnums);
        List<double> operatorsPriority = new List<double>() { 1, 1, 1 };

        for(int i = 0;i< operationEnums.Count;i++)
        {
            if (operationEnums[i] == OperationEnum.Multiply || operationEnums[i] == OperationEnum.Divide)
            {
                operatorsPriority[i] = 2;
            }
        }

        switch (parenthesesMode)
        {
            
            case ParenthesesMode.DoFrontOperationFirst:
                operatorsPriority[0] = 3;
                break;
            case ParenthesesMode.DoMiddleOperationFirst:
                operatorsPriority[1] = 3;
                break;
            case ParenthesesMode.DoLastOperationFirst:
                operatorsPriority[2] = 3;
                break;
            case ParenthesesMode.DoMiddleOperationLast:
                operatorsPriority[1] = 0;
                break;
            default:
                //ParenthesesMode.NoParentheses
                break;
        }

        List<int> operatorOrder = operatorsPriority
        .Select((value, index) => new { value, index })
        .OrderByDescending(x => x.value)
        .Select(x => x.index)
        .ToList();

        foreach (int originalIndex in operatorOrder)
        {
            if (originalIndex >= tempOperations.Count)
                continue;

            double result = Operate(
                tempNumbers[originalIndex],
                tempNumbers[originalIndex + 1],
                tempOperations[originalIndex]);

            tempNumbers[originalIndex] = result;
            tempNumbers.RemoveAt(originalIndex + 1);
            tempOperations.RemoveAt(originalIndex);
        }
        
        return Math.Round(tempNumbers[0], 2);
    }

    public double Operate(double a, double b, OperationEnum operation)
    {
        switch (operation)
        {
            case OperationEnum.Plus:
                return a + b;
            case OperationEnum.Minus:
                return a - b;
            case OperationEnum.Multiply:
                return a * b;
            case OperationEnum.Divide:
                return a / b;
        }

        return double.NegativeInfinity;
    }

    public string GetEquation()
    {
        switch (parenthesesMode)
        {
            case ParenthesesMode.DoFrontOperationFirst:
                return "(" + numbers[0] + GetStringFromOperatorEnum(operationEnums[0]) +
                    numbers[1] + ")" + GetStringFromOperatorEnum(operationEnums[1]) +
                    numbers[2] + GetStringFromOperatorEnum(operationEnums[2]) +
                    numbers[3];
            case ParenthesesMode.DoMiddleOperationFirst:
                return numbers[0] + GetStringFromOperatorEnum(operationEnums[0]) +
                    "(" + numbers[1] + GetStringFromOperatorEnum(operationEnums[1]) +
                    numbers[2] + ")" + GetStringFromOperatorEnum(operationEnums[2]) +
                    numbers[3];
            case ParenthesesMode.DoLastOperationFirst:
                return numbers[0] + GetStringFromOperatorEnum(operationEnums[0]) +
                    numbers[1] + GetStringFromOperatorEnum(operationEnums[1]) +
                    "(" + numbers[2] + GetStringFromOperatorEnum(operationEnums[2]) +
                    numbers[3] + ")";
            case ParenthesesMode.DoMiddleOperationLast:
                return "(" + numbers[0] + GetStringFromOperatorEnum(operationEnums[0]) +
                    numbers[1] + ")" + GetStringFromOperatorEnum(operationEnums[1]) +
                    "(" + numbers[2] + GetStringFromOperatorEnum(operationEnums[2]) +
                    numbers[3] + ")";
            default:
                //ParenthesesMode.NoParentheses
                return numbers[0] + GetStringFromOperatorEnum(operationEnums[0]) +
                    numbers[1] + GetStringFromOperatorEnum(operationEnums[1]) +
                    numbers[2] + GetStringFromOperatorEnum(operationEnums[2]) +
                    numbers[3];
        }
    }
}


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
                handEquation[i].GetComponent<CardEntity>() != null ? handEquation[i].GetComponent<CardData>().FaceValue : 0,
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
        //Debug.Log("Parentheses Mode " + mode);
        switch (mode)
        {
            case ParenthesesMode.DoFrontOperationFirst:
                if (cards.Count > 1 && cards[1] != null)
                {
                    cards[1].priority = 3;
                }
                break;
            case ParenthesesMode.DoMiddleOperationFirst:
                if (cards.Count > 3 && cards[3] != null)
                {
                    cards[3].priority = 3;
                }
                break;
            case ParenthesesMode.DoLastOperationFirst:
                if (cards.Count > 5 && cards[5] != null)
                {
                    cards[5].priority = 3;
                }
                break;
            case ParenthesesMode.DoMiddleOperationLast:
                if (cards.Count > 1 && cards[1] != null)
                {
                    cards[1].priority = 3;
                }

                if (cards.Count > 5 && cards[5] != null)
                {
                    cards[5].priority = 3;
                }
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

    public static Dictionary<double, List<EquationObject>> GetMostFrequentResults(List<double> numbers, List<OperationEnum> posibleOperators)
    {
        stringPosibleOperator = new List<string>();
        if (posibleOperators.Contains(OperationEnum.Plus)) { stringPosibleOperator.Add("+"); }
        if (posibleOperators.Contains(OperationEnum.Minus)) { stringPosibleOperator.Add("-"); }
        if (posibleOperators.Contains(OperationEnum.Multiply)) { stringPosibleOperator.Add("*"); }
        if (posibleOperators.Contains(OperationEnum.Divide)) { stringPosibleOperator.Add("/"); }

        var resultCounts = new Dictionary<double, List<EquationObject>>();

        //optimize: preselect number
        List<double> randomFour = numbers
            .OrderBy(x => UnityEngine.Random.value)
            .Take(4)
            .ToList();

        List<List<double>> numberCombinations = GetCombinations(numbers, 4);
        List<List<OperationEnum>> operatorCombinations = GetOperatorCombinations(posibleOperators);
        List<ParenthesesMode> parentheses = new List<ParenthesesMode>() { 
            ParenthesesMode.NoParentheses, 
            ParenthesesMode.DoFrontOperationFirst, 
            ParenthesesMode.DoMiddleOperationFirst, 
            ParenthesesMode.DoLastOperationFirst, 
            ParenthesesMode.DoMiddleOperationLast
        };

        foreach(List<double> numberCombination in numberCombinations)
        {
            foreach(List<OperationEnum> operatorCombination in operatorCombinations)
            {
                foreach (ParenthesesMode parenthese in parentheses)
                {
                    EquationObject equationObject = new EquationObject(numberCombination, operatorCombination, parenthese);
                    double equationAns = equationObject.GetAnswer();

                    if (!double.IsNaN(equationAns) && !double.IsInfinity(equationAns))
                    {

                        if (!resultCounts.ContainsKey(equationAns))
                        {
                            resultCounts[equationAns] = new List<EquationObject>();
                        }
                        resultCounts[equationAns].Add(equationObject);
                    }

                }
            }
        }
        return resultCounts;
    }

    static List<List<OperationEnum>> GetOperatorCombinations(List<OperationEnum> posibleOperators)
    {
        List<OperationEnum> posibleOperators_Nobuff = posibleOperators;
        posibleOperators_Nobuff.Remove(OperationEnum.Buff);
        List < List < OperationEnum >> tempOperatorCom = new List<List<OperationEnum>> ();
        foreach (OperationEnum op1 in posibleOperators_Nobuff)
        {
            foreach (OperationEnum op2 in posibleOperators_Nobuff)
            {
                foreach (OperationEnum op3 in posibleOperators_Nobuff)
                {
                    tempOperatorCom.Add(new List<OperationEnum> { op1, op2,op3 });

                    Debug.Log(op1.ToString() + " " + op2.ToString() + " " + op3.ToString());
                }

            }

        }
        return tempOperatorCom;
    }

    static IEnumerable<T[]> GetPermutations<T>(IList<T> list, int length)
    {
        if (length == 1)
            return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(o => !t.Contains(o)),
                        (t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
    }

    static List<List<OperationEnum>> GetCombinations<OperationEnum>(List<OperationEnum> list, int length)
    {
        var result = new List<List<OperationEnum>>();
        void Recurse(int start, List<OperationEnum> current)
        {
            if (current.Count == length)
            {
                result.Add(new List<OperationEnum>(current));
                return;
            }

            for (int i = start; i < list.Count; i++)
            {
                current.Add(list[i]);
                Recurse(i + 1, current);
                current.RemoveAt(current.Count - 1);
            }
        }
        Recurse(0, new List<OperationEnum>());
        return result;
    }
    #endregion


    #region get target number base on GetMostFrequentResults() with 
    public static Dictionary<double, List<EquationObject>> GetAnswerByDifficulty(Dictionary<double, List<EquationObject>> resultCounts, double difficulty, double maxAnswerRange, bool isPositiveOnly)
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
        }
        else
        {
            filtered.RemoveAll(n => n.Key <= -maxAnswerRange);
        }

        // Determine target index from percentile
        int index = (int)Math.Round(difficulty * (filtered.Count - 1));
        //Debug.Log("index of target answer: " + index);
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
        List<EquationObject> value = resultCounts[key];

        return new Dictionary<double, List<EquationObject>>() { { key, value } };
    }


    #endregion

    #region hand Validation
    public static int ValidationHand(List<GameObject> cards)
    {


        int numberCount = 0;
        // Validation check if first and last Gameobject have CardEntity component
        if (cards.Count % 2 == 0 || cards[0].GetComponent<CardEntity>() == null || cards[^1].GetComponent<CardEntity>() == null)
        {
            //wrong order, the number must come first and last
            return -1;
        }

        // Validation check if the list got the number and operator zip in together
        for (int i = 0; i < cards.Count; i++)
        {
            if (i % 2 == 0 && cards[i].GetComponent<CardEntity>() == null ||
                i % 2 == 1 && cards[i].GetComponent<Operatorcard>() == null)
            {
                //wrong order, operator and number must be zipped between each other
                return -2;
            }

            if (i % 2 == 0 && cards[i].GetComponent<CardEntity>() != null)
            {
                numberCount++;
            }
        }

        return numberCount;
    }
    #endregion
}

