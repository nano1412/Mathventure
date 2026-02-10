using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Hero : Character
{
    private double multiplier = 1;
    private double leftcardValue = 0;
    private double rightcardValue = 0;

    [field: Header("Equipment"), SerializeField] public HeroEquipmentSlot HeroEquipmentSlot { get; private set; }
    [field: SerializeField] public GameObject Weapon { get; private set; }
    [field: SerializeField] public GameObject Armor { get; private set; }
    [field: SerializeField] private double effectiveATK;

    private void OnEnable()
    {
        effectiveATK = CurrentMove.Value;
        HeroEquipmentSlot.WeaponSlot.OnItemChanged += HandleEquipmentChanged;
        HeroEquipmentSlot.ArmorSlot.OnItemChanged += HandleEquipmentChanged;
    }

    private void OnDisable()
    {
        HeroEquipmentSlot.WeaponSlot.OnItemChanged -= HandleEquipmentChanged;
        HeroEquipmentSlot.ArmorSlot.OnItemChanged -= HandleEquipmentChanged;
    }

    public Move GetMove(OperationEnum operationEnum)
    {
        if (transform.GetComponent<Character_Plugin_RandomMove>() != null)
        {
            return transform.GetComponent<Character_Plugin_RandomMove>().GetRandomMove();
        }

        if (transform.GetComponent<Hero_Plugin_BuffHeroMove>() != null)
        {
            return transform.GetComponent<Hero_Plugin_BuffHeroMove>().GetMoveByOperator(operationEnum);
        }



        return CurrentMove;
    }


    public void Attack(double mul, CardData leftAtkValue, CardData rightAtkValue, OperationEnum operationEnum)
    {
        Debug.Log("hero attack is called");

        CurrentMove = GetMove(operationEnum);

        targets = GetTargetByMove(CurrentMove);

        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Debug.Log(transform.name + " is attacking " + target.name + 
                    " with Lcard:Base(" + leftAtkValue.EffectiveValue + ") w/Buff(" + leftAtkValue.GetEffectiveValueWithBuff() +
                    ") Rcard:Base(" + rightAtkValue.EffectiveValue + ") w/Buff(" + rightAtkValue.GetEffectiveValueWithBuff() +
                    ") multiplier:" + mul);

            }
        }

        multiplier = mul;
        leftcardValue = leftAtkValue.GetEffectiveValueWithBuff();
        rightcardValue = rightAtkValue.GetEffectiveValueWithBuff();

    animator.SetTrigger("Attack");

    }

    public override double GetEffectiveAttackValue()
    {
        double tempATK = effectiveATK;

        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            switch (characterBuff.CharacterBuffTargetValue)
            {
                case CharacterBuffTargetValue.ATK:
                    tempATK += characterBuff.BuffMethodCalculation(tempATK);
                    break;
                default:
                    continue;
            }
        }

        return tempATK;
    }

    public override void CheckDead()
    {
        if (Hp <= 0)
        {
            Debug.Log(transform.name + "is dead");
            Dead();

            GameController.current.Lose();
        }
    }

    void HandleEquipmentChanged()
    {
        effectiveATK = CurrentMove.Value;
        Weapon = HeroEquipmentSlot.WeaponSlot.ItemInThisSlot;
        Armor = HeroEquipmentSlot.ArmorSlot.ItemInThisSlot;

        EquipmentData weaponEquipmentData = Weapon != null? Weapon.GetComponent<EquipmentData>():null;
        EquipmentData armorEquipmentData = Armor != null ? Armor.GetComponent<EquipmentData>() : null;

        if (weaponEquipmentData != null)
        {
            effectiveATK += weaponEquipmentData.IncreaseATK;
        }
        if (armorEquipmentData != null)
        {
            effectiveATK += armorEquipmentData.IncreaseATK;
        }

        UpdateEffectiveMaxHP();
    }

    protected override double IncreaseMaxHPViaEquipment(double tempMaxHP)
    {
        EquipmentData weaponEquipmentData = Weapon != null ? Weapon.GetComponent<EquipmentData>() : null;
        EquipmentData armorEquipmentData = Armor != null ? Armor.GetComponent<EquipmentData>() : null;

        if (weaponEquipmentData != null)
        {
            tempMaxHP += weaponEquipmentData.IncreaseMaxHP;
        }
        if (armorEquipmentData != null)
        {
            tempMaxHP += armorEquipmentData.IncreaseMaxHP;
        }
        return tempMaxHP;
    }
}
