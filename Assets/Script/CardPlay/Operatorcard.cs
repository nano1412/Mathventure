using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HDROutputUtils;
using static Utils;

public class Operatorcard : MonoBehaviour, IPointerUpHandler
{

    private OperationEnum operation;
    private TMP_Text operationText;
    /*
     * priority are follow PEMDAS principal
     * Parentheses = 3
     * Exponents (not use in this game)
     * Multiplication and Division = 2
     * Addition and Subtraction = 1
     * not Operator = 0
     * if the priority are equal then process from left to right
     */


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

    public OperationEnum GetOperator()
    {
        return operation;
    }

    public void SetOperator(OperationEnum value)
    {
        operation = value;
    }
}
