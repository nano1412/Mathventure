using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Equipment : Item
{
    [Header("Equipable Hero")]
    [field: SerializeField] public List<CharacterType> EquipableHeros { get; private set; }

    public override bool UseItem()
    {
        Debug.Log("you cant use equipmemt");
        return false;
    }
}
