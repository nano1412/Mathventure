using UnityEngine;
using UnityEngine.EventSystems;
using static Utils;

public class ItemData : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public AudioSource SelectSFX { get; private set; }
    [field: SerializeField] public AudioSource UseSFX { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int SellPrice { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string ShortDescription { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("item clicked");

        ItemSlot itemSlot = transform.parent.GetComponent<ItemSlot>();
        if(itemSlot == null)
        {
            return;
        }

        if (itemSlot.TypeOfSlot == SlotType.Shop)
        {
            Shop.current.ShopSelectItem(gameObject);
        }

        if (!(itemSlot.TypeOfSlot == SlotType.Shop || itemSlot.TypeOfSlot == SlotType.Display))
        {
            EquipmentInventory.current.SetInventorySelectItem(gameObject);

            if (transform.GetComponentInChildren<ConsumableData>() != null)
            {
                BuffController.current.SelectedConsumable = gameObject;
            }
        }
    }
}