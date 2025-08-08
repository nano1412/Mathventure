using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Utils;
using static PlayCardCalculation;

public class CardPlayGameController : MonoBehaviour
{
    public static CardPlayGameController current;
    public GameObject playedCardHandle;
    public GameObject cardInHand;
    public GameObject operatorButton;
    public GameObject playedCardSlots;
    public GameObject PlayStateText;
    [SerializeField] private GameObject card;
    [SerializeField] private Transform deckObject;
    public Deck templateDeck;
    [SerializeField] private Deck persistentDeck;
    [SerializeField] private Deck roundDeck;
    [SerializeField] private ParenthesesMode parenthesesMode;

    public bool isHandReady = false;
    public bool isHandValiid = false;

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


        List<object[]> steplog = PlayCardCalculation.EvaluateEquation(cardsPlayed, parenthesesMode);
        foreach (var step in steplog)
            Debug.Log($"{step[0]}, Pos: {step[1]}");
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
        foreach (Transform cardSlot in cardInHand.transform)
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
