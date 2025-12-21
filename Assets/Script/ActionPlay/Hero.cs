using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    public void Attack(double mul, double leftAtkValue, double RightAtkValue)
    {
        Debug.Log("hero attack is called");
        targets = GetTargetByMove();

        foreach (GameObject target in targets)
        {
            Debug.Log(transform.name + " is attacking " + target.name  +  " with Lcard:" + leftAtkValue + " Rcard:" + RightAtkValue + " multiplier:" + mul);
        }

        animator.SetTrigger("Attack");

    }

    public override void CheckDead()
    {
        if (Hp < 0)
        {
            Debug.Log(transform.name + "is dead");
            Dead();

            GameController.current.Lose();
        }
    }
}
