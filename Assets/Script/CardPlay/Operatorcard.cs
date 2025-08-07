using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HDROutputUtils;
using static Utils;

public class Operatorcard : MonoBehaviour, IPointerUpHandler
{

    public OperationEnum operation;
    private TMP_Text operationText;
    public OperationPriority priority;
    /*
     * priority are follow PEMDAS principal
     * Parentheses = 2
     * Exponents (not use in this game)
     * Multiplication and Division = 1
     * Addition and Subtraction = 0
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
                priority = OperationPriority.AdditionAndSubtraction;
                return "+";


            case OperationEnum.Minus:
                priority = OperationPriority.AdditionAndSubtraction;
                return "-";


            case OperationEnum.Multiply:
                priority = OperationPriority.MultiplicationAndDivision;
                return "X";


            case OperationEnum.Divide:
                priority = OperationPriority.MultiplicationAndDivision;
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
