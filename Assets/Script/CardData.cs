using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
    [field: SerializeField] public List<CardBuffCondition> CardType { get; private set; }

    private void Start()
    {
        SetupCardType();
    }

    private void SetupCardType()
    {
        if (FaceValue % 2 == 0)
        {
            CardType.Add(CardBuffCondition.Card_IsEven);
        } else
        {
            CardType.Add(CardBuffCondition.Card_IsOdd);
        }

        if (Utils.IsPrimeFromDouble(FaceValue))
        {
            CardType.Add(CardBuffCondition.Card_IsPrime);
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
            //check condition
            bool isApplyBuffToThisCard = cardBuff.CardBuffCondition.Contains(CardBuffCondition.Card_All);

            if (cardBuff.CardBuffCondition.Intersect(CardType).Any())
            {
                isApplyBuffToThisCard = true;
                break;
            }

            if (cardBuff.CardBuffCondition.Contains(CardBuffCondition.Card_Equal))
            {
                isApplyBuffToThisCard = FaceValue == cardBuff.ThresholdValue;
            } else if (cardBuff.CardBuffCondition.Contains(CardBuffCondition.Card_Lessthan))
            {
                isApplyBuffToThisCard = FaceValue <= cardBuff.ThresholdValue;
            }
            else if (cardBuff.CardBuffCondition.Contains(CardBuffCondition.Card_Morethan))
            {
                isApplyBuffToThisCard = FaceValue >= cardBuff.ThresholdValue;
            }

            // apply buff
            if (isApplyBuffToThisCard)
            {
                switch (cardBuff.buffMethod)
                {
                    case BuffMethod.AddValue:
                        temp += cardBuff.Value;

                        break;
                    case BuffMethod.MultiplyValue:
                        temp *= cardBuff.Value;

                        break;

                }
            }
        }


        return temp;
    }
}
