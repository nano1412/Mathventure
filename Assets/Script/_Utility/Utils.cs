using System;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    public enum OperationEnum
    {
        Plus,
        Minus,
        Multiply,
        Divide
    }

    public enum TargetType
    {
        front,
        back,
        firstTwo,
        all,
        allFriendly
    }

    public enum MoveType
    {
        RawAttack,
        ApplyStatusToEnemy,
        ApplyStatusToFriendly
    }

    public enum EffectType
    {
        None,
        HealOvertime,
        DamageOveertime,
        Stun,
        Empty
        // Add more as needed
    }

    public enum AnimationType
    {
        GetClose,
        Casting,
        Projectile,
        ToTheBackOfOpponentLine,
        InFrontOfOpponentLine
    }

    public enum OperationPriority
    {
        NotOperator,
        AdditionAndSubtraction,
        MultiplicationAndDivision,
        Parentheses

    }

    public enum CardType { Number, Operator }

    public enum ItemType { Weapon, Armor, Equipment, Consumable, General_Item }

    public enum SlotType { Weapon, Armor, Equipment, Consumable, Shop, Display }

    public enum CharacterType { Hero_Plus, Hero_Minus, Hero_Multiply, Hero_Divide, Hero_Buff, Enemy }

    public enum ModifierCondition
    {
        Card_All,
        Card_IsEven,
        Card_IsOdd,
        Card_IsPrime,
        Card_Morethan,
        Card_Lessthan,
        Card_Equal,

        Hero_All,
        Hero_Plus,
        Hero_Minus,
        Hero_Multiply,
        Hero_Divide,
        Hero_OnlyEquiper,

        Enemy_All,
        Enemy_Front,
        Enemy_Back,
        Enemy_FirstTwo,
        Enemy_RecriveAttack
    }

    public enum ModifierMethod
    {
        AddValue,
        TemporaryAddValue,
        MultiplyValue,
        TemporaryMultiplyValue,
        AddEffect,
        RemoveEffect
    }

    public enum ModifierTarget
    {
        HP,
        ATK,
        DEF
    }

    public class StatusEffect
    {
        public string name;
        public EffectType effectType;
        public int value;
        public int duration;
    }

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

    public enum GameState
    {
        PlayerInput,
        CardCalculation,
        HeroAttack,
        EnemyAttack,
        Win,
        RoundVictory,
        Lose,
        Shop,
        Pause,
        Menu

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

    public static Random rnd = new Random();
}
