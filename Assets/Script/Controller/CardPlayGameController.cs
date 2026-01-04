using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Utils;
using static PlayCardCalculation;
using UnityEngine.Windows;
using System.Linq;
using System;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor;
using UnityEngine.Serialization;
using Unity.VisualScripting;

public class CardPlayGameController : MonoBehaviour
{
    public static CardPlayGameController current;

    [field: Header("Univasal entry"), SerializeField]
    public GameObject PlayerHand { get; private set; }

    [field: SerializeField]
    public GameObject OperatorButton { get; private set; }

    [field: SerializeField]
    public GameObject PlayedCardSlots { get; private set; }

    [field: SerializeField] public Transform ParenthesesRester { get; private set; }


    [field: Header("Play CardEntity Slot"), SerializeField]
    public List<GameObject> PlayCardSlotList { get; private set; }
    [field: SerializeField]
    public List<GameObject> PlayCardList { get; private set; }

    [field: SerializeField] public GameObject PlayNumberSlot1 { get; private set; }
    [field: SerializeField] public GameObject PlayOperatorSlot1 { get; private set; }
    [field: SerializeField] public GameObject PlayNumberSlot2 { get; private set; }
    [field: SerializeField] public GameObject PlayOperatorSlot2 { get; private set; }
    [field: SerializeField] public GameObject PlayNumberSlot3 { get; private set; }
    [field: SerializeField] public GameObject PlayOperatorSlot3 { get; private set; }
    [field: SerializeField] public GameObject PlayNumberSlot4 { get; private set; }

    [field: SerializeField] public Transform PlayOpenParentheses1 { get; private set; }
    [field: SerializeField] public Transform PlayCloseParentheses1 { get; private set; }
    [field: SerializeField] public Transform PlayOpenParentheses2 { get; private set; }
    [field: SerializeField] public Transform PlayCloseParentheses2 { get; private set; }


    [field: Header("Core Data"), SerializeField]
    public double PlayerAnswer { get; private set; }

    [field: SerializeField]
    public double PreviewPlayerAnswer { get; private set; }

    [field: SerializeField]
    public double Multiplier { get; private set; }

    [field: SerializeField]
    public List<int> OperatorOrders { get; private set; }


    [field: Header("CardEntity and Deck"), SerializeField]
    public GameObject CardPrefab { get; private set; }

    [field: SerializeField]
    public Transform DeckObject { get; private set; }

    [field: SerializeField]
    public Deck PersistentDeck { get; private set; }

    [field: SerializeField]
    public Deck RoundDeck { get; private set; }


    [Header("Equation Solver")]
    private List<GameObject> CardInhandGameObject = new();

    private ParenthesesMode parenthesesMode;
    public ParenthesesMode ParenthesesMode
    {
        get
        {
            return parenthesesMode;
        }

        set
        {
            if (value == parenthesesMode) return;
            parenthesesMode = value;
            UpdateParenthesesGameobjectPosition();
        }
    }

    [field: SerializeField]
    public int IsHandReady { get; private set; }

    [field: SerializeField]
    public bool IsHandValiid { get; private set; }

    [field: SerializeField]
    public List<object[]> StepLog { get; private set; }


    [field: Header("Target Number Finder"), SerializeField]
    public List<OperationEnum> PossibleOperators { get; private set; }

    [field: SerializeField]
    public double TargetNumber { get; private set; }

    [field: SerializeField]
    public double Difficulty { get; private set; }

    [field: SerializeField]
    public double MaxAnswerRange { get; private set; }

    [field: SerializeField]
    public bool IsPositiveOnly { get; private set; }

    private Dictionary<double, List<string>> allPossibleEquationAnswers;


    [field: Header("Multiplier Finder"), SerializeField]
    public double BlueZoneMultiplier { get; private set; }

    [field: Space(5), SerializeField]
    public double GreenZoneValue { get; private set; }

    [field: SerializeField]
    public double GreenZoneRatio { get; private set; }

    [field: SerializeField]
    public double GreenZoneMultiplier { get; private set; }

    [field: Space(5), SerializeField]
    public double YellowZoneValue { get; private set; }

    [field: SerializeField]
    public double YellowZoneRatio { get; private set; }

    [field: SerializeField]
    public double YellowZoneMultiplier { get; private set; }

    [field: Space(5), SerializeField]
    public double RedZoneMultiplier { get; private set; }

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }

    private void OnEnable()
    {
        GameController.current.OnGameStateChange += RemoveAllCardInPlayedHand;
    }

    private void OnDisable()
    {
        GameController.current.OnGameStateChange -= RemoveAllCardInPlayedHand;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
    }

    public void SetupCardContoller()
    {
        PersistentDeck = GameController.current.TemplateDeck.CloneDeck();
        RoundDeck = PersistentDeck.CloneDeck();
        PossibleOperators = GameController.current.PossibleOperators;
        IsPositiveOnly = GameController.current.Level < 2;

        PlayCardSlotList.Add(PlayNumberSlot1);
        PlayCardSlotList.Add(PlayOperatorSlot1);
        PlayCardSlotList.Add(PlayNumberSlot2);
        PlayCardSlotList.Add(PlayOperatorSlot2);
        PlayCardSlotList.Add(PlayNumberSlot3);
        PlayCardSlotList.Add(PlayOperatorSlot3);
        PlayCardSlotList.Add(PlayNumberSlot4);
    }

    // Update is called once per frame
    void Update()
    {
        PlayCardList = CardslotsToCards();
        CardInhandGameObject = new List<GameObject>();
        foreach (Transform child in PlayedCardSlots.transform)
        {
            if (child.childCount > 0)
            {
                CardInhandGameObject.Add(child.GetChild(0).gameObject);
            }
        }

        IsHandReady = ValidationHand(PlayCardList);
        if (CardPlayGameController.current.IsHandReady > 0 && CardPlayGameController.current.IsHandReady < 4)
        {
            PreviewScore(ParenthesesMode.NoParentheses);
        }
        else if (CardPlayGameController.current.IsHandReady >= 4)
        {
            PreviewScore(ParenthesesMode);
        }

    }

    public void PreviewScore(ParenthesesMode parentheses)
    {
        if (IsHandReady < 0)
        {
            //Debug.Log("invalid hand to preview");
            return;
        }
        //Debug.Log("valid hand go to calculation");

        List<object[]> previewSteplog = new List<object[]>();
        previewSteplog = PlayCardCalculation.EvaluateEquation(PlayCardList, parentheses);
        //foreach (var step in StepLog)
        //{
        //    Debug.Log($"{step[0]}, Pos: {step[1]}");
        //}

        PreviewPlayerAnswer = (double)previewSteplog[previewSteplog.Count - 1][1];
    }

    public void SummitEquation(Action onComplete)
    {
        if (IsHandReady < 4)
        {
            Debug.Log("invalid hand");
            return;
        }
        Debug.Log("valid hand go to calculation");



        //Debug.Log(CardInhandGameObject.Count);

        OperatorOrders = new List<int>();
        StepLog = PlayCardCalculation.EvaluateEquation(PlayCardList, ParenthesesMode);
        foreach (var step in StepLog)
        {
            Debug.Log($"{step[0]}, Pos: {step[1]}");
        }

        for (int i = 0; i < StepLog.Count; i++)
        {
            OperatorOrders.Add(Convert.ToInt32(StepLog[i][1]));
        }

        PreviewPlayerAnswer = 0; //reset PreviewPlayerAnswer for next round
        ParenthesesMode = ParenthesesMode.NoParentheses;
        PlayerAnswer = (double)StepLog[StepLog.Count - 1][1];

        GetMultiplierValue();

        onComplete?.Invoke();
    }

    private void GetMultiplierValue()
    {
        if (GreenZoneRatio > 0)
        {
            GreenZoneValue = (GreenZoneRatio * TargetNumber) + TargetNumber;
        }

        if (YellowZoneRatio > 0)
        {
            YellowZoneValue = (YellowZoneRatio * TargetNumber) + TargetNumber;
        }

        switch (PlayerAnswer)
        {
            case double i when i == TargetNumber:
                Multiplier = BlueZoneMultiplier;
                break;
            case double i when i <= GreenZoneValue && i >= -GreenZoneValue:
                Multiplier = GreenZoneMultiplier;
                break;
            case double i when i <= YellowZoneValue && i >= -YellowZoneValue:
                Multiplier = YellowZoneMultiplier;
                break;
            default:
                Multiplier = RedZoneMultiplier;
                break;
        }

    }

    public void GetAllPossibleEquation()
    {
        // get FaceValue of card on hand
        int cardInHandCount = 0;
        List<double> numbers = new List<double>();


        foreach (Transform cardInHand in PlayerHand.transform)
        {
            if (cardInHand.childCount == 1)
            {
                if (cardInHand.GetChild(0).GetComponent<CardData>() != null)
                {
                    cardInHandCount++;
                    numbers.Add(cardInHand.GetChild(0).GetComponent<CardData>().FaceValue);
                }
            }
        }

        if (cardInHandCount < 4)
        {
            Debug.Log("player have fewer than 4 card. they cant play it");
            return;
        }

        //get all possible operator that player can play
        // as for now we will just set that player can play all 4 operators


        Dictionary<double, List<string>> resultsDict = PlayCardCalculation.GetMostFrequentResults(numbers, PossibleOperators);

        if (resultsDict.Count <= 0)
        {
            Debug.Log("there is no equation in the dic, maybe the threshold is too high");
        }

        var sorted = resultsDict.OrderByDescending(kvp => kvp.Value?.Count ?? 0);
        foreach (var kvp in sorted)
        {
            double key = kvp.Key;
            int count = kvp.Value != null ? kvp.Value.Count : 0;
            //Debug.Log($"Number: {key}, Count: {count}");
        }

        allPossibleEquationAnswers = resultsDict;
    }

    public void GetTargetNumber()
    {
        if (allPossibleEquationAnswers.Count <= 0 || allPossibleEquationAnswers == null)
        {
            Debug.Log("there is no equation in the dic, maybe the threshold is too high");
            return;
        }
        Dictionary<double, List<string>> targetNumberWithItsEquation = PlayCardCalculation.GetAnswerByDifficulty(allPossibleEquationAnswers, Difficulty, MaxAnswerRange, IsPositiveOnly);
        TargetNumber = targetNumberWithItsEquation.Keys.First();
        List<string> correctEquation = targetNumberWithItsEquation.Values.First();
        Debug.Log("target number: " + TargetNumber);
        Debug.Log("number of possible equation " + correctEquation.Count);
        Debug.Log("One of correct equation: " + correctEquation[UnityEngine.Random.Range(0, correctEquation.Count - 1)]);
    }

    public void AddCardButton()
    {
        AddCardTohand(1);
    }

    public void AddCardTohand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {


            bool isHandHaveSpace = false;
            Transform currentCardSlot = null;
            foreach (Transform cardSlot in PlayerHand.transform)
            {
                if (cardSlot.CompareTag("Slot") && cardSlot.transform.childCount == 0)
                {
                    currentCardSlot = cardSlot;
                    isHandHaveSpace = true;
                    break;
                }
            }

            if (isHandHaveSpace)
            {


                if (!RoundDeck.TryDrawCard(out CardInDeckData SelectedCarddata))
                {
                    Debug.Log("Deck empty");
                    return;
                }


                GameObject newCard = Instantiate(CardPrefab, DeckObject.position, new Quaternion(), currentCardSlot);
                CardEntity newCardScript = newCard.GetComponent<CardEntity>();
                newCardScript.deckPosition = DeckObject.position;
                newCardScript.GetComponent<CardData>().SetCardData(SelectedCarddata);

                currentCardSlot.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("hand already full");
                return;
            }

            if (PlayerHand.transform.childCount > GameController.current.MaxCardInHand)
            {
                return;
            }
        }
    }

    private void UpdateParenthesesGameobjectPosition()
    {
        // reset
        PlayOpenParentheses1.gameObject.SetActive(true);
        PlayCloseParentheses1.gameObject.SetActive(true);
        PlayOpenParentheses2.gameObject.SetActive(true); ;
        PlayCloseParentheses2.gameObject.SetActive(true); ;
        PlayOpenParentheses1.SetParent(ParenthesesRester, false);
        PlayCloseParentheses1.SetParent(ParenthesesRester, false);
        PlayOpenParentheses2.SetParent(ParenthesesRester, false);
        PlayCloseParentheses2.SetParent(ParenthesesRester, false);

        int numberSiblingIndex1 = PlayNumberSlot1.transform.GetSiblingIndex();
        int numberSiblingIndex2 = PlayNumberSlot2.transform.GetSiblingIndex();
        int numberSiblingIndex3 = PlayNumberSlot3.transform.GetSiblingIndex();
        int numberSiblingIndex4 = PlayNumberSlot4.transform.GetSiblingIndex();
        int increment = 1;

        switch (ParenthesesMode)
        {
            case ParenthesesMode.NoParentheses: // NoParentheses
                break;
            case ParenthesesMode.DoFrontOperationFirst: // (XX)XX
                PlayOpenParentheses1.SetParent(PlayedCardSlots.transform, false);
                PlayCloseParentheses1.SetParent(PlayedCardSlots.transform, false);

                PlayOpenParentheses1.SetSiblingIndex(numberSiblingIndex1);
                PlayCloseParentheses1.SetSiblingIndex(numberSiblingIndex2 + 1 + increment++);

                break;
            case ParenthesesMode.DoMiddleOperationFirst: // X(XX)X
                PlayOpenParentheses1.SetParent(PlayedCardSlots.transform, false);
                PlayCloseParentheses1.SetParent(PlayedCardSlots.transform, false);

                PlayOpenParentheses1.SetSiblingIndex(numberSiblingIndex2);
                PlayCloseParentheses1.SetSiblingIndex(numberSiblingIndex3 + 1 + increment++);

                break;
            case ParenthesesMode.DoLastOperationFirst: // XX(XX)
                PlayOpenParentheses1.SetParent(PlayedCardSlots.transform, false);
                PlayCloseParentheses1.SetParent(PlayedCardSlots.transform, false);

                PlayOpenParentheses1.SetSiblingIndex(numberSiblingIndex3);
                PlayCloseParentheses1.SetSiblingIndex(numberSiblingIndex4 + 1 + increment++);
                break;
            case ParenthesesMode.DoMiddleOperationLast: // (XX)(XX)
                PlayOpenParentheses1.SetParent(PlayedCardSlots.transform, false);
                PlayCloseParentheses1.SetParent(PlayedCardSlots.transform, false);
                PlayOpenParentheses2.SetParent(PlayedCardSlots.transform, false);
                PlayCloseParentheses2.SetParent(PlayedCardSlots.transform, false);

                PlayOpenParentheses1.SetSiblingIndex(numberSiblingIndex1);
                PlayCloseParentheses1.SetSiblingIndex(numberSiblingIndex2 + 1 + increment++);
                PlayOpenParentheses2.SetSiblingIndex(numberSiblingIndex3 + increment++);
                PlayCloseParentheses2.SetSiblingIndex(numberSiblingIndex4 + 1 + increment++);
                break;
        }
    }

    public void RemoveAllCardInPlayedHand(GameState gameState)
    {
        if (gameState == GameState.RoundVictory || gameState == GameState.Lose || gameState == GameState.Shop || gameState == GameState.Win)
        {
            foreach (var card in PlayCardList)
            {
                Destroy(card);
            }
            PlayCardList.Clear();

        }
    }

    public bool AddCardToPersistentDeck(GameObject item)
    {
        if(item.GetComponent<CardInShopData>() == null) { return false; }

        CardInShopData cardInShopData = item.GetComponent<CardInShopData>();

        CardInDeckData cardData = new CardInDeckData(cardInShopData.FaceValue, cardInShopData.EffectValue);

        PersistentDeck.AddCardData(cardData);

        return true;


    }

    #region Get Set boi

    public List<OperatorOrder> GetOperatorOrdersAsEnum()
    {
        List<OperatorOrder> OperatorOrderEnum = new List<OperatorOrder>();
        foreach (int OperatorOrder in OperatorOrders)
        {
            bool isValid = Enum.IsDefined(typeof(OperatorOrder), OperatorOrder);
            if (isValid)
            {
                OperatorOrderEnum.Add((OperatorOrder)OperatorOrder);
            }
        }

        return OperatorOrderEnum;
    }

    public List<GameObject> CardslotsToCards()
    {
        List<GameObject> cards = new();

        foreach (GameObject cardslot in PlayCardSlotList)
        {
            if (cardslot.transform.childCount > 0)
            {
                cards.Add(cardslot.transform.GetChild(0).gameObject);
            }
        }

        return cards;
    }

    public void ResetRoundDeck()
    {
        RoundDeck = PersistentDeck.CloneDeck();
    }

    #endregion
}
