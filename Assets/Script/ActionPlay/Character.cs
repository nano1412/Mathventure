using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public abstract class Character : MonoBehaviour
{
    [Header("hp and status")]
    [SerializeField] protected double maxHp;
    [SerializeField] protected double hp;
    [SerializeField] protected double shield;
    [SerializeField] protected bool isStuned;

    [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    [Header("Attack data")]
    [SerializeField] Move defaultAttack;

    public double GetMaxHP()
    {
        return maxHp;
    }

    public double GetHP()
    {
        return hp;
    }

    public double GetShield()
    {
        return shield;
    }

    public void TakeDamage(double damage, string owner)
    {
        hp -= damage;
        Debug.Log(transform.name + " take " + damage + " damages from " + name);
    }

    public void Heal(double heal, string owner)
    {
        hp += heal;
        Debug.Log(transform.name + " gain " + heal + " hp from " + name);
    }

    

    public void EndTurnUpdate()
    {
        if (hp <= 0)
        {
            Debug.Log(transform.name + "is dead");
            //play ded animation here
        }

        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect statusEffect = statusEffects[i];
            UpdateStatusEffect(statusEffect, i);
        }

    }

    private void UpdateStatusEffect(StatusEffect statusEffect, int index)
    {
        //do the effect
        switch (statusEffect.effectType)
        {
            case EffectType.OvertimeHeal:
                Heal(statusEffect.value, statusEffect.name);

                break;

            case EffectType.Poison:
                TakeDamage(statusEffect.value, statusEffect.name);

                break;

            case EffectType.Stun:
                isStuned = true;
                break;

        }


        statusEffect.turnDuration--;
        if(statusEffect.turnDuration <= 0)
        {
            Debug.Log(statusEffect.effectName + " has been resolve");

            if(statusEffect.effectType == EffectType.Stun)
            {
                isStuned = false;
                Debug.Log(transform.name + " regain consciousness");
            }
            statusEffects.RemoveAt(index);
        }
    }

}

