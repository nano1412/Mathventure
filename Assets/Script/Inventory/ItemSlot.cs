using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static Utils;

public class ItemSlot : MonoBehaviour
{
    [field: SerializeField] public ItemType AcceptableItemType { get; private set; }
    [field: SerializeField] public SlotType TypeOfSlot { get; private set; }

    [field: SerializeField] public GameObject MiniItemDetail { get; private set; }
    [field: SerializeField] public TMP_Text ItemName { get; private set; }
    [field: SerializeField] public TMP_Text ItemShortDescription { get; private set; }

    private void Start()
    {
        BuffController.current.OnSelectedConsumableUpdate += OnComsumableSelectionChange;
    }

    private void OnDestroy()
    {
        BuffController.current.OnSelectedConsumableUpdate -= OnComsumableSelectionChange;
    }


    void OnComsumableSelectionChange(GameObject itemGO)
    {
        Debug.Log("1");
        if(itemGO == null)
        {
            Debug.Log("2");
            MiniItemDetail.SetActive(false);
            Debug.Log("3");
            transform.GetComponentInChildren<ItemData>().gameObject.GetComponent<Outline>().enabled = false;
            Debug.Log("4");
            return;
        }
        
        if (transform.GetComponentInChildren<ItemData>() == itemGO.GetComponent<ItemData>())
        {
            
            MiniItemDetail.SetActive(true);
            transform.GetComponentInChildren<ItemData>().gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnTransformChildrenChanged()
    {
        if(transform.GetComponentInChildren<ItemData>() != null)
        {
            ItemName.text = transform.GetComponentInChildren<ItemData>().ItemName;
            ItemShortDescription.text = transform.GetComponentInChildren<ItemData>().ShortDescription;
        }
    }
}
