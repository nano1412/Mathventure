using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public abstract class Character : MonoBehaviour
{
    [field: Header("Character"), SerializeField] public CharacterType CharacterType { get; private set; }

    [field: Header("Hp and status"),SerializeField] public double MaxHp { get; private set; }
    [field: SerializeField] public double Hp { get; private set; }
    [field: SerializeField] public double Shield { get; private set; }
    [field: SerializeField] public bool IsStuned { get; private set; }

    [field: SerializeField] public List<StatusEffect> StatusEffects { get; private set; } = new List<StatusEffect>();

    [field: Header("Attack data"), SerializeField] public Move DefaultMove { get; private set; }

    public void TakeDamage(double damage, string attacker)
    {
        if (damage < 0)
        {
            Debug.LogWarning("TakeDamage received a negative value. Use Heal() instead.");
            return;
        }

        Hp -= damage - Shield;
        Debug.Log(transform.name + " take " + damage + " damages from " + attacker);
    }

    public void Heal(double heal, string healer)
    {
        if (heal < 0)
        {
            Debug.LogWarning("Heal received a negative value. Use TakeDamage() instead.");
            return;
        }

        Hp += heal;
        Debug.Log(transform.name + " gain " + heal + " Hp from " + healer);
    }

    public virtual void CheckDead()
    {
        if(Hp < 0)
        {
            Debug.Log(transform.name + "is dead");
            Dead();
        }
    }

    protected void Dead()
    {
        Destroy(gameObject);
    }


    public void EndTurnUpdate()
    {
        if (Hp <= 0)
        {
            Debug.Log(transform.name + "is dead");
            //play ded animation here
        }

        for (int i = StatusEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect statusEffect = StatusEffects[i];
            UpdateStatusEffect(statusEffect, i);
        }

    }

    private void UpdateStatusEffect(StatusEffect statusEffect, int index)
    {
        //do the effect
        switch (statusEffect.effectType)
        {
            case EffectType.HealOvertime:
                Heal(statusEffect.value, statusEffect.name);

                break;

            case EffectType.DamageOveertime:
                TakeDamage(statusEffect.value, statusEffect.name);

                break;

            case EffectType.Stun:
                IsStuned = true;
                break;

        }


        statusEffect.duration--;
        if(statusEffect.duration <= 0)
        {
            Debug.Log(statusEffect.name + " has been resolve");

            if(statusEffect.effectType == EffectType.Stun)
            {
                IsStuned = false;
                Debug.Log(transform.name + " regain consciousness");
            }
            StatusEffects.RemoveAt(index);
        }
    }
}

