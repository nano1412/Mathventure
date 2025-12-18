using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public abstract class Item : MonoBehaviour
{
    [field: Header("Apparent"), SerializeField]
    public string ItemName { get; private set; }

    [field: Header("Shop Data"), SerializeField]
    public int Price { get; private set; }

    [field: SerializeField]
    public int SellPrice { get; private set; }

    [field: SerializeField]
    public string ShortDescription { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: Header("Item Data"), SerializeField]
    public ItemType ItemType { get; private set; }

    [field: SerializeField]
    public SlotType ItemRelation { get; private set; } = SlotType.Shop;

    [field: SerializeField]
    public List<CharacterType> UsableCharacter { get; private set; }

    [field: SerializeField]
    public List<ModifierSO> Modifiers { get; private set; }

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnTransformParentChanged()
    {
        if (transform.parent.GetComponent<ItemSlot>())
        {
            ItemRelation = transform.parent.GetComponent<ItemSlot>().TypeOfSlot;
        }

        if(ItemRelation == SlotType.Shop)
        {
            image.raycastTarget = false;
        }
        else
        {
            image.raycastTarget = true;
        }
        
    }

    public abstract bool UseItem();
}
