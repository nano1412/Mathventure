using UnityEngine;

public class CardSlot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
