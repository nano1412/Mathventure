using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Scriptable Objects/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    public EffectType effectType;
    public double value;
    public int turnDuration;
}
