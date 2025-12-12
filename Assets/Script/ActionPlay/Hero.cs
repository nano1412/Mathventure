using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] private Move defaultMove;

    public override Move DefaultMove => defaultMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {

    }

    public override void CheckDead()
    {
        if (hp < 0)
        {
            Debug.Log(transform.name + "is dead");
            Dead();

            GameController.current.Lose();
        }
    }
}
