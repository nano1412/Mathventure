using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static Utils;

public class Shop : MonoBehaviour
{
    public static Shop current;
    public MainGameUIController uIController;
    public GameObject infoPanel;
    [field: SerializeField] public AudioSource RerollSFX { get; private set; }

    [field: SerializeField]
    public List<GameObject> ShopSlots { get; private set; }


    [field: SerializeField]
    public TMP_Text DescriptionPreview { get; private set; }

    [field: SerializeField]
    public TMP_Text ItemNamePreview { get; private set; }

    [field: SerializeField]
    public ItemData ShopSelectedItemData { get; private set; }

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
        //LevelCreator.current.OnGameStart += UpdatdPossibleShopItem;
    }

    private void OnDisable()
    {
        //LevelCreator.current.OnGameStart -= UpdatdPossibleShopItem;
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
        if (gameState != GameState.Shop)
        {
            uIController.Close(gameObject);
            uIController.Close(infoPanel);
        }
        else
        {
            uIController.Open(gameObject);
            uIController.Open(infoPanel);
        }
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
            RerollSFX.Play();
            Debug.Log("rerolled shop item");

            CurrentRerollPrice += RerollInflation;

        }
    }

    public void BuyItem()
    {
        if (ShopSelectedItemData == null)
        {
            Debug.Log("No item selected");
            return;
        }

        if (!InventoryController.current.SpendCoin(ShopSelectedItemData.Price, true))
        {
            Debug.Log("can't buy");

        }


        if (InventoryController.current.AddItem(ShopSelectedItemData.gameObject))
        {
            Debug.Log("buy successfully");
            ShopSelectItem(null);
        }
    }

    public void ShopSelectItem(GameObject item)
    {
        if (item == null)
        {
            ShopSelectedItemData = null;
            ShopSelectedItemImagePreview.sprite = DefaultShopImgPreview;
            DescriptionPreview.text = "";
            ItemNamePreview.text = "";
        }
        else
        {
            item.GetComponent<ItemData>().SelectSFX.Play();
            ShopSelectedItemData = item.GetComponent<ItemData>();
            ShopSelectedItemImagePreview.sprite = item.GetComponent<Image>().sprite;
            DescriptionPreview.text = item.GetComponent<ItemData>().Description;
            ItemNamePreview.text = item.GetComponent<ItemData>().ItemName;

        }

        UpdateBtn(InventoryController.current.Coin);
    }

    bool SpawnItem(int CoinSpend)
    {
        SpawnableItems = LevelCreator.current.SpawnableItems;

        if (!InventoryController.current.SpendCoin(CoinSpend, false))
        {
            Debug.Log("spawn item not sucessful");
            return false;
        }

        ShopSelectItem(null);

        foreach (GameObject shopslot in ShopSlots)
        {
            if (shopslot.transform.Find("itemSlot").GetComponentInChildren<ItemData>() != null)
            {
                Destroy(shopslot.transform.Find("itemSlot").GetComponentInChildren<ItemData>().gameObject);
            }

            int r = rnd.Next(SpawnableItems.Count);
            GameObject newItem = Instantiate(SpawnableItems[r], shopslot.transform.Find("itemSlot"));
        }

        return true;
    }

    void UpdateBtn(int coin)
    {
        if (ShopSelectedItemData != null)
        {
            int itemprice = ShopSelectedItemData.GetComponent<ItemData>().Price;
            BuyPriceText.text = itemprice + "G";
            BuyBtn.interactable = itemprice <= coin;
        }

        RerollPriceText.text = "(" + CurrentRerollPrice + "G)";
        RerollBtn.interactable = CurrentRerollPrice <= coin;
    }


}
