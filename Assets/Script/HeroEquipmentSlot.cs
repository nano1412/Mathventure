using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class HeroEquipmentSlot : MonoBehaviour
{
    [field: SerializeField]
    public CharacterType HeroTypeOwner { get; private set; }
    [field: SerializeField]
    public GameObject HeroOwner { get; private set; }
    [field: SerializeField]
    public Image HeroPreview { get; private set; }
    [field: SerializeField]
    public Sprite HeroSprite { get; private set; }
    [field: SerializeField]
    public GameObject WeaponSlot { get; private set; }
    [field: SerializeField]
    public GameObject ArmorSlot { get; private set; }
    [field: SerializeField]
    public Button EquipBtn { get; private set; }
    [field: SerializeField]
    public TMP_Text EquipBtnText { get; private set; }

    private void Awake()
    {
        EquipmentInventory.current.OnEquipmentSelectedItemChange += CheckIsSelectedEquipmentWearable;

    }

    void CheckIsSelectedEquipmentWearable(GameObject equipment)
    {
        if (equipment == null)
        {
            EquipBtn.interactable = false;
            return;
        }

        if (equipment.transform.IsChildOf(this.transform))
        {
            SetButton(UnequipItem, "Unequip");
            EquipBtn.interactable = true;
            return;
        }
        else
        {
            SetButton(EquipItem, "Equip");
        }

        Item item = equipment.GetComponent<Item>();
        if (!item.UsableCharacter.Contains(HeroTypeOwner))
        {
            EquipBtn.interactable = false;
            return;
        }

        switch (item.ItemType)
        {
            case ItemType.Weapon:
                EquipBtn.interactable = WeaponSlot.transform.childCount <= 0;
                break;

            case ItemType.Armor:
                EquipBtn.interactable = ArmorSlot.transform.childCount <= 0;
                break;

            default:
                EquipBtn.interactable = false;
                return;

        }

    }

    public void EquipItem()
    {
        if (EquipmentInventory.current.EquipmentSelectedItem == null)
        {
            return;
        }
        GameObject equipment = EquipmentInventory.current.EquipmentSelectedItem;

        Item item = equipment.GetComponent<Item>();

        GameObject equipmentSlot;

        switch (item.ItemType)
        {
            case ItemType.Weapon:
                equipmentSlot = WeaponSlot;
                break;

            case ItemType.Armor:
                equipmentSlot = ArmorSlot;
                break;

            default:
                return;

        }

        if (equipmentSlot.transform.childCount >= 1)
        {
            Debug.Log(HeroTypeOwner + " already have equipment on " + equipmentSlot.name);
            return;
        }

        equipment.transform.SetParent(equipmentSlot.transform, false);
        EquipmentInventory.current.EquipmentSelectedItem = null;
    }

    public void UnequipItem()
    {
        if (EquipmentInventory.current.EquipmentSelectedItem == null)
        {
            return;
        }


        InventoryController.current.AddItem(EquipmentInventory.current.EquipmentSelectedItem);
        EquipmentInventory.current.EquipmentSelectedItem = null;
        SetButton(EquipItem, "Equip");
    }

    void SetButton(Action action, string text)
    {
        EquipBtn.onClick.RemoveAllListeners();
        EquipBtn.onClick.AddListener(() => action?.Invoke());

        EquipBtnText.text = text;
    }
}
