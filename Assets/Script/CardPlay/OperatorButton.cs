using UnityEngine;
using UnityEngine.Windows;
using static Operatorcard;
using static UnityEngine.Rendering.HDROutputUtils;
using static Utils;

public class OperatorButton : MonoBehaviour
{
    [field: Header("SFX"), SerializeField] private AudioSource plusSFX;
    [field: SerializeField] private AudioSource minusSFX;
    [field: SerializeField] private AudioSource multiplySFX;
    [field: SerializeField] private AudioSource divideSFX;
    [SerializeField] private GameObject operatorCardPrefab;

    public void AddPlus()
    {
        plusSFX.Play();
        AddOperatorCard(OperationEnum.Plus);
    }

    public void AddMinus()
    {
        minusSFX.Play();
        AddOperatorCard(OperationEnum.Minus);
    }

    public void AddMultiply()
    {
        multiplySFX.Play();
        AddOperatorCard(OperationEnum.Multiply);
    }

    public void AddDivide()
    {
        divideSFX.Play();
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
