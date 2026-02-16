using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static Utils;

public class EquipmentInventory : MonoBehaviour
{
    public static EquipmentInventory current;
    public event Action<GameObject> OnEquipmentSelectedItemChange;
    [field: SerializeField] public List<GameObject> SelectedTargets { get; private set; }
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

    private bool isActive;
    [field: SerializeField] public GameObject HeroEquipmentMenu { get; private set; }
    [field: SerializeField] public TMP_Text EquipmentSelectedItemNameText { get; private set; }
    [field: SerializeField] public TMP_Text EquipmentSelectedItemNameShortdescriptionText { get; private set; }
    [field: SerializeField] public TMP_Text EquipmentSelectedItemNamedescriptionText { get; private set; }

    void Awake()
    {
        current = this;
        GameController.current.OnGameStateChange += HandleGameStateChange;
    }

    private void Start()
    {
        isActive = gameObject.activeSelf;
    }

    private void HandleGameStateChange(GameState gameState)
    {
        isActive = true;
        ToggleEquipmentMenuPopup();
    }


    private void UpdateSelectedItemDescription(GameObject equipmentSelectedItem)
    {
        if (equipmentSelectedItem == null)
        {
            EquipmentSelectedItemNameText.text = "itemName";
            EquipmentSelectedItemNameShortdescriptionText.text = "";
            EquipmentSelectedItemNamedescriptionText.text = "";
            return;
        }

        ItemData item = equipmentSelectedItem.GetComponent<ItemData>();

        item.SelectSFX.Play();
        EquipmentSelectedItemNameText.text = item.ItemName;
        EquipmentSelectedItemNameShortdescriptionText.text = item.ShortDescription;
        EquipmentSelectedItemNamedescriptionText.text = item.Description;
    }

    public void SetInventorySelectItem(GameObject item)
    {
        if (item.GetComponent<ItemData>() == null)
        {
            Debug.Log(item + " dont have item data in it");
            return;
        }
        EquipmentSelectedItem = item;
    }

    public void ToggleEquipmentMenuPopup()
    {
        isActive = !isActive;

        if (GameController.current.GameState == Utils.GameState.Shop)
        {
            gameObject.SetActive(true);
            HeroEquipmentMenu.SetActive(false);
            return;
        }
        gameObject.SetActive(isActive);
        HeroEquipmentMenu.SetActive(isActive);
    }
}
