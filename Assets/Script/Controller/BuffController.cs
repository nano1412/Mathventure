using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utils;

public class BuffController : MonoBehaviour
{
    public static BuffController current;

    public event Action<int> OnBuffTakeEffect; // positive number for amount of reduction, -1 for remove all buff instantly
    public event Action<GameObject> OnSelectedCharacterUpdate;
    public event Action<GameObject> OnSelectedConsumableUpdate;

    [field: SerializeField] public List<CardBuff> CurrentCardBuffs { get; private set; }
    [field: SerializeField] public AudioSource useConsumableAudiosource { get; private set; }

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
                OnSelectedCharacterUpdate?.Invoke(selectedCharacter);
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
            if (selectedConsumable != null || value == null)
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
        if(consumableData == null)
        {
            Debug.Log("no consumable on consumableData");
            return false;
        }

        if (consumableData.UsableOn.Contains(CharacterType.CardSlot))
        {
            Debug.LogWarning("UseCardbuff");
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
            } else
            {
                Debug.LogWarning("no selectedCharacter");
                return false;
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
                    if (HeroToggler.current.IsPlusHeroOnThisLevel)
                    {
                        characterSlots.Add(ActionGameController.current.PlusHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Minus:
                    if (HeroToggler.current.IsMinusHeroOnThisLevel)
                    {
                        characterSlots.Add(ActionGameController.current.MinusHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Multiply:
                    if (HeroToggler.current.IsMultiplyHeroOnThisLevel)
                    {
                        characterSlots.Add(ActionGameController.current.MultiplyHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Divide:
                    if (HeroToggler.current.IsDivideHeroOnThisLevel)
                    {
                        characterSlots.Add(ActionGameController.current.DivideHeroSlot);
                    }
                    break;
                case CharacterType.Hero_Buff:
                    if (HeroToggler.current.IsBuffHeroOnThisLevel)
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
            if(characterslot.transform.GetComponentInChildren<Character>() != null)
            {
                characters.Add(characterslot.transform.GetComponentInChildren<Character>());
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

    public void AllCharactersTakeBuffsEffect()
    {
        OnBuffTakeEffect?.Invoke(1);


        foreach (CardBuff cardBuff in CurrentCardBuffs)
        {
            cardBuff.ReduceDuration();
        }
        CurrentCardBuffs.RemoveAll(cardBuff => cardBuff.Duration <= 0);
    }

    public void RemoveAllBuff()
    {
        Debug.Log("remove all buff!");
        OnBuffTakeEffect?.Invoke(-1);
        CurrentCardBuffs.Clear();
    }
}
