using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Utils;

public abstract class Item : MonoBehaviour
{
    [Header ("Apparent")]
    [SerializeField] private string name;

    [Header ("ShopData")]
    [SerializeField] private int price;
    [SerializeField] private int sellPrice;
    [SerializeField] private string shortDiscription;
    [SerializeField] private string discription;

    [Header("ItemData")]
    [SerializeField] ItemType itemType;
    [SerializeField] private List<ModifierSO> modifiers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
