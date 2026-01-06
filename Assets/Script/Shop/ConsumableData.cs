using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class ConsumableData : MonoBehaviour
{
    [field: SerializeField] public List<CharacterType> UsableCharacter { get; private set; }
    [field: SerializeField] public CharacterBuff CharacterBuffOnUse { get; private set; }
    [field: SerializeField] public CardBuff CardBuffOnUse { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
