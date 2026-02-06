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
        
        if (!(TypeOfSlot == SlotType.Shop || TypeOfSlot == SlotType.Display) && transform.childCount >= 0)
        {
            EquipmentInventory.current.SetInventorySelectItem(transform.GetChild(0).gameObject);

            if(transform.GetChild(0).gameObject.GetComponent<ConsumableData>()!= null)
            {
                BuffController.current.SelectedConsumable = transform.GetChild(0).gameObject;
            }
        }
    }
}
