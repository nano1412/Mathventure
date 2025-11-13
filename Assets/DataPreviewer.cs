using UnityEngine;
using TMPro;
using static Utils;
using System.Collections.Generic;
public class DataPreviewer : MonoBehaviour
{
    [Header("Preview text")]
    [SerializeField] private TMP_Text PlayStateText;
    [SerializeField] private TMP_Text actualAnswerText;
    [SerializeField] private TMP_Text previewAnswerText;
    [SerializeField] private TMP_Text targetNumberText;
    [SerializeField] private TMP_Text previewDataText;
    [Space(5)]

    [SerializeField] private TMP_Text blueZoneMultiplierText;
    [Space(5)]

    [SerializeField] private TMP_Text minGreenZoneValueText;
    [SerializeField] private TMP_Text maxGreenZoneValueText;
    [SerializeField] private TMP_Text greenZoneMultiplierText;
    [Space(5)]

    [SerializeField] private TMP_Text minYellowZoneValueText;
    [SerializeField] private TMP_Text maxYellowZoneValueText;
    [SerializeField] private TMP_Text yellowZoneMultiplierText;
    [Space(5)]

    [SerializeField] private TMP_Text redZoneMultiplierText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blueZoneMultiplierText.text = "x" + CardPlayGameController.current.BlueZoneMultiplier;
        greenZoneMultiplierText.text = "x" + CardPlayGameController.current.GreenZoneMultiplier;
        yellowZoneMultiplierText.text = "x" + CardPlayGameController.current.YellowZoneMultiplier;
        redZoneMultiplierText.text = "x" + CardPlayGameController.current.RedZoneMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        previewAnswerText.text = "preview answer: " + CardPlayGameController.current.PreviewPlayerAnswer.ToString();
        actualAnswerText.text = "actual answer: " + CardPlayGameController.current.PlayerAnswer.ToString();
        targetNumberText.text = "target number: " + CardPlayGameController.current.TargetNumber.ToString();
        previewDataText.text = GetAttackOrderSummary();

        if (!double.IsNaN(CardPlayGameController.current.TargetNumber))
        {
            minGreenZoneValueText.text = (CardPlayGameController.current.TargetNumber - CardPlayGameController.current.GreenZoneValue).ToString();
            maxGreenZoneValueText.text = (CardPlayGameController.current.TargetNumber + CardPlayGameController.current.GreenZoneValue).ToString();

            minYellowZoneValueText.text = (CardPlayGameController.current.TargetNumber - CardPlayGameController.current.YellowZoneValue).ToString();
            maxYellowZoneValueText.text = (CardPlayGameController.current.TargetNumber + CardPlayGameController.current.YellowZoneValue).ToString();

        }

        if (CardPlayGameController.current.isHandReady < 0)
        {
            PlayStateText.text = "Invalid";
        }
        else if (CardPlayGameController.current.isHandReady > 0 && CardPlayGameController.current.isHandReady < 4)
        {
            PlayStateText.text = "Ready to preview";
        }
        else if (CardPlayGameController.current.isHandReady >= 4)
        {
            PlayStateText.text = "Valid";

        }
    }

    private string GetAttackOrderSummary()
    {
        double answer = CardPlayGameController.current.GetAnswer();
        double multiplier = CardPlayGameController.current.GetMultiplier();
        List<OperatorOrder> operatorOrder = CardPlayGameController.current.GetOperatorOrdersAsEnum();

        if (operatorOrder.Count < 3) { return "player haven't summit equation yet"; }

        return "Player answer: " + answer + " with " + multiplier + "x multiplier. order of operator are "
            + operatorOrder[0].ToString() + ", " + operatorOrder[1].ToString() + ", " + operatorOrder[2].ToString();
    }
}
