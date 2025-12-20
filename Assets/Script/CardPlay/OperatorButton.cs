using UnityEngine;
using UnityEngine.Windows;
using static Operatorcard;
using static UnityEngine.Rendering.HDROutputUtils;
using static Utils;

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

    public void AddPlus()
    {
        AddOperatorCard(OperationEnum.Plus);
    }

    public void AddMinus()
    {
        AddOperatorCard(OperationEnum.Minus);
    }

    public void AddMultiply()
    {
        AddOperatorCard(OperationEnum.Multiply);
    }

    public void AddDivide()
    {
        AddOperatorCard(OperationEnum.Divide);
    }

    private void AddOperatorCard(OperationEnum operation)
    {
        foreach (GameObject operatorCardSlot in CardPlayGameController.current.PlayCardSlotList)
        {
            if (operatorCardSlot.CompareTag("OperatorSlot") && operatorCardSlot.transform.childCount == 0)
            {
                GameObject newOperatorCard = Instantiate(operatorCardPrefab, transform.position, new Quaternion(), operatorCardSlot.transform);
                newOperatorCard.GetComponent<Operatorcard>().SetOperator(operation);
                newOperatorCard.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }
}
