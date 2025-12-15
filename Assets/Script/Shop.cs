using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Utils;

public class Shop : MonoBehaviour
{
    public static Shop current;

    [SerializeField] private List<GameObject> shopSlots;

    [SerializeField] private TMP_Text DiscriptionPreview;
    [SerializeField] private Item shopSelectedItem;
    [SerializeField] private Image shopSelectedItemImagePreview;
    [SerializeField] private Sprite defaultShopImgPreview;

    [SerializeField] private Button buyBtn;
    [SerializeField] private TMP_Text buyPriceText;

    [SerializeField] private Button rerollBtn;
    [SerializeField] private TMP_Text rerollPriceText;
    [SerializeField] private int rerollPrice;
    [SerializeField] private int currentRerollPrice;
    [SerializeField] private int rerollInflation;

    [SerializeField] private List<GameObject> SpawnableItem;

    private void Awake()
    {
        current = this;
    }

    private void OnEnable()
    {
        InventoryController.current.OnCoinsChanged += UpdateBtn;
        ResetShop();
    }

    private void OnDisable()
    {
        InventoryController.current.OnCoinsChanged -= UpdateBtn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ResetShop()
    {
        SelectItem(null);
        buyBtn.interactable = false;
        currentRerollPrice = rerollPrice;
        UpdateBtn(InventoryController.current.coin);
        SpawnItem(0);
    }

    public void Reroll()
    {
        if (SpawnItem(currentRerollPrice))
        {
            Debug.Log("rerolled shop item");
            
            currentRerollPrice += rerollInflation;

        }
    }

    public void BuyItem()
    {
        if(shopSelectedItem == null)
        {
            Debug.Log("No item selected");
            return;
        }

        if (!InventoryController.current.SpendCoin(shopSelectedItem.price))
        {
            Debug.Log("can't buy");

        }


            if (InventoryController.current.AddItem(shopSelectedItem.gameObject))
            {
                Debug.Log("buy sucessgully");
                SelectItem(null);
            }
    }

    public void SelectItem(GameObject item)
    {
        if(item == null)
        {
            shopSelectedItem = null;
            shopSelectedItemImagePreview.sprite = defaultShopImgPreview;
            DiscriptionPreview.text = "";
        } else
        {
            shopSelectedItem = item.GetComponent<Item>();
            shopSelectedItemImagePreview.sprite = item.GetComponent<Image>().sprite;
            DiscriptionPreview.text = shopSelectedItem.discription;

        }

        UpdateBtn(InventoryController.current.coin);
    }

    bool SpawnItem(int CoinSpend)
    {
        if(!InventoryController.current.SpendCoin(CoinSpend))
        {
            return false;
        }

        SelectItem(null);

        foreach (GameObject shopslot in shopSlots)
        {
            if(shopslot.transform.Find("itemSlot").childCount > 0)
            {
                Destroy(shopslot.transform.Find("itemSlot").GetChild(0).gameObject);
            }

            int r = rnd.Next(SpawnableItem.Count);
            GameObject newItem = Instantiate(SpawnableItem[r], shopslot.transform.Find("itemSlot"));
        }

        return true;
    }

    void UpdateBtn(int coin)
    {
        if(shopSelectedItem != null)
        {
            int itemprice = shopSelectedItem.GetComponent<Item>().price;
            buyPriceText.text = "buy (" + itemprice + "G)";
            buyBtn.interactable = itemprice <= coin;
        }

        rerollPriceText.text = "Reroll (" + currentRerollPrice + "G)";
        rerollBtn.interactable = currentRerollPrice <= coin;
    }

    
}
