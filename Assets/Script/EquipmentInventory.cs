using System;
using TMPro;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public static EquipmentInventory current;
    public event Action<GameObject> OnEquipmentSelectedItemChange;

    [field: SerializeField]
    private GameObject equipmentSelectedItem;

    public GameObject EquipmentSelectedItem
    {
        get => equipmentSelectedItem;
        set
        {
            if (equipmentSelectedItem == value) return;
            equipmentSelectedItem = value;
            UpdateSelectedItemDescription(equipmentSelectedItem);
            OnEquipmentSelectedItemChange?.Invoke(equipmentSelectedItem);
        }
    }


    [field: SerializeField] public TMP_Text equipmentSelectedItemNameText { get; private set; }
    [field: SerializeField] public TMP_Text equipmentSelectedItemNameShortdescriptionText { get; private set; }
    [field: SerializeField] public TMP_Text equipmentSelectedItemNamedescriptionText { get; private set; }

    void Awake()
    {
        current = this;
    }


    private void UpdateSelectedItemDescription(GameObject equipmentSelectedItem)
    {
        if (equipmentSelectedItem == null)
        {
            equipmentSelectedItemNameText.text = "itemName";
            equipmentSelectedItemNameShortdescriptionText.text = "";
            equipmentSelectedItemNamedescriptionText.text = "";
            return;
        }

        Item item = equipmentSelectedItem.GetComponent<Item>();

        equipmentSelectedItemNameText.text = item.ItemName;
        equipmentSelectedItemNameShortdescriptionText.text = item.ShortDescription;
        equipmentSelectedItemNamedescriptionText.text = item.Description;
    }

    public void SetInventorySelectItem(GameObject item)
    {
        if(item.GetComponent<Item>() == null)
        {
            Debug.Log(item + " dont have item data in it");
            return;
        }
        EquipmentSelectedItem = item;
    }
}
