using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utils;

public class BuffController : MonoBehaviour
{
    public static BuffController current;

    public event Action<int> OnReduceDurationEvent; // positive number for amount of reduction, -1 for remove all buff instantly
    public event Action<GameObject> OnSelectedConsumableUpdate;

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
            if (SelectedConsumable != null && value.GetComponent<Character>() != null)
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
            //for toggle selected
            if (selectedConsumable != null)
            {
                selectedConsumable = null;
            }
            else if (value.GetComponent<ConsumableData>() != null)
            {
                selectedConsumable = value;
            }

            OnSelectedConsumableUpdate?.Invoke(selectedConsumable);
        }
    }

    private void Awake()
    {
        current = this;
    }

    public bool UseConsumable()
    {
        ConsumableData consumableData = selectedConsumable.GetComponent<ConsumableData>();
        if(consumableData = null)
        {
            Debug.Log("no consumable on consumableData");
            return false;
        }

        if (consumableData.UsableOn.Contains(CharacterType.CardSlot))
        {
            //use only card buff
            return SendBuffToCardSlot(consumableData.CardBuffOnUse);
        }

        CharacterSlotsHolder heroCharacterSlotsHolder = ActionGameController.current.HeroSlotsHolder;
        CharacterSlotsHolder enemyCharacterSlotsHolder = ActionGameController.current.EnemySlotsHolder;
        List<GameObject> characterSlots = new List<GameObject>();
        List<Character> characters = new List<Character>();

        if (consumableData.UsableOn.Contains(CharacterType.Character_OnlySelected))
        {
            if (selectedCharacter != null)
            {
                characters.Add(selectedCharacter.GetComponent<Character>());
            }
        }

        foreach (CharacterType targetCharacter in consumableData.UsableOn)
        {
            switch (targetCharacter)
            {
                case CharacterType.Hero_All:
                    characterSlots.AddRange(heroCharacterSlotsHolder.GetAllCharactersAsList());
                    break;
                case CharacterType.Hero_Plus:
                    if (GameController.current.PossibleOperators.Contains(OperationEnum.Plus))
                    {
                        characterSlots.Add(ActionGameController.current.PlusHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Minus:
                    if (GameController.current.PossibleOperators.Contains(OperationEnum.Minus))
                    {
                        characterSlots.Add(ActionGameController.current.MinusHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Multiply:
                    if (GameController.current.PossibleOperators.Contains(OperationEnum.Multiply))
                    {
                        characterSlots.Add(ActionGameController.current.MultiplyHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Divide:
                    if (GameController.current.PossibleOperators.Contains(OperationEnum.Divide))
                    {
                        characterSlots.Add(ActionGameController.current.DivideHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Buff:
                    if (GameController.current.PossibleOperators.Contains(OperationEnum.Buff))
                    {
                        characterSlots.Add(ActionGameController.current.BuffHeroSlot);
                    }
                    break;

                case CharacterType.Enemy_All:
                    characterSlots.AddRange(enemyCharacterSlotsHolder.GetAllCharactersAsList());
                    break;
                case CharacterType.Enemy_Front:
                    characterSlots.AddRange(enemyCharacterSlotsHolder.GetFirstCharacterAsList());
                    break;
                case CharacterType.Enemy_Back:
                    characterSlots.AddRange(enemyCharacterSlotsHolder.GetLastCharacterAsList());
                    break;
                case CharacterType.Enemy_FirstTwo:
                    characterSlots.AddRange(enemyCharacterSlotsHolder.GetFirstTwoCharactersAsList());
                    break;

                default:
                    // this include Enemy, CardSlot,Character_OnlySelected
                    continue;

            }

        }
    
        foreach(GameObject characterslot in characterSlots)
        {
            if(characterslot.transform.childCount >= 1)
            {
                characters.Add(characterslot.transform.GetChild(0).GetComponent<Character>());
            }
        }

        characters = characters.Distinct().ToList();

        return SendBuffToCharacters(consumableData.CharacterBuffOnUse, characters);
    }

    bool SendBuffToCardSlot(List<CardBuff> cardBuff)
    {
        List<CardBuff> copiedCardBuffs = new List<CardBuff>();

        foreach (CardBuff buff in cardBuff)
        {
            CardBuff buffCopy = ScriptableObject.Instantiate(buff);
            copiedCardBuffs.Add(buffCopy);
        }

        CurrentCardBuffs.AddRange(copiedCardBuffs);
        return true;
    }

    bool SendBuffToCharacters(List<CharacterBuff> characterBuffs, List<Character> characters)
    {
        foreach(Character character in characters)
        {

            List<CharacterBuff> copiedCharacterBuffs = new List<CharacterBuff>();

            foreach (CharacterBuff buff in characterBuffs)
            {
                CharacterBuff buffCopy = ScriptableObject.Instantiate(buff);
                copiedCharacterBuffs.Add(buffCopy);
            }

            character.AddCharacterBuffs(copiedCharacterBuffs);
        }
        return true;
    }

    public void ReduceAllCurrentBuffDuration()
    {
        OnReduceDurationEvent?.Invoke(1);


        foreach (CardBuff cardBuff in CurrentCardBuffs)
        {
            cardBuff.ReduceDuration();
        }
        CurrentCardBuffs.RemoveAll(cardBuff => cardBuff.Duration <= 0);
    }

    public void RemoveAllBuff()
    {
        Debug.Log("remove all buff!");
        OnReduceDurationEvent?.Invoke(-1);
        CurrentCardBuffs.Clear();
    }
}
