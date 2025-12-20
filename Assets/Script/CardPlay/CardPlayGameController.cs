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

public class CardPlayGameController : MonoBehaviour
{
    public static CardPlayGameController current;

    [field: Header("Univasal entry"), SerializeField]
    public GameObject PlayerHand { get; private set; }

    [field: SerializeField]
    public GameObject OperatorButton { get; private set; }

    [field: SerializeField]
    public GameObject PlayedCardSlots { get; private set; }


    [field: Header("Play Card Slot"), SerializeField]
    public List<Transform> PlayCardList { get; private set; }

    [field: SerializeField] public Transform PlayNumberSlot1 { get; private set; }
    [field: SerializeField] public Transform PlayOperatorSlot1 { get; private set; }
    [field: SerializeField] public Transform PlayNumberSlot2 { get; private set; }
    [field: SerializeField] public Transform PlayOperatorSlot2 { get; private set; }
    [field: SerializeField] public Transform PlayNumberSlot3 { get; private set; }
    [field: SerializeField] public Transform PlayOperatorSlot3 { get; private set; }
    [field: SerializeField] public Transform PlayNumberSlot4 { get; private set; }

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


    [field: Header("Card and Deck"), SerializeField]
    public GameObject CardPrefab { get; private set; }

    [field: SerializeField]
    public Transform DeckObject { get; private set; }

    [field: SerializeField]
    public Deck PersistentDeck { get; private set; }

    [field: SerializeField]
    public Deck RoundDeck { get; private set; }


    [Header("Equation Solver")]
    private List<GameObject> CardInhandGameObject = new();

    public ParenthesesMode ParenthesesMode;

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
        current = this;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
    }

    public void SetupCardContoller()
    {
        PersistentDeck = Instantiate(GameController.current.TemplateDeck);
        RoundDeck = Instantiate(PersistentDeck);
        PossibleOperators = GameController.current.PossibleOperators;
        IsPositiveOnly = GameController.current.Level < 2;

        PlayCardList.Add(PlayNumberSlot1);
        PlayCardList.Add(PlayOperatorSlot1);
        PlayCardList.Add(PlayNumberSlot2);
        PlayCardList.Add(PlayOperatorSlot2);
        PlayCardList.Add(PlayNumberSlot3);
        PlayCardList.Add(PlayOperatorSlot3);
        PlayCardList.Add(PlayNumberSlot4);
}

    // Update is called once per frame
    void Update()
    {
        CardInhandGameObject = new List<GameObject>();
        foreach (Transform child in PlayedCardSlots.transform)
        {
            if (child.childCount > 0)
            {
                CardInhandGameObject.Add(child.GetChild(0).gameObject);
            }
        }

        IsHandReady = ValidationHand(CardInhandGameObject);
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
        previewSteplog = PlayCardCalculation.EvaluateEquation(CardInhandGameObject, parentheses);
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
        StepLog = PlayCardCalculation.EvaluateEquation(CardInhandGameObject, ParenthesesMode);
        foreach (var step in StepLog)
        {
            Debug.Log($"{step[0]}, Pos: {step[1]}");
        }

        for (int i = 0; i < StepLog.Count; i++)
        {
            OperatorOrders.Add(Convert.ToInt32(StepLog[i][1]));
        }

        PreviewPlayerAnswer = 0; //reset PreviewPlayerAnswer for next round
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
        // get faceValue of card on hand
        int cardInHandCount = 0;
        List<double> numbers = new List<double>();


        foreach (Transform cardInHand in PlayerHand.transform)
        {
            if (cardInHand.childCount == 1)
            {
                if (cardInHand.GetChild(0).GetComponent<Card>() != null)
                {
                    cardInHandCount++;
                    numbers.Add(cardInHand.GetChild(0).GetComponent<Card>().GetFaceValue());
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
        AddCard(1);
    }

    public void AddCard(int amount)
    {
        for(int i = 0; i < amount; i++)
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
                CardData SelectedCarddata = RoundDeck.GetRandomCard();
                if (SelectedCarddata.Effect == EffectType.Empty)
                {
                    return;
                }


                GameObject newCard = Instantiate(CardPrefab, DeckObject.position, new Quaternion(), currentCardSlot);
                Card newCardScript = newCard.GetComponent<Card>();
                newCardScript.deckPosition = DeckObject.position;
                newCardScript.SetCardData(SelectedCarddata);

                currentCardSlot.gameObject.SetActive(true);
            } else
            {
                Debug.Log("hand already full");
                return;
            }
        }
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



    #endregion
}
