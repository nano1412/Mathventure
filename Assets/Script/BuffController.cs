using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public static BuffController current;

    [field:SerializeField] public List<CharacterBuff> CurrentCharacterBuff { get; private set; }
    [field: SerializeField] public List<CardBuff> CurrentCardBuff { get; private set; }

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
        foreach(CharacterBuff characterBuff in CurrentCharacterBuff)
        {
            characterBuff.ReduceDuration();
        }
        CurrentCharacterBuff.RemoveAll(characterBuff => characterBuff.Duration <= 0);

        foreach (CardBuff cardBuff in CurrentCardBuff)
        {
            cardBuff.ReduceDuration();
        }
        CurrentCardBuff.RemoveAll(cardBuff => cardBuff.Duration <= 0);
    }
}
