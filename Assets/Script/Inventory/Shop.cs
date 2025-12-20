using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Utils;

public class Shop : MonoBehaviour
{
    public static Shop current;

    [field: SerializeField]
    public List<GameObject> ShopSlots { get; private set; }


    [field: SerializeField]
    public TMP_Text DescriptionPreview { get; private set; }

    [field: SerializeField]
    public Item ShopSelectedItem { get; private set; }

    [field: SerializeField]
    public Image ShopSelectedItemImagePreview { get; private set; }

    [field: SerializeField]
    public Sprite DefaultShopImgPreview { get; private set; }


    [field: SerializeField]
    public Button BuyBtn { get; private set; }

    [field: SerializeField]
    public TMP_Text BuyPriceText { get; private set; }


    [field: SerializeField]
    public Button RerollBtn { get; private set; }

    [field: SerializeField]
    public TMP_Text RerollPriceText { get; private set; }

    [field: SerializeField]
    public int RerollPrice { get; private set; }

    [field: SerializeField]
    public int CurrentRerollPrice { get; private set; }

    [field: SerializeField]
    public int RerollInflation { get; private set; }


    [field: SerializeField]
    public List<GameObject> SpawnableItems { get; private set; }

    private void Awake()
    {
        current = this;
        InventoryController.current.OnCoinsChanged += UpdateBtn;
        GameController.current.OnGameStateChange += HandleGameStateChange;
    }

    private void OnEnable()
    {
        
        ResetShop();
        LevelCreator.current.OnGameStart += UpdatdPossibleShopItem;
    }

    private void OnDisable()
    {
        LevelCreator.current.OnGameStart -= UpdatdPossibleShopItem;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
    }

    private void UpdatdPossibleShopItem(int i)
    {
        SpawnableItems = LevelCreator.current.SpawnableItems;
    }

    private void HandleGameStateChange(GameState gameState)
    {
        if(gameState != GameState.Shop)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void ResetShop()
    {
        ShopSelectItem(null);
        BuyBtn.interactable = false;
        CurrentRerollPrice = RerollPrice;
        UpdateBtn(InventoryController.current.Coin);
        SpawnItem(0);
    }

    public void Reroll()
    {
        if (SpawnItem(CurrentRerollPrice))
        {
            Debug.Log("rerolled shop item");
            
            CurrentRerollPrice += RerollInflation;

        }
    }

    public void BuyItem()
    {
        if(ShopSelectedItem == null)
        {
            Debug.Log("No item selected");
            return;
        }

        if (!InventoryController.current.SpendCoin(ShopSelectedItem.Price))
        {
            Debug.Log("can't buy");

        }


            if (InventoryController.current.AddItem(ShopSelectedItem.gameObject))
            {
                Debug.Log("buy successfully");
                ShopSelectItem(null);
            }
    }

    public void ShopSelectItem(GameObject item)
    {
        if(item == null)
        {
            ShopSelectedItem = null;
            ShopSelectedItemImagePreview.sprite = DefaultShopImgPreview;
            DescriptionPreview.text = "";
        } else
        {
            ShopSelectedItem = item.GetComponent<Item>();
            ShopSelectedItemImagePreview.sprite = item.GetComponent<Image>().sprite;
            DescriptionPreview.text = ShopSelectedItem.Description;

        }

        UpdateBtn(InventoryController.current.Coin);
    }

    bool SpawnItem(int CoinSpend)
    {
        if(!InventoryController.current.SpendCoin(CoinSpend))
        {
            Debug.Log("spawn item not sucessful");
            return false;
        }

        ShopSelectItem(null);

        foreach (GameObject shopslot in ShopSlots)
        {
            if(shopslot.transform.Find("itemSlot").childCount > 0)
            {
                Destroy(shopslot.transform.Find("itemSlot").GetChild(0).gameObject);
            }

            int r = rnd.Next(SpawnableItems.Count);
            GameObject newItem = Instantiate(SpawnableItems[r], shopslot.transform.Find("itemSlot"));
        }

        return true;
    }

    void UpdateBtn(int coin)
    {
        if(ShopSelectedItem != null)
        {
            int itemprice = ShopSelectedItem.GetComponent<Item>().Price;
            BuyPriceText.text = "buy (" + itemprice + "G)";
            BuyBtn.interactable = itemprice <= coin;
        }

        RerollPriceText.text = "Reroll (" + CurrentRerollPrice + "G)";
        RerollBtn.interactable = CurrentRerollPrice <= coin;
    }

    
}
