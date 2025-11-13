using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "deck", menuName = "Scriptable Objects/deck")]
public class Deck : ScriptableObject
{
    public List<CardData> cardDatas;
    public Sprite backCard;
    //deckObject effect

    public CardData GetRandomCard()
    {
        if (cardDatas.Count <= 0)
        {
            Debug.Log("run out of card in deck");
            return new CardData(0, 0, EffectType.Empty);
        }

        int index = Random.Range(0, cardDatas.Count);
        CardData randomCardData = cardDatas[index];
        cardDatas.RemoveAt(index);
        return randomCardData;
    }

    public void ChangeCard(CardData target, CardData newCardData)
    {
        for (int i = 0; i < cardDatas.Count; i++)
        {
            if (cardDatas[i] == target)
            {
                cardDatas[i] = newCardData;
                break; // Only change once
            }
        }
    }

    public void AddCardData(CardData newCardData)
    {
        cardDatas.Append(newCardData);
    }
}

[System.Serializable]
public class CardData
{
    public double FaceValue;
    public double EffectValue;
    public EffectType Effect;

    public CardData(double faceValue, double effectValue, EffectType effect)
    {
        FaceValue = faceValue;
        EffectValue = effectValue;
        Effect = effect;
    }
}
