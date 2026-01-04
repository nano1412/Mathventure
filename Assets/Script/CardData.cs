using UnityEngine;
using static Utils;

public class CardData : MonoBehaviour
{
    [field: Header("CardInDeckData Properties"), SerializeField] public double FaceValue { get; private set; } //use to calculate equation
    private double effectiveValue;
    public double EffectiveValue
    {
        get
        {
            return effectiveValue;
        }
        set
        {
            effectiveValue = value;
        }
    }
    [field: SerializeField] public CardBuffCondition CardType { get; private set; }

    private void Start()
    {
        SetupCardType();
    }

    private void SetupCardType()
    {
        if (FaceValue % 2 == 0)
        {

        }
    }

    public void SetFaceValue(double value)
    {
        FaceValue = value;
    }

    public void SetEffectValue(double value)
    {
        EffectiveValue = value;
    }

    public void SetCardData(CardInDeckData cardData)
    {
        FaceValue = cardData.FaceValue;
        EffectiveValue = cardData.EffectValue;
    }

    public double GetEffectiveValueWithBuff()
    {
        double temp = effectiveValue;

        foreach (CardBuff cardBuff in BuffController.current.CurrentCardBuff)
        {
            bool isApplyBuffToThisCard = false;


        }

        return temp;
    }
}
