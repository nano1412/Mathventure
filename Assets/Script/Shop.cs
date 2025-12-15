using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Utils;

public class Shop : MonoBehaviour
{
    public Shop current;

    [SerializeField] private List<GameObject> shopSlots;

    [SerializeField] private TMP_Text DiscriptionPreview;
    [SerializeField] private Item selectedItem;
    [SerializeField] private int rerollPrice;

    [SerializeField] private List<GameObject> SpawnableItem;

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("shop start");
        //on enable
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reroll()
    {

    }

    void SpawnItem()
    {
        foreach(GameObject shopslot in shopSlots)
        {
            int r = rnd.Next(SpawnableItem.Count);
            GameObject newItem = Instantiate(SpawnableItem[r], shopslot.transform.Find("itemSlot"));
        }
    }
}
