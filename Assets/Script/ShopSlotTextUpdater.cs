using UnityEngine;

public class ShopSlotTextUpdater : MonoBehaviour
{
    void OnTransformChildrenChanged()
    {
        transform.parent.GetComponent<Shopslot>().UpdateText();
    }
}
