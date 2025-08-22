using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    public string effectName;
    public EffectType effectType;
    public double value;
    public int turnDuration;
}
