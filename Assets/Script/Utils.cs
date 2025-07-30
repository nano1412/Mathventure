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


    public static double RoundUpToDecimalPlaces(double value, int decimalPlaces)
    {
        double multiplier = Math.Pow(10, decimalPlaces);
        if (value >= 0)
            return Math.Ceiling(value * multiplier) / multiplier;
        else
            return Math.Floor(value * multiplier) / multiplier;
    }
}
