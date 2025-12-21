using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.HDROutputUtils;
using static Utils;

public class Operatorcard : MonoBehaviour, IPointerUpHandler
{

    public OperationEnum operation;
    [SerializeField] private Image image;

    [SerializeField] private Sprite plusSprite;
    [SerializeField] private Sprite minusSprite;
    [SerializeField] private Sprite multiplySprite;
    [SerializeField] private Sprite divideSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        switch (operation)
        {
            case OperationEnum.Plus:
                image.sprite = plusSprite;
                break;


            case OperationEnum.Minus:
                image.sprite = minusSprite;
                break;


            case OperationEnum.Multiply:
                image.sprite = multiplySprite;
                break;


            case OperationEnum.Divide:
                image.sprite = divideSprite;
                break;

        }

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
