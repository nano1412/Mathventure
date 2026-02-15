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
    [field: SerializeField] public List<CardBuffCondition> CardTypes { get; private set; } = new List<CardBuffCondition>();

    private void Start()
    {
        SetupCardType();
    }

    private void SetupCardType()
    {
        CardTypes.Clear();
        CardTypes.Add(CardBuffCondition.Card_All);
        if (FaceValue % 2 == 0)
        {
            CardTypes.Add(CardBuffCondition.Card_IsEven);
        } else
        {
            CardTypes.Add(CardBuffCondition.Card_IsOdd);
        }

        if (Utils.IsPrimeFromDouble(FaceValue))
        {
            CardTypes.Add(CardBuffCondition.Card_IsPrime);
        }
    }

    public void SetFaceValue(double value)
    {
        FaceValue = value;
        SetupCardType();
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
            //debug
            //Debug.Log("this card face value:" + FaceValue + " and type are:");
            //foreach(CardBuffCondition cardType in CardTypes)
            //{
            //    Debug.Log(cardType.ToString());
            //}
            //end debug

        foreach (CardBuff cardBuff in BuffController.current.CurrentCardBuffs)
        {
            //debug
            //Debug.Log("this buff condition:");
            //foreach (CardBuffCondition cardBuffCondition in cardBuff.CardBuffConditions)
            //{
            //    Debug.Log(cardBuffCondition.ToString());
            //}
            //end debug

            //check condition
            bool isApplyBuffToThisCard = cardBuff.CardBuffConditions.Any(x => CardTypes.Contains(x));   

            if (cardBuff.CardBuffConditions.Contains(CardBuffCondition.Card_Equal))
            {
                isApplyBuffToThisCard = FaceValue == cardBuff.ThresholdValue;
            } else if (cardBuff.CardBuffConditions.Contains(CardBuffCondition.Card_Lessthan))
            {
                isApplyBuffToThisCard = FaceValue <= cardBuff.ThresholdValue;
            }
            else if (cardBuff.CardBuffConditions.Contains(CardBuffCondition.Card_Morethan))
            {
                isApplyBuffToThisCard = FaceValue >= cardBuff.ThresholdValue;
            }

            // apply buff
            if (isApplyBuffToThisCard)
            {
                //Debug.Log("Buff apply!");
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
