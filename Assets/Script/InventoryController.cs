using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InventoryController : MonoBehaviour
{
    public static InventoryController current;

    [Header("Currency")]
    public int coin = 0;
    public event Action<int> OnCoinsChanged;

    [Header("Hero Equipment Slot")]
    [SerializeField] private GameObject plusHeroEquipmentSlot;
    [SerializeField] private GameObject minusHeroEquipmentSlot;
    [SerializeField] private GameObject multiplyHeroEquipmentSlot;
    [SerializeField] private GameObject divideHeroEquipmentSlot;
    [SerializeField] private GameObject buffHeroEquipmentSlot;

    [Header("Equipment Inventory")]
    [SerializeField] private List<GameObject> EquipmentInventorySlots;

    [Header("Consumable Inventory")]
    [SerializeField] private List<GameObject> ConsumableInventorySlots;

    private void Awake()
    {
        current = this;
    }

    public void AddCoin(int amount)
    {
        if(amount <= 0)
        {
            Debug.LogWarning("amount is negative, use SpendCoin() instead");
        }

        coin += amount;
        Debug.Log("Money Added: " + amount + " G. Current coin:" + coin + "G");
        OnCoinsChanged?.Invoke(coin);
    }

    public bool SpendCoin(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("amount is negative, use AddCoin() instead");
        }

        if (amount > coin)
        {
            Debug.Log("BB no money");
            return false;
        }

        coin -= amount;
        Debug.Log("Money spended: " + amount + " G. Coin left:" + coin + "G");
        OnCoinsChanged?.Invoke(coin);
        return true;
    }


    public bool AddItem(GameObject item)
    {
        
        if(item.GetComponent<Item>() == null)
        {
            Debug.LogWarning(item + " is not Item");
            return false;
        }

        if (item.GetComponent<Item>() is Equipment equipment)
        {
            return AddItemToInventory(EquipmentInventorySlots, item);
        }
        else if (item.GetComponent<Item>() is Consumable consumable)
        {
            return AddItemToInventory(ConsumableInventorySlots, item);
        }

        Debug.Log(item + "is item but its type is not imprement yet");
        return false;
    }

    private bool AddItemToInventory(List<GameObject> slotList, GameObject item)
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount <= 0)
            {
                item.transform.SetParent(slot.transform,false);
                return true;
            }
        }

        Debug.Log("Inventory full");
        return false;
    }
}
