using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [field:SerializeField]public int CoinOnKill { get; private set; }
    public void Attack()
    {
        targets = GetTargetByMove();

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
            InventoryController.current.AddCoin(CoinOnKill);
            Dead();
        }
    }
}
