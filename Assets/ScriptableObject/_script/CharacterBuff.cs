using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "CharacterBuff", menuName = "Scriptable Objects/Buff/CharacterBuff")]
public class CharacterBuff : ScriptableObject
{
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public BuffMethod BuffMethod { get; private set; }
    [field: SerializeField] public CharacterBuffTargetValue CharacterBuffTargetValue { get; private set; }
    
    [field: SerializeField] public int Duration { get; private set; } = 1;

    public void ReduceDuration()
    {
        Duration -= 1;
    }
}
