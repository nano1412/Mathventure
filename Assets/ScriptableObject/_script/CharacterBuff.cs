using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "CharacterBuff", menuName = "Scriptable Objects/Buff/CharacterBuff")]
public class CharacterBuff : ScriptableObject
{
    public int Value { get; private set; }
    public BuffMethod BuffMethod { get; private set; }
    public CharacterBuffTargetValue CharacterBuffTargetValue { get; private set; }
    public List<CharacterBuffCondition> CharacterBuffCondition { get; private set; }
}
