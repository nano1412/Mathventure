using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GameController : MonoBehaviour
{
    public static GameController current;

    public int maxCardInHand = 8;
    public int level = 1;
    public List<OperationEnum> posibleOperators = new List<OperationEnum>();
    public GameState gamestate = GameState.Menu;
    public Deck templateDeck;

    public int coin = 0;
    public int wave = 1;


    //charector that player pick

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //temp fix
        posibleOperators.Add(OperationEnum.Plus);
        posibleOperators.Add(OperationEnum.Minus);
        posibleOperators.Add(OperationEnum.Multiply);
        posibleOperators.Add(OperationEnum.Divide);

        GameStart();
    }

    void GameStart()
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
        CardPlayGameController.current.SummitEquation(OnEquationSummitionSucess); 
    }

    void OnEquationSummitionSucess()
    {
        ActionGameController.current.AllCharecterAttack(CleanupFornextRound,RounndWin);

        
    }

    void CleanupFornextRound()
    {
        foreach (Transform cardAlreadyPlayed in CardPlayGameController.current.PlayCardList)
        {
            Destroy(cardAlreadyPlayed.GetChild(0).gameObject);
        }

        NextRoundStart();
    }

    void RounndWin()
    {
        //to round victory screen or check win
        if(level < 4 && wave >= 4)
        {
            this.SetGamestate(GameState.Win);
            //show sumary of this run
        } else
        {
            this.SetGamestate(GameState.RoundVictory);
        }
    }

    public void Lose()
    {
        //hero.cs gonna be the caller

        this.SetGamestate(GameState.Lose);
        //show lose screen
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
