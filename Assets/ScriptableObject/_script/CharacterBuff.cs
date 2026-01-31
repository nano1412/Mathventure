using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "CharacterBuff", menuName = "Scriptable Objects/Buff/CharacterBuff")]
public class CharacterBuff : ScriptableObject
{
    [field: SerializeField] public bool IsEffectInstant { get; private set; }
    //IsEffectInstant only work if it target hp (for something like bomb or potion)
    [field: SerializeField] public int Value { get; private set; }
    //Value will work as percentile if BuffMethod select to be multiply
    [field: SerializeField] public BuffMethod BuffMethod { get; private set; }
    [field: SerializeField] public CharacterBuffTargetValue CharacterBuffTargetValue { get; private set; }
    
    [field: SerializeField] public int Duration { get; private set; } = 1;

    public void ReduceDuration(int duration)
    {
        Duration -= duration;
    }

    public double BuffMethodCalculation(double value)
    {
        switch (BuffMethod)
        {
            case BuffMethod.AddValue:
                return Value;
            case BuffMethod.MultiplyValue:
                return value * (Value - 1);
                //this function is called in Character.TakeBuffsEffect
                //so it relay that on that part have use += to store value
                //that why it has to be (Value - 1)
        }
        return value;
    }
}
