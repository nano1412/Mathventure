using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static Utils;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public ItemType AcceptableItemType { get; private set; }
    [field: SerializeField] public SlotType TypeOfSlot { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (!(TypeOfSlot == SlotType.Shop || TypeOfSlot == SlotType.Display) && transform.GetComponentInChildren<ItemData>() != null)
        {
            EquipmentInventory.current.SetInventorySelectItem(transform.GetComponentInChildren<ItemData>().gameObject);

            if(transform.GetComponentInChildren<ConsumableData>() != null)
            {
                BuffController.current.SelectedConsumable = transform.GetComponentInChildren<ConsumableData>().gameObject;
            }
        }
    }
}
