using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Utils;

public abstract class Item : MonoBehaviour
{
    [Header ("Apparent")]
    public string itemName;

    [Header ("ShopData")]
    public int price;
    public int sellPrice;
    public string shortDiscription;
    public string discription;

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
