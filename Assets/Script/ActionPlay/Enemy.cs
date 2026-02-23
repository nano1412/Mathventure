using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [field:SerializeField]public int CoinOnKill { get; private set; }

    [field:Header("Endless Data"), SerializeField] public int EndlessSpawnableOnWaveMoreThan{ get; private set; }
    [field:SerializeField] public int EndlessValue { get; private set; }
    public void Attack()
    {
        targets = GetTargetByMove(CurrentMove);

        foreach (GameObject target in targets)
        {
            Debug.Log(transform.name + " is attacking " + target.name);

        }

        animator.SetTrigger("Attack");
    }

    public override void CheckDead()
    {
        if (Hp <= 0)
        {
            Debug.Log(transform.name + "is dead");
            InventoryController.current.AddCoin(CoinOnKill, false);
            Dead();
        }
    }
}
