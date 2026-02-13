using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class HeroToggler : MonoBehaviour
{
    public static HeroToggler current;

    [field: Header("Plus"), SerializeField] public bool IsPlusHeroOnThisLevel{ get; private set; }
    [field: SerializeField] public GameObject PlusHeroSlot { get; private set; }
    [field: SerializeField] public GameObject PlusHeroInventory { get; private set; }
    [field: SerializeField] public GameObject PlusOperatorBtn { get; private set; }

    [field: Header("Minus"), SerializeField] public bool IsMinusHeroOnThisLevel { get; private set; }
    [field: SerializeField] public GameObject MinusHeroSlot { get; private set; }
    [field: SerializeField] public GameObject MinusHeroInventory { get; private set; }
    [field: SerializeField] public GameObject MinusOperatorBtn { get; private set; }

    [field: Header("Multiply"), SerializeField] public bool IsMultiplyHeroOnThisLevel { get; private set; }
    [field: SerializeField] public GameObject MultiplyHeroSlot { get; private set; }
    [field: SerializeField] public GameObject MultiplyHeroInventory { get; private set; }
    [field: SerializeField] public GameObject MultiplyOperatorBtn { get; private set; }

    [field: Header("Divide"), SerializeField] public bool IsDivideHeroOnThisLevel { get; private set; }
    [field: SerializeField] public GameObject DivideHeroSlot { get; private set; }
    [field: SerializeField] public GameObject DivideHeroInventory { get; private set; }
    [field: SerializeField] public GameObject DivideOperatorBtn { get; private set; }

    [field: Header("Buff"), SerializeField] public bool IsBuffHeroOnThisLevel { get; private set; }
    [field: SerializeField] public GameObject BuffHeroSlot { get; private set; }
    [field: SerializeField] public GameObject BuffHeroInventory { get; private set; }

    private void Awake()
    {
        current = this;
    }

    public void SetupEnableHeroViaPossibleOperator(List<OperationEnum> possibleOperators)
    {
        if (!possibleOperators.Contains(OperationEnum.Plus)){
            IsPlusHeroOnThisLevel = false;
            PlusHeroSlot.SetActive(false);
            PlusHeroInventory.SetActive(false);
            PlusOperatorBtn.SetActive(false);
        }

        if (!possibleOperators.Contains(OperationEnum.Minus))
        {
            IsMinusHeroOnThisLevel = false;
            MinusHeroSlot.SetActive(false);
            MinusHeroInventory.SetActive(false);
            MinusOperatorBtn.SetActive(false);
        }

        if (!possibleOperators.Contains(OperationEnum.Multiply))
        {
            IsMultiplyHeroOnThisLevel = false;
            MultiplyHeroSlot.SetActive(false);
            MultiplyHeroInventory.SetActive(false);
            MultiplyOperatorBtn.SetActive(false);
        }

        if (!possibleOperators.Contains(OperationEnum.Divide))
        {
            IsDivideHeroOnThisLevel = false;
            DivideHeroSlot.SetActive(false);
            DivideHeroInventory.SetActive(false);
            DivideOperatorBtn.SetActive(false);
        }

        if (!possibleOperators.Contains(OperationEnum.Buff))
        {
            IsBuffHeroOnThisLevel = false;
            BuffHeroSlot.SetActive(false);
            BuffHeroInventory.SetActive(false);
        }
    }
}
