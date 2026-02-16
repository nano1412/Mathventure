using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class EquipmentData : MonoBehaviour
{
    [field:SerializeField] public List<CharacterType> UsableHero { get; private set; }

    [field: SerializeField] public EquipmentType EquipmentType { get; private set; }

    [field: SerializeField] public double IncreaseMaxHP { get; private set; }

    [field: SerializeField] public double IncreaseATK { get; private set; }

    [field: SerializeField] public CharacterBuff CharacterBuffInflictOnAttack { get; private set; }
}
