using UnityEngine;

public class CardSlot : MonoBehaviour
{

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void DestroyCardInSlot()
    {
        if (transform.GetComponentInChildren<CardData>().gameObject != null)
        {
            Destroy(transform.GetComponentInChildren<CardData>().gameObject);
        }
    }
}
