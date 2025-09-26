using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "move", menuName = "Scriptable Objects/move")]
public class Move : ScriptableObject
{
    [SerializeField] protected double attackDamage;
    [SerializeField] protected double damageMultiplier;
    [SerializeField] protected AttackType attackType;
    [SerializeField] protected AnimationType AnimationType;

    [SerializeField] protected List<StatusEffect> applyStatusViaAttack = new List<StatusEffect>();

    public void SetAttackDamage(double value) //use with hero card calculation
    {
        attackDamage = value;
    }
}
