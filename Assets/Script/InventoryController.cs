using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Hero Equipment Slot")]
    [SerializeField] private GameObject plusHeroEquipmentSlot;
    [SerializeField] private GameObject minusHeroEquipmentSlot;
    [SerializeField] private GameObject multiplyHeroEquipmentSlot;
    [SerializeField] private GameObject divideHeroEquipmentSlot;
    [SerializeField] private GameObject buffHeroEquipmentSlot;

    [Header("Equipment Inventory")]
    [SerializeField] private GameObject EquipmentInventorySlot1;
    [SerializeField] private GameObject EquipmentInventorySlot2;
    [SerializeField] private GameObject EquipmentInventorySlot3;
    [SerializeField] private GameObject EquipmentInventorySlot4;

    [Header("Consumable Inventory")]
    [SerializeField] private GameObject ConsumableInventorySlot1;
    [SerializeField] private GameObject ConsumableInventorySlot2;
    [SerializeField] private GameObject ConsumableInventorySlot3;
    [SerializeField] private GameObject ConsumableInventorySlot4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
