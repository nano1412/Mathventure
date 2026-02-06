using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class ConsumableData : MonoBehaviour
{
    [field: SerializeField] public List<CharacterType> UsableOn { get; private set; } = new List<CharacterType>();
    [field: SerializeField] public List<CharacterBuff> CharacterBuffOnUse { get; private set; } = new List<CharacterBuff>();
    [field: SerializeField] public List<CardBuff> CardBuffOnUse { get; private set; } = new List<CardBuff>();
    
}
