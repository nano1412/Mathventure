using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController current;

    public int maxCardInHand = 8;

    //coin
    //charector that player pick
    //valid operator

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardPlayGameController.current.playerHand.GetComponent<HorizontalCardHolder>().SetUpPlayerSlot(RoundStart);
    }

    private void RoundStart()
    {
        CardPlayGameController.current.AddCard(maxCardInHand);

        CardPlayGameController.current.GetAllPossibleEquation();
        CardPlayGameController.current.GetTargetNumber();
    }

    public void PlayCardButton()
    {
        CardPlayGameController.current.PlayCard(PlayCalculationAnimation);

        //call for player attack here
        //call for enemy attack here

        //clean up card in playedcard
        foreach(Transform cardAlreadyPlayed in CardPlayGameController.current.playedCardSlots.transform)
        {
            Destroy(cardAlreadyPlayed.GetChild(0).gameObject);
        }

        RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayCalculationAnimation()
    {
        Debug.Log("play something");
    }
}
