using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static Utils;

[CreateAssetMenu(fileName = "deck", menuName = "Scriptable Objects/Card/deck")]
public class Deck : ScriptableObject
{
    public List<CardData> cardDatas;
    public Sprite backCard;
    //DeckObject effect

    public Deck CloneDeck()
    {
        Deck clone = Instantiate(this);
        clone.cardDatas = new List<CardData>(cardDatas);
        return clone;
    }

    public bool TryDrawCard(out CardData card)
    {
        if (cardDatas.Count == 0)
        {
            card = default;
            return false;
        }

        int index = Random.Range(0, cardDatas.Count);
        card = cardDatas[index];
        cardDatas.RemoveAt(index);
        return true;
    }

    public void ChangeCard(CardData target, CardData newCardData)
    {
        for (int i = 0; i < cardDatas.Count; i++)
        {
            if (cardDatas[i].Equals(target))
            {
                cardDatas[i] = newCardData;
                break; // Only change once
            }
        }
    }

    public void AddCardData(CardData newCardData)
    {
        cardDatas.Add(newCardData);
    }
}

[System.Serializable]
public struct CardData
{
    public double FaceValue;
    public double EffectValue;

    public CardData(double faceValue, double effectValue)
    {
        FaceValue = faceValue;
        EffectValue = effectValue;
    }
}
