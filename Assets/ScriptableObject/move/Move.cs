using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "move", menuName = "Scriptable Objects/move")]
public class Move : ScriptableObject
{
    [SerializeField] protected double motionValue;
    [SerializeField] protected TargetType targetType;
    [SerializeField] protected MoveType moveType;
    [SerializeField] protected AnimationType AnimationType;

    [SerializeField] protected List<ModifierSO> applyStatusViaAttack = new List<ModifierSO>();
}
