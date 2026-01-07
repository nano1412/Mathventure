using UnityEngine;
using static Utils;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardBuff", menuName = "Scriptable Objects/Buff/CardBuff")]
public class CardBuff : ScriptableObject
{
    [field:SerializeField] public int Value { get; private set; }
    [field: SerializeField] public BuffMethod buffMethod { get; private set; }
    [field: SerializeField] public List<CardBuffCondition> CardBuffConditions { get; private set; }
    [field: SerializeField] public double ThresholdValue { get; private set; }
    [field: SerializeField] public int Duration { get; private set; } = 1;

    public void ReduceDuration()
    {
        Duration -= 1;
    }
}
