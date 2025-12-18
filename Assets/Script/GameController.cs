using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Utils;

public class GameController : MonoBehaviour
{
    public static GameController current;
    public event Action<GameState> OnGameStateChange;

    [field: SerializeField]
    public int MaxCardInHand { get; private set; }

    [field: SerializeField]
    public int Level { get; private set; }

    [field: SerializeField]
    public List<OperationEnum> PossibleOperators { get; private set; }

    [field: SerializeField]
    private GameState gameState;
    public GameState GameState { get => gameState; private set {
            gameState = value;
            OnGameStateChange?.Invoke(gameState);
        } }

    [field: SerializeField]
    public Deck TemplateDeck { get; private set; }

    [field: SerializeField]
    public int Wave { get; private set; }


    //charector that player pick

    private void Awake()
    {
        current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //temp fix
        PossibleOperators.Add(OperationEnum.Plus);
        PossibleOperators.Add(OperationEnum.Minus);
        PossibleOperators.Add(OperationEnum.Multiply);
        PossibleOperators.Add(OperationEnum.Divide);

        GameStart();
    }

    void GameStart()
    {
        CardPlayGameController.current.SetupCardContoller();
        CardPlayGameController.current.PlayerHand.GetComponent<HorizontalCardHolder>().SetUpPlayerSlot(NextRoundStart);
    }

    public void NextRoundStart()
    {
        Debug.LogWarning("imprement ememy data reader and reset deck here");


        this.SetGamestate(GameState.PlayerInput);
        CardPlayGameController.current.AddCard(MaxCardInHand);

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

    public void RounndWin()
    {
        //to round victory screen or check win
        if(Level < 4 && Wave >= 4)
        {
            this.SetGamestate(GameState.Win);
            //show sumary of this run
        } else
        {
            //this.SetGamestate(GameState.RoundVictory);
            Wave++;
            this.SetGamestate(GameState.Shop);
        }
    }

    public void Lose()
    {
        //attackerHero.cs gonna be the caller

        this.SetGamestate(GameState.Lose);
        //show lose screen
    }

    public void SetGamestate(GameState GS)
    {
        GameState = GS;
    }
}
