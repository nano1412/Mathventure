using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "move", menuName = "Scriptable Objects/move")]
public class Move : ScriptableObject
{
    [SerializeField] protected double attackDamage;
    [SerializeField] protected AttackType attackType;

    [SerializeField] protected List<StatusEffect> applyStatusViaAttack = new List<StatusEffect>();
}
