using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Shopslot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject itemslot;
    [SerializeField] TMP_Text price;
    [SerializeField] TMP_Text shortDiscription;
    [SerializeField] TMP_Text displayItemname;

    public void UpdateText()
    {
        if (itemslot.transform.childCount > 0)
        {
            price.text = itemslot.transform.GetChild(0).GetComponent<oldItem>().Price.ToString();
            shortDiscription.text = itemslot.transform.GetChild(0).GetComponent<oldItem>().ShortDescription;
            displayItemname.text = itemslot.transform.GetChild(0).GetComponent<oldItem>().ItemName;
        }

    }

    public GameObject GetItem()
    {
        if (itemslot.transform.childCount > 0)
        {
            return itemslot.transform.GetChild(0).gameObject;
        }

        Debug.Log("no item on this shop slot");
        return null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.displayItemname + " clicked");
        GameObject item = GetItem();
        Debug.Log(item.GetComponent<oldItem>().ItemName);
        Shop.current.ShopSelectItem(item);
    }
}
