using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Shopslot : MonoBehaviour
{
    [SerializeField] GameObject itemslot;
    [SerializeField] TMP_Text price;
    [SerializeField] TMP_Text shortDiscription;
    [SerializeField] TMP_Text displayItemname;

    public void UpdateText()
    {
        if (itemslot.transform.GetComponentInChildren<ItemData>() != null)
        {
            ItemData itemData = itemslot.transform.GetComponentInChildren<ItemData>();
            price.text = itemData.Price.ToString();
            shortDiscription.text = itemData.ShortDescription;
            displayItemname.text = itemData.ItemName;
        }

    }

    public ItemData GetItem()
    {
        if (itemslot.transform.GetComponentInChildren<ItemData>() != null)
        {
            return itemslot.transform.GetComponentInChildren<ItemData>();
        }

        Debug.Log("no item on this shop slot");
        return null;
    }
}
