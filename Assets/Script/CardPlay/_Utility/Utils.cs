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

    public enum EffectType
    {
        None,
        Heal,
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
        Mode0, // No parentheses
        Mode1, // (XX)XX
        Mode2, // X(XX)X
        Mode3, // XX(XX)
        Mode4  // (XX)(XX)
    }


    public static double RoundUpToDecimalPlaces(double value, int decimalPlaces)
    {
        double multiplier = Math.Pow(10, decimalPlaces);
        if (value >= 0)
            return Math.Ceiling(value * multiplier) / multiplier;
        else
            return Math.Floor(value * multiplier) / multiplier;
    }
}
