using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
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
        Debug.Log(transform.name + " is attacking");
    }
}
