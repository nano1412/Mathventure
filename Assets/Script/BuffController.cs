using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public static BuffController current;

    [field:SerializeField] public List<CharacterBuff> CurrentCharacterBuffs { get; private set; }
    [field: SerializeField] public List<CardBuff> CurrentCardBuffs { get; private set; }

    [field: SerializeField] private GameObject selectedCharacter;
    public GameObject SelectedCharacter
    {
        get
        {
            return selectedCharacter;
        }
        set
        {
            if(SelectedConsumable != null && value.GetComponent<Character>() != null)
            {
                selectedCharacter = value;
            }
        }
    }

    [field: SerializeField] private GameObject selectedConsumable;
    public GameObject SelectedConsumable
    {
        get
        {
            return selectedConsumable;
        }
        set
        {
            if(value.GetComponent<ConsumableData>() != null)
            {
                selectedConsumable = value;
            }
        }
    }

    private void Awake()
    {
        current = this;
    }

    public void ReduceAllCurrentBuffDuration()
    {
        Debug.Log("reduce all buff duration");
        foreach (CharacterBuff characterBuff in CurrentCharacterBuffs)
        {
            characterBuff.ReduceDuration();
        }
        CurrentCharacterBuffs.RemoveAll(characterBuff => characterBuff.Duration <= 0);

        foreach (CardBuff cardBuff in CurrentCardBuffs)
        {
            cardBuff.ReduceDuration();
        }
        CurrentCardBuffs.RemoveAll(cardBuff => cardBuff.Duration <= 0);
    }

    public void RemoveAllBuff()
    {
        Debug.Log("remove all buff!");
        CurrentCharacterBuffs.Clear();
        CurrentCardBuffs.Clear();
    }
}
