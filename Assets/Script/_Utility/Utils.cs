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

    public enum ItemType { Equipment, Consumable, General_Item }

    public enum EquipmentType { Weapon, Armor }

    public enum SlotType { Weapon, Armor, Equipment, Consumable, Shop, Display }

    public enum CharacterType { Hero_Plus, Hero_Minus, Hero_Multiply, Hero_Divide, Hero_Buff, Enemy }

    public enum CardBuffCondition
    {
        Card_All,
        Card_IsEven,
        Card_IsOdd,
        Card_IsPrime,
        Card_Morethan,
        Card_Lessthan,
        Card_Equal,

        
    }

    public enum CharacterBuffCondition
    {
        Hero_All,
        Hero_Plus,
        Hero_Minus,
        Hero_Multiply,
        Hero_Divide,
        Hero_OnlyEquiper,
        Hero_OnlySelected,

        Enemy_All,
        Enemy_Front,
        Enemy_Back,
        Enemy_FirstTwo,
        Enemy_OnlySelected
    }

    public enum BuffMethod
    {
        AddValue,
        MultiplyValue,
    }

    public enum CharacterBuffTargetValue
    {
        HP,
        ATK,
    }

    public class StatusEffect
    {
        public string name;
        public EffectType effectType;
        public double value;
        public int duration;

        public StatusEffect(string name, EffectType effectType, double value, int duration)
        {
            this.name = name;
            this.effectType = effectType;
            this.value = value;
            this.duration = duration;
        }
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

    public static bool IsPrimeFromDouble(double value)
    {
        // Round the value
        long n = (long)Math.Round(value);

        // Prime numbers must be > 1
        if (n <= 1)
            return false;

        // 2 is the only even prime
        if (n == 2)
            return true;

        // Eliminate even numbers
        if (n % 2 == 0)
            return false;

        long limit = (long)Math.Sqrt(n);

        for (long i = 3; i <= limit; i += 2)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }
}
