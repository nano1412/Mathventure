using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Utils;

public class GameController : MonoBehaviour
{
    public static GameController current;
    public GameObject playedCardHandle;
    public GameObject cardInHand;
    public GameObject operatorInHand;
    public GameObject playedCardSlots;
    public GameObject playedOperatorSlots;
    public GameObject PlayStateText;
    [SerializeField] private GameObject card;
    [SerializeField] private Transform deck;

    public bool isHandReady = false;
    public bool isHandValiid = false;

    private void Awake()
    {
        current = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playedCardSlots = playedCardHandle.transform.Find("NumberCard").gameObject;
        playedOperatorSlots = playedCardHandle.transform.Find("OparetorCard").gameObject;
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

        foreach(Transform playedcard in playedCardSlots.transform)
        {
            //playedcard.GetChild(0);

            if (playedcard.childCount == 1 && playedcard.GetChild(0).GetComponent<Card>()!= null )
            {
                cardCount++;
            }
        }

        foreach (Transform operatorcard in playedOperatorSlots.transform)
        {
            //playedcard.GetChild(0);

            if (operatorcard.childCount == 1 && operatorcard.GetChild(0).GetComponent<Operatorcard>() != null)
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
            double min = -20;
            double max = 20;

            System.Random rand = new System.Random();
            double faceValue = rand.NextDouble() * (max - min) + min;
            rand = new System.Random();
            double effectValue = rand.NextDouble() * (max - min) + min;

            faceValue = RoundUpToDecimalPlaces(faceValue, 0);
            effectValue = RoundUpToDecimalPlaces(effectValue, 0);


            GameObject newCard = Instantiate(card, deck.position, new Quaternion(), currentCardSlot);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.deckPosition = deck.position;
            newCardScript.SetFaceValue(faceValue);
            newCardScript.SetEffectValue(effectValue);

            currentCardSlot.gameObject.SetActive(true);
        }
    }
}
