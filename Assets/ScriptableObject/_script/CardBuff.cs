using UnityEngine;
using static Utils;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardBuff", menuName = "Scriptable Objects/Buff/CardBuff")]
public class CardBuff : ScriptableObject
{
    [field:SerializeField] public int Value { get; private set; }
    [field: SerializeField] public BuffMethod buffMethod { get; private set; }
    [field: SerializeField] public List<CardBuffCondition> CardBuffCondition { get; private set; }
    [field: SerializeField] public double ThresholdValue { get; private set; }
}
