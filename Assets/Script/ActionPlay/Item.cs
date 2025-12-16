using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;
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
    public List<ModifierSO> Modifiers { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
