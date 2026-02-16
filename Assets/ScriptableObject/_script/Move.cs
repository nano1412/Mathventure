using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "move", menuName = "Scriptable Objects/Action/move")]
public class Move : ScriptableObject
{

    [field: SerializeField] public double Value { get; private set; }
    [field: SerializeField] public AttackTargetType TargetType { get; private set; }
    [field: SerializeField] public MoveType MoveType { get; private set; }
    [field: SerializeField] public AnimationType AnimationType { get; private set; }

    [field: SerializeField] public List<CharacterBuff> ApplyStatusViaAttack { get; private set; } = new List<CharacterBuff>();
}
