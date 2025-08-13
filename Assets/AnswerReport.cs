using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Utils;

public class AnswerReport : MonoBehaviour
{
    private TMP_Text tmpText;
    [SerializeField] string text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tmpText = transform.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text = GetText();
        tmpText.text = text;
    }

    private string GetText()
    {
        double answer = CardPlayGameController.current.GetAnswer();
        double multiplier = CardPlayGameController.current.GetMultiplier();
        List<OperatorOrder> operatorOrder = CardPlayGameController.current.GetOperatorOrdersAsEnum();

        if(operatorOrder.Count < 3) { return "Invalid"; }

        return "Player answer: " + answer + " with " + multiplier + "x multiplier. order of operator are "
            + operatorOrder[0].ToString() + ", " + operatorOrder[1].ToString() + ", " + operatorOrder[2].ToString();
    }
}
