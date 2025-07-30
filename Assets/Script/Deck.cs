using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Deck current;
    public List<Card> cards;
    
    private void Awake()
    {
        current = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
