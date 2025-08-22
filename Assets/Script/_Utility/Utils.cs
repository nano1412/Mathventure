using System;
using UnityEngine;

public static class Utils
{
    public enum OperationEnum
    {
        Plus,
        Minus,
        Multiply,
        Divide
    }

    public enum AttackType
    {
        front,
        back,
        firstTwo,
        all,
    }

    public enum EffectType
    {
        None,
        OvertimeHeal,
        Poison,
        Stun,
        Empty
        // Add more as needed
    }

    public enum OperationPriority
    {
        NotOperator,
        AdditionAndSubtraction,
        MultiplicationAndDivision,
        Parentheses

    }

    public enum CardType { Number, Operator }

    public enum ParenthesesMode
    {
        NoParentheses, // NoParentheses
        DoFrontOperationFirst, // (XX)XX
        DoMiddleOperationFirst, // X(XX)X
        DoLastOperationFirst, // XX(XX)
        DoMiddleOperationLast  // (XX)(XX)
    }

    public enum OperatorOrder
    {
        firstOperator = 1,
        middleOperator = 3,
        lastOperator = 5
    }


    public static double RoundUpToDecimalPlaces(double value, int decimalPlaces)
    {
        double multiplier = Math.Pow(10, decimalPlaces);
        if (value >= 0)
            return Math.Ceiling(value * multiplier) / multiplier;
        else
            return Math.Floor(value * multiplier) / multiplier;
    }

    public static bool RectOverlaps(RectTransform rt1, RectTransform rt2)
    {
        Rect r1 = GetWorldRect(rt1);
        Rect r2 = GetWorldRect(rt2);
        return r1.Overlaps(r2);
    }

    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}
