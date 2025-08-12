using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Utils;
using static PlayCardCalculation;
using UnityEngine.Windows;
using System.Linq;

public class CardPlayGameController : MonoBehaviour
{
    [Header("Univasal entry")]
    public static CardPlayGameController current;
    public GameObject playedCardHandle;
    public GameObject playerHand;
    public GameObject operatorButton;
    public GameObject playedCardSlots;
    public GameObject PlayStateText;

    [Header("Core Data")]
    [SerializeField] private double playerAnswer;
    [SerializeField] private double multiplier;
    [SerializeField] private List<int> OperatorOrders = new List<int>();

    [Header("Card and Deck")]
    [SerializeField] private GameObject card;
    [SerializeField] private Transform deckObject;
    public Deck templateDeck;
    [SerializeField] private Deck persistentDeck;
    [SerializeField] private Deck roundDeck;

    [Header("Equation Solver")]
    public ParenthesesMode parenthesesMode;
    public bool isHandReady = false;
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
        PlayStateText = playedCardHandle.transform.Find("PlayState").gameObject;


        //will have to look on possible operators again once we imperment character
        posibleOperators.Add(OperationEnum.Plus);
        posibleOperators.Add(OperationEnum.Minus);
        posibleOperators.Add(OperationEnum.Multiply);
        posibleOperators.Add(OperationEnum.Divide);
    }

    // Update is called once per frame
    void Update()
    {
        isHandReady = CheckIfPlayedCardReadyToplay();
        if (isHandReady)
        {
            PlayStateText.GetComponent<TMP_Text>().text = "Valid";
        } else
        {
            PlayStateText.GetComponent<TMP_Text>().text = "Invalid";
        }
    }

    private bool CheckIfPlayedCardReadyToplay()
    {
        int cardCount = 0;
        int operatorCount = 0;

        foreach(Transform slot in playedCardSlots.transform)
        {
            //slot.GetChild(0);

            if (slot.childCount == 1 && slot.GetChild(0).GetComponent<Card>()!= null && slot.CompareTag("Slot"))
            {
                cardCount++;
            }

            if (slot.childCount == 1 && slot.GetChild(0).GetComponent<Operatorcard>() != null && slot.CompareTag("OperatorSlot"))
            {
                operatorCount++;
            }
        }

        if(cardCount == 4 && operatorCount == 3)
        {
            return true;
            
        } else
        {
            return false;
        }
    }

    public void PreviewScore()
    {

    }

    public void PlayCard()
    {
        if (!isHandReady) {
            Debug.Log("invalid hand");
            return; 
        }
        Debug.Log("valid hand go to calculation");
            List<GameObject> cardsPlayed = new List<GameObject>();

        foreach (Transform child in playedCardSlots.transform)
        {
            cardsPlayed.Add(child.GetChild(0).gameObject);
        }

        Debug.Log(cardsPlayed.Count);

        OperatorOrders = new List<int>();
        steplog = PlayCardCalculation.EvaluateEquation(cardsPlayed, parenthesesMode);
        foreach (var step in steplog)
        {
            Debug.Log($"{step[0]}, Pos: {step[1]}");
            OperatorOrders.Add((int)step[1]);
        }

        playerAnswer = (double)steplog[steplog.Count - 1][1];

        GetMultiplier();
    }

    private void GetMultiplier()
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

        //get all possible operator that player can play
        // as for now we will just set that player can play all 4 operators
        

        if (cardInHandCount < 4)
        {
            Debug.Log("player have fewer than 4 card. they cant play it");
            return;
        }

        //run the function to get value
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
        Debug.Log("One of correct equation: " + correctEquation[Random.Range(0, correctEquation.Count-1)]);
    }

    public double DoOperation(double a,double b, OperationEnum operation)
    {
        //double.NegativeInfinity mean Invalid, preview must say so and should be able to play this hand
        switch (operation)
        {
            case OperationEnum.Plus:
                return a + b;


            case OperationEnum.Minus:
                return a - b;


            case OperationEnum.Multiply:
                return a * b;


            case OperationEnum.Divide:
                if(b == 0)
                {
                    return double.NegativeInfinity;
                }
                return a / b;
                // dont forget to check for divide by zero

        }

        return double.NegativeInfinity;
    }

    public bool RectOverlaps(RectTransform rt1, RectTransform rt2)
    {
        Rect r1 = GetWorldRect(rt1);
        Rect r2 = GetWorldRect(rt2);
        return r1.Overlaps(r2);
    }

    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft, topRight - bottomLeft);
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


            GameObject newCard = Instantiate(card, deckObject.position, new Quaternion(), currentCardSlot);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.deckPosition = deckObject.position;
            newCardScript.SetCardData(SelectedCarddata);

            currentCardSlot.gameObject.SetActive(true);
        }
    }
}
