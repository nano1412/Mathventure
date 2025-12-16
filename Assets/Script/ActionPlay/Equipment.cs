using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Equipment : Item
{
    [Header("Equipable Hero")]
    [field: SerializeField] public List<HeroType> EquipableHeros { get; private set; }
}
