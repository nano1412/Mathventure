using UnityEngine;

public class OperatorButton : MonoBehaviour
{
    [SerializeField] private GameObject operatorCardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddOperatorCard()
    {


        foreach(Transform operatorCardSlot in GameController.current.playedOperatorSlots.transform)
        {
            Debug.Log(operatorCardSlot.name);
            if (operatorCardSlot.CompareTag("Slot") && operatorCardSlot.childCount == 0)
            {
                GameObject newOperatorCard = Instantiate(operatorCardPrefab, transform.position, new Quaternion(), operatorCardSlot);
                newOperatorCard.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }
}
