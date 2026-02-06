using UnityEngine;

public class ItemData : MonoBehaviour
{
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int SellPrice { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string ShortDescription { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
