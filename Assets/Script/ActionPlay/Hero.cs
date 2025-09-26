using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    protected override Move defaultAttack => throw new System.NotImplementedException();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Attack(List<Transform> targets, Move move)
    {

    }
}
