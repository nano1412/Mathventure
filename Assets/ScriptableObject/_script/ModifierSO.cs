using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "ModifierSO", menuName = "Scriptable Objects/Action/ModifierSO")]
public class ModifierSO : ScriptableObject
{
    [Header("universal")]
    [field: SerializeField] public string ModifierName { get; private set; }
    [field: SerializeField] public string Discription { get; private set; }

    [field: SerializeField] public ModifierTarget ModifierTarget { get; private set; }
    [field: SerializeField] public ModifierCondition ModifierCondition { get; private set; }

    [field: SerializeField] public ModifierMethod ModifierMethod { get; private set; }
    [field: SerializeField] public double ModifierMethodValue { get; private set; }

    [Header("effect")]
    [field: SerializeField] public string EffectName { get; private set; }
    [field: SerializeField] public EffectType EffectType { get; private set; }
    [field: SerializeField] public double EffectValue { get; private set; }

    [Header("Consumable/effect")]
    [field: SerializeField] public int Duration { get; private set; }

    [Header("Card Condition")]
    [field: SerializeField] public int ModifierConditionthreshold { get; private set; }
}
