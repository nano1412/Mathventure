using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(double mul, double atkValue)
    {

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
