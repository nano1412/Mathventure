using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryController : MonoBehaviour
{
    public static InventoryController current;

    [field: Header("Currency"), SerializeField]
    public int Coin { get; private set; } = 0;

    public event Action<int> OnCoinsChanged;

    [field: Header("Equipment Inventory"), SerializeField]
    public List<GameObject> EquipmentInventorySlots { get; private set; }

    [field: Header("Consumable Inventory"), SerializeField]
    public List<GameObject> ConsumableInventorySlots { get; private set; }

    private void Awake()
    {
        current = this;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
    }

    public void AddCoin(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("amount is negative, use SpendCoin() instead");
        }

        Coin += amount;
        Debug.Log("Money Added: " + amount + " G. Current Coin:" + Coin + "G");
        OnCoinsChanged?.Invoke(Coin);
    }

    public bool SpendCoin(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("amount is negative, use AddCoin() instead");
        }

        if (amount > Coin)
        {
            Debug.Log("BB no money");
            return false;
        }

        Coin -= amount;
        Debug.Log("Money spended: " + amount + " G. Coin left:" + Coin + "G");
        OnCoinsChanged?.Invoke(Coin);
        return true;
    }


    public bool AddItem(GameObject item)
    {

        if (item.GetComponent<ItemData>() == null)
        {
            Debug.LogWarning(item + " is not Item");
            return false;
        }

        if (item.GetComponent<EquipmentData>() != null)
        {
            return AddItemToInventory(EquipmentInventorySlots, item);
        }
        else if (item.GetComponent<ConsumableData>() != null)
        {
            return AddItemToInventory(ConsumableInventorySlots, item);
        }

        // add card in shop item here

        Debug.Log(item + "is item but its type is not imprement yet");
        return false;
    }

    private bool AddItemToInventory(List<GameObject> slotList, GameObject item)
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                item.transform.SetParent(slot.transform, false);
                return true;
            }
        }

        Debug.Log("Inventory full");
        return false;
    }
}
