using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GameController : MonoBehaviour
{
    public static GameController current;

    public int maxCardInHand = 8;
    public int coin = 0;
    public int wave = 1;

    public GameState gamestate = GameState.Menu;

    //charector that player pick
    //valid operator

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardPlayGameController.current.SetupCardContoller();
        CardPlayGameController.current.playerHand.GetComponent<HorizontalCardHolder>().SetUpPlayerSlot(NextRoundStart);
    }

    private void NextRoundStart()
    {
        this.SetGamestate(GameState.PlayerInput);
        CardPlayGameController.current.AddCard(maxCardInHand);

        CardPlayGameController.current.GetAllPossibleEquation();
        CardPlayGameController.current.GetTargetNumber();
    }

    public void PlayCardButton()
    {
        CardPlayGameController.current.PlayCard(OnPlayClassSucess); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayClassSucess()
    {
        if (CardPlayGameController.current.OperatorOrders.Count != 3 || CardPlayGameController.current.OperatorOrders == null)
        {
            Debug.Log("Player haven't answer yet");
            return;
        }

        double multiplier = CardPlayGameController.current.Multiplier;
        List<int> operatorOrders = CardPlayGameController.current.OperatorOrders;
        List<SimplifiedCard> simplifiedCardData = PlayCardCalculation.simplified;

        PlayCalculationAnimation(multiplier, operatorOrders, simplifiedCardData);

        ActionGameController.current.CharecterAttack();


        foreach (Transform cardAlreadyPlayed in CardPlayGameController.current.playedCardSlots.transform)
        {
            Destroy(cardAlreadyPlayed.GetChild(0).gameObject);
        }

        NextRoundStart();
    }

    void PlayCalculationAnimation(double multiplier, List<int> operatorOrders, List<SimplifiedCard> simplifiedCardData)
    {
        this.SetGamestate(GameState.CardCalculation);

        for (int i = 0; i <= 2; i++)
        {
            Debug.Log("play something");
        }


    }

    public GameState GetGamestate()
    {
        return gamestate;
    }

    public void SetGamestate(GameState GS)
    {
        gamestate = GS;
    }
}
