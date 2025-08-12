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

public class CardPlayGameController : MonoBehaviour
{
    [Header("Univasal entry")]
    public static CardPlayGameController current;
    public GameObject playedCardHandle;
    public GameObject playerHand;
    public GameObject operatorButton;
    public GameObject playedCardSlots;
    [SerializeField] private TMP_Text PlayStateText;
    [SerializeField] private TMP_Text actualAnswerText;
    [SerializeField] private TMP_Text previewAnswerText;
    [SerializeField] private TMP_Text targetNumberText;
    [SerializeField] private TMP_Text previewDataText;

    [Header("Core Data")]
    [SerializeField] private double playerAnswer;
    [SerializeField] private double previewPlayerAnswer;
    [SerializeField] private double multiplier;
    [SerializeField] private List<int> OperatorOrders = new List<int>();

    [Header("Card and Deck")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform deckObject;
    public Deck templateDeck;
    [SerializeField] private Deck persistentDeck;
    [SerializeField] private Deck roundDeck;

    [Header("Equation Solver")]
    private List<GameObject> CardInhandGameObject = new List<GameObject>();
    public ParenthesesMode parenthesesMode;
    public int isHandReady = 0;
    public bool isHandValiid = false;
    [SerializeField] List<object[]> steplog = new List<object[]>();

    [Header("Target Number finder")]
    [SerializeField] private List<OperationEnum> posibleOperators = new List<OperationEnum>();
    [SerializeField] private double targetNumber;
    [SerializeField] private double difficulty = 0.5;
    [SerializeField] private double maxAnswerRange = 100;
    private Dictionary<double, List<string>> allPossibleEquationAnswers;

    [Header("Multiplier Finder")]
    //  targetNumber is blueZone (perfect hit)
    [SerializeField] private double blueZoneMultiplier = 3;
    [Space(5)]

    [SerializeField] private double greenZoneValue = 13;
    [SerializeField] private double greenZoneRatio;
    [SerializeField] private double greenZoneMultiplier = 2;
    [Space(5)]

    [SerializeField] private double yellowZoneValue = 25;
    [SerializeField] private double yellowZoneRatio;
    [SerializeField] private double yellowZoneMultiplier = 1;
    [Space(5)]

    // redZone is range outside yellowZone
    [SerializeField] private double redZoneMultiplier = 0.5;

    private void Awake()
    {
        current = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        persistentDeck = Instantiate(templateDeck);
        roundDeck = Instantiate(persistentDeck);

        playedCardSlots = playedCardHandle.transform.Find("NumberCard").gameObject;


        //will have to look on possible operators again once we imperment character
        posibleOperators.Add(OperationEnum.Plus);
        posibleOperators.Add(OperationEnum.Minus);
        posibleOperators.Add(OperationEnum.Multiply);
        posibleOperators.Add(OperationEnum.Divide);
    }

    // Update is called once per frame
    void Update()
    {
        CardInhandGameObject = new List<GameObject>();
        foreach (Transform child in playedCardSlots.transform)
        {
            if(child.childCount > 0)
            {
                CardInhandGameObject.Add(child.GetChild(0).gameObject);
            }
        }

        previewAnswerText.text = previewPlayerAnswer.ToString();
        actualAnswerText.text = playerAnswer.ToString();
        targetNumberText.text = targetNumber.ToString();


        isHandReady = ValidationHand(CardInhandGameObject);
        if (isHandReady < 0)
        {
            PlayStateText.text = "Invalid";
        } else if(isHandReady > 0 && isHandReady < 4)
        {
            PlayStateText.text = "Ready to preview";
            PreviewScore(ParenthesesMode.NoParentheses);
        } else if(isHandReady >= 4)
        {
            PlayStateText.text = "Valid";
            PreviewScore(parenthesesMode);
        }
    }

    public void PreviewScore(ParenthesesMode parentheses)
    {
        if (isHandReady < 0)
        {
            //Debug.Log("invalid hand to preview");
            return;
        }
        //Debug.Log("valid hand go to calculation");

        List<object[]> previewSteplog = new List<object[]>();
        previewSteplog = PlayCardCalculation.EvaluateEquation(CardInhandGameObject, parentheses);
        //foreach (var step in steplog)
        //{
        //    Debug.Log($"{step[0]}, Pos: {step[1]}");
        //}

        previewPlayerAnswer = (double)previewSteplog[previewSteplog.Count - 1][1];
    }

    public void PlayCard()
    {
        if (isHandReady < 4) {
            Debug.Log("invalid hand");
            return; 
        }
        Debug.Log("valid hand go to calculation");

        

        Debug.Log(CardInhandGameObject.Count);

        OperatorOrders = new List<int>();
        steplog = PlayCardCalculation.EvaluateEquation(CardInhandGameObject, parenthesesMode);
        foreach (var step in steplog)
        {
            Debug.Log($"{step[0]}, Pos: {step[1]}");
        }

        for (int i = 0; i < steplog.Count; i++)
        {
            OperatorOrders.Add(Convert.ToInt32(steplog[i][1]));
        }

        previewPlayerAnswer = 0; //reset previewPlayerAnswer for next round
        playerAnswer = (double)steplog[steplog.Count - 1][1];

        GetMultiplierValue();
    }

    private void GetMultiplierValue()
    {
        if (greenZoneRatio > 0)
        {
            greenZoneValue = (greenZoneRatio * targetNumber) + targetNumber;
        }

        if(yellowZoneRatio > 0)
        {
            yellowZoneValue = (yellowZoneRatio * targetNumber) + targetNumber;
        }

        switch (playerAnswer)
        {
            case double i when i == targetNumber:
                multiplier = blueZoneMultiplier;
                break;
            case double i when i <= greenZoneValue && i >= -greenZoneValue:
                multiplier = greenZoneMultiplier;
                break;
            case double i when i <= yellowZoneValue && i >= -yellowZoneValue:
                multiplier = yellowZoneMultiplier;
                break;
            default:
                multiplier = redZoneMultiplier;
                break;
        }

    }

    public void GetAllPossibleEquation()
    {
        // get faceValue of card on hand
        int cardInHandCount = 0;
        List<double> numbers = new List<double>();
        foreach(Transform cardInHand in playerHand.transform)
        {
            if(cardInHand.childCount == 1)
            {
                if(cardInHand.GetChild(0).GetComponent<Card>() != null)
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
        

        Dictionary<double, List<string>>  resultsDict = PlayCardCalculation.GetMostFrequentResults(numbers, posibleOperators);

        if (resultsDict.Count <= 0)
        {
            Debug.Log("there is no equation in the dic, maybe the threshold is too high");
        }

        var sorted = resultsDict.OrderByDescending(kvp => kvp.Value?.Count ?? 0);
        foreach (var kvp in sorted)
        {
            double key = kvp.Key;
            int count = kvp.Value != null ? kvp.Value.Count : 0;
            Debug.Log($"Number: {key}, Count: {count}");
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
        Dictionary<double, List<string>> targetNumberWithItsEquation = PlayCardCalculation.GetAnswerByDifficulty(allPossibleEquationAnswers, difficulty, maxAnswerRange);
        targetNumber = targetNumberWithItsEquation.Keys.First();
        List<string> correctEquation = targetNumberWithItsEquation.Values.First();
        Debug.Log("target number: " + targetNumber);
        Debug.Log("One of correct equation: " + correctEquation[UnityEngine.Random.Range(0, correctEquation.Count-1)]);
    }

    public void AddCard()
    {


        bool isHandHaveSpace = false;
        Transform currentCardSlot = null;
        foreach (Transform cardSlot in playerHand.transform)
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
            CardData SelectedCarddata = roundDeck.GetRandomCard();
            if(SelectedCarddata.Effect == EffectType.Empty)
            {
                return;
            }


            GameObject newCard = Instantiate(cardPrefab, deckObject.position, new Quaternion(), currentCardSlot);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.deckPosition = deckObject.position;
            newCardScript.SetCardData(SelectedCarddata);

            currentCardSlot.gameObject.SetActive(true);
        }
    }

    #region Get Set boi
    public double GetPreviewAnswer()
    {
        return previewPlayerAnswer;
    }

    public double GetAnswer()
    {
        return playerAnswer;
    }

    public double GetMultiplier()
    {
        return multiplier;
    }
     

    public List<int> GetOperatorOrders()
    {
        return OperatorOrders;
    }

    public List<OperatorOrder> GetOperatorOrdersAsEnum()
    {
        List<OperatorOrder> OperatorOrderEnum = new List<OperatorOrder>();
        foreach(int OperatorOrder in OperatorOrders)
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
