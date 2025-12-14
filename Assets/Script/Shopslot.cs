using TMPro;
using UnityEngine;

public class Shopslot : MonoBehaviour
{
    [SerializeField] GameObject itemslot;
    [SerializeField] TMP_Text price;
    [SerializeField] TMP_Text shortDiscription;

    public void UpdateText()
    {
        if(itemslot.transform.childCount > 0)
        {
            price.text = itemslot.transform.GetChild(0).GetComponent<Item>().price.ToString();
            shortDiscription.text = itemslot.transform.GetChild(0).GetComponent<Item>().shortDiscription;
        }
        
    }
}
