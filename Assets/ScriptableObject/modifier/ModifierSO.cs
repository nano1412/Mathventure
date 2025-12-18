using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "ModifierSO", menuName = "Scriptable Objects/ModifierSO")]
public class ModifierSO : ScriptableObject
{
    [Header("universal")]
    [SerializeField] private string modifierName;
    [SerializeField] private string discription;

    [SerializeField] private ModifierTarget modifierTarget;
    [SerializeField] private ModifierCondition modifierCondition;

    [SerializeField] private ModifierMethod modifierMethod;
    [SerializeField] private double modifierMethodValue;

    [Header("effect")]
    [SerializeField] private string effectName;
    [SerializeField] private EffectType effectType;
    [SerializeField] private double effectValue;

    [Header ("Consumable/effect")]
    [SerializeField] private int Duration;

    [Header("Card Condition")]
    [SerializeField] private int modifierConditionthreshold;
}
