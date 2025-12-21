using UnityEngine;

public class HeroEquipmentMenu : MonoBehaviour
{
    [field: Header("Hero Equipment Slot"), SerializeField]
    public GameObject PlusHeroEquipmentSlot { get; private set; }

    [field: SerializeField]
    public GameObject MinusHeroEquipmentSlot { get; private set; }

    [field: SerializeField]
    public GameObject MultiplyHeroEquipmentSlot { get; private set; }

    [field: SerializeField]
    public GameObject DivideHeroEquipmentSlot { get; private set; }

    [field: SerializeField]
    public GameObject BuffHeroEquipmentSlot { get; private set; }

    private void OnEnable()
    {
        EquipmentInventory.current.OnEquipmentSelectedItemChange += OnSelectedEquipmentChange;
    }

    private void OnDisable()
    {
        EquipmentInventory.current.OnEquipmentSelectedItemChange -= OnSelectedEquipmentChange;
    }

    private void OnSelectedEquipmentChange(GameObject selectedItem)
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
