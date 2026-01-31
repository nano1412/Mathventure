using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    private double multiplier = 1;
    private double leftcardValue = 0;
    private double rightcardValue = 0;

    [field: Header("Equipment"), SerializeField] public HeroEquipmentSlot HeroEquipmentSlot { get; private set; }
    [field: SerializeField] public GameObject Weapon { get; private set; }
    [field: SerializeField] public GameObject Armor { get; private set; }

    private void OnEnable()
    {
        HeroEquipmentSlot.WeaponSlot.OnItemChanged += HandleEquipmentChanged;
        HeroEquipmentSlot.ArmorSlot.OnItemChanged += HandleEquipmentChanged;
    }

    private void OnDisable()
    {
        HeroEquipmentSlot.WeaponSlot.OnItemChanged -= HandleEquipmentChanged;
        HeroEquipmentSlot.ArmorSlot.OnItemChanged -= HandleEquipmentChanged;
    }


    public void Attack(double mul, CardData leftAtkValue, CardData rightAtkValue)
    {
        Debug.Log("hero attack is called");
        targets = GetTargetByMove();

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

    public override void ResolveAttack()
    {
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<Character>())
            {
                target.GetComponent<Character>().TakeDamage((DefaultMove.Value + leftcardValue + rightcardValue) * multiplier, transform.name);
            }
        }
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
        Weapon = HeroEquipmentSlot.WeaponSlot.ItemInThisSlot;
        Armor = HeroEquipmentSlot.ArmorSlot.ItemInThisSlot;
    }
}
