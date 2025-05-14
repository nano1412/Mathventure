using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Operatorcard : MonoBehaviour, IPointerUpHandler
{
    public enum OperationEnum
    {
        Plus,
        Minus,
        Multiply,
        Divide
    }
    public OperationEnum operation;
    private TMP_Text operationText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        operationText = transform.Find("operationText").GetComponent<TMP_Text>();

        operationText.text = ChangeOperationTextToSymbol(operation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string ChangeOperationTextToSymbol(OperationEnum operation)
    {
        switch (operation)
        {
            case OperationEnum.Plus:
                return "+";


            case OperationEnum.Minus:
                return "-";


            case OperationEnum.Multiply:
                return "X";


            case OperationEnum.Divide:
                return "/";

        }

        return "Null Operation";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (eventData.button != PointerEventData.InputButton.Left)
        //    return;

        Destroy(gameObject);
    }
}
