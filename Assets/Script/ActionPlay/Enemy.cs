using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public void Attack()
    {
        targets = GetTargetByMove();

        foreach(GameObject target in targets)
        {
            Debug.Log(transform.name + " is attacking " + target.name);

        }

        animator.SetTrigger("Attack");
    }
}
