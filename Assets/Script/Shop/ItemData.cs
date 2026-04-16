using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    private void Start()
    {
        Shopslot shopslot = transform.parent.parent.GetComponent<Shopslot>();
        Debug.Log(shopslot);
        if (shopslot && shopslot.transform.Find("itemSlot").GetComponent<ItemSlot>().TypeOfSlot == SlotType.Shop)
        {
            shopslot.GetComponent<Button>().onClick.AddListener(SelectShopItem);
        }
    }

    private void SelectShopItem()
    {
        Shop.current.ShopSelectItem(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("item clicked");

        ItemSlot itemSlot = transform.parent.GetComponent<ItemSlot>();

        if (itemSlot == null)
        {
            return;
        }

        if (!(itemSlot.TypeOfSlot == SlotType.Shop || itemSlot.TypeOfSlot == SlotType.Display))
        {
            Debug.Log(gameObject);
            EquipmentInventory.current.SetInventorySelectItem(gameObject);

            if (transform.GetComponentInChildren<ConsumableData>() != null)
            {
                BuffController.current.SelectedConsumable = gameObject;
            }
        }

       
    }
}