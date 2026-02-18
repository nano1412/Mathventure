using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cardMultiplierIndicater : MonoBehaviour
{
    [field: SerializeField] private Slider Slider;
    [field: SerializeField] private double ActualValue;
    [field: SerializeField] private float numberIncreasePerSecond = 20f;
    [field: SerializeField] private float Speed = 3f;

    [field: SerializeField] private double sliderValue;
    [field: SerializeField] private double multiplierValueValue;
    [field: SerializeField] private double minValue;
    [field: SerializeField] private double maxValue;
    [field: SerializeField] private double targetValue;

    [field: SerializeField] private TMP_Text sliderValueText;
    [field: SerializeField] private TMP_Text multiplierText;
    [field: SerializeField] private TMP_Text minText;
    [field: SerializeField] private TMP_Text maxText;
    [field: SerializeField] private TMP_Text targetText;

    [field: SerializeField] private Color BlueColor;
    [field: SerializeField] private Color GreenColor;
    [field: SerializeField] private Color YellowColor;
    [field: SerializeField] private Color RedColor;

    // Update is called once per frame
    void Update()
    {
        UpdateValue();
        UpdateMultiplierText();
        UpdateText();
        UpdateSlider();
    }

    void UpdateValue()
    {
        ActualValue = CardPlayGameController.current.PreviewPlayerAnswer;
        sliderValue = Mathf.MoveTowards(
    (float)sliderValue,
    (float)ActualValue,
    numberIncreasePerSecond * Time.deltaTime * Speed
);

        minValue = CardPlayGameController.current.TargetNumber - CardPlayGameController.current.YellowZoneValue;
        maxValue = CardPlayGameController.current.TargetNumber + CardPlayGameController.current.YellowZoneValue;
        targetValue = CardPlayGameController.current.TargetNumber;
    }

    void UpdateMultiplierText()
    {
        if(ActualValue == targetValue)
        {
            multiplierText.text = "x" + CardPlayGameController.current.BlueZoneMultiplier;
            multiplierText.color = BlueColor;
            return;
        }

        if (ActualValue >= CardPlayGameController.current.TargetNumber - CardPlayGameController.current.GreenZoneValue &&
            ActualValue <= CardPlayGameController.current.TargetNumber + CardPlayGameController.current.GreenZoneValue
            )
        {
            multiplierText.text = "x" + CardPlayGameController.current.GreenZoneMultiplier;
            multiplierText.color = GreenColor;
            return;
        }

        if (ActualValue >= CardPlayGameController.current.TargetNumber - CardPlayGameController.current.YellowZoneValue &&
            ActualValue <= CardPlayGameController.current.TargetNumber + CardPlayGameController.current.YellowZoneValue
            )
        {
            multiplierText.text = "x" + CardPlayGameController.current.YellowZoneMultiplier;
            multiplierText.color = YellowColor;
            return;
        }

        multiplierText.text = "x" + CardPlayGameController.current.RedZoneMultiplier;
        multiplierText.color = RedColor;
        return;
    }

    void UpdateText()
    {
        sliderValueText.text = sliderValue.ToString("F2");
        minText.text = minValue.ToString();
        maxText.text = maxValue.ToString();
        targetText.text = targetValue.ToString();
    }

    void UpdateSlider()
    {
        Slider.minValue = (float)minValue;
        Slider.maxValue = (float)maxValue;
        Slider.value = (float)sliderValue;
    }
}
