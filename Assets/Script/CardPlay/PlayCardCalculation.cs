using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;
using static Utils;

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
        public static List<object[]> EvaluateEquation(List<GameObject> handEquation, ParenthesesMode mode)
        {
            List<object[]> resultLog = new List<object[]>();

            // Validation check if first and last Gameobject have Card component
            if (handEquation.Count % 2 == 0 || handEquation[0].GetComponent<Card>() == null || handEquation[^1].GetComponent<Card>() == null)
            {
                Debug.Log("wrong order, the number must come first and last");
                return null;
            }

            // Validation check if the list got the number and operator zip in together
            for (int i = 0; i < handEquation.Count; i++)
            {
                if (i % 2 == 0 && handEquation[i].GetComponent<Card>() == null ||
                    i % 2 == 1 && handEquation[i].GetComponent<Operatorcard>() == null)
                {
                    Debug.Log("wrong order, operator and number must be zipped between each other");
                    return null;
                }
            }


            // Convert to simplified struct
            List<SimplifiedCard> simplified = new List<SimplifiedCard>();
            for (int i = 0; i < handEquation.Count; i++)
            {
                SimplifiedCard sCard = new SimplifiedCard(
                    handEquation[i].GetComponent<Card>() != null ? handEquation[i].GetComponent<Card>().GetFaceValue() : 0,
                    handEquation[i].GetComponent<Operatorcard>() != null ? handEquation[i].GetComponent<Operatorcard>().GetOperator() : null,
                    i);

                simplified.Add(sCard);
            }

            // Apply parentheses collapsing
            ApplyParenthesesMode(simplified, mode);

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
                case ParenthesesMode.Mode1:
                    cards[1].priority = 3;
                    break;
                case ParenthesesMode.Mode2:
                    cards[3].priority = 3;
                    break;
                case ParenthesesMode.Mode3:
                    cards[5].priority = 3;
                    break;
                case ParenthesesMode.Mode4:
                    cards[1].priority = 3;
                    cards[5].priority = 3;
                    break;
            }
        }


        private static string BuildEquationString(List<SimplifiedCard> cards)
        {
            return string.Join("", cards);
        }
    }
   
