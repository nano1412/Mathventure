using UnityEngine;
using UnityEngine.EventSystems;
using static Utils;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public ItemType AcceptableItemType { get; private set; }
    [field: SerializeField] public SlotType TypeOfSlot { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if((TypeOfSlot == SlotType.Equipment || TypeOfSlot == SlotType.Consumable) && transform.childCount >= 0)
        {
            EquipmentInventory.current.SetInventorySelectItem(transform.GetChild(0).gameObject);
        }
    }

    private void OnTransformChildrenChanged()
    {
        if(transform.childCount > 1)
        {
            Debug.LogWarning("there are more than 1 items in " + transform.name + " slot");
        }

        if (transform.childCount == 1)
        {
            if(transform.GetChild(0).GetComponent<Item>() == null)
            {
                Debug.LogWarning("item on " + transform.name + " dont have item data");
            }
        }
    }
}
