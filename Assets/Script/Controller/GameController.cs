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
    private bool startWithoutLevelCretor;

    [field: SerializeField]
    public int MaxCardInHand { get; private set; } = 6;

    [field: SerializeField]
    public bool IsEndless { get; private set; }

    [field: SerializeField]
    public int Level { get; private set; }

    [field: SerializeField]
    public int Wave { get; private set; }

    [field: Header("wave data"), SerializeField]
    public List<LevelData> LevelDatas { get; private set; }

    [field: SerializeField]
    public List<WaveData> EndlessWaveDatas { get; private set; }


    [field: SerializeField]
    public List<OperationEnum> PossibleOperators { get; private set; }

    [field: SerializeField]
    private GameState gameState;
    public GameState GameState
    {
        get => gameState; private set
        {
            gameState = value;
            OnGameStateChange?.Invoke(gameState);
        }
    }

    [field: SerializeField]
    public Deck TemplateDeck { get; private set; }

   

    private void Awake()
    {
        current = this;

    }

    private void OnEnable()
    {
        LevelCreator.current.OnGameStart += GameSetup;
    }

    private void OnDisable()
    {
        LevelCreator.current.OnGameStart -= GameSetup;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
    }

    private void GameSetup(int i)
    {
        GameState = GameState.PlayerInput;
        PossibleOperators = LevelCreator.current.PossibleOperators;
        TemplateDeck = LevelCreator.current.TemplateDeck;
        Level = LevelCreator.current.Level;

        IsEndless = LevelCreator.current.IsEndless;
        LevelDatas = LevelCreator.current.LevelDatas;
        EndlessWaveDatas = LevelCreator.current.EndlessWaveDatas;
        HeroToggler.current.SetupEnableHeroViaPossibleOperator(PossibleOperators);

        GameStart();
    }


    void Start()
    {
        if (startWithoutLevelCretor)
        {
            StartWithOutLevelCretor();
        }
    }

    private void StartWithOutLevelCretor()
    {

        PossibleOperators.Add(OperationEnum.Plus);
        PossibleOperators.Add(OperationEnum.Minus);
        PossibleOperators.Add(OperationEnum.Multiply);
        PossibleOperators.Add(OperationEnum.Divide);

        GameStart();
    }

    void GameStart()
    {
        CardPlayGameController.current.SetupCardContoller();
        CardPlayGameController.current.PlayerHand.GetComponent<HorizontalCardHolder>().SetUpPlayerSlot(OnNextWaveStart);
    }

    public void OnNextWaveStart()
    {
        ActionGameController.current.SpawnNextWave();
        ActionGameController.current.ResetHerosHP();
        CardPlayGameController.current.ResetRoundDeck();

        NextRoundStart();
    }

    public void NextRoundStart()
    {


        this.SetGamestate(GameState.PlayerInput);
        CardPlayGameController.current.AddCardTohand(MaxCardInHand);

        CardPlayGameController.current.GetAllPossibleEquation();
        CardPlayGameController.current.GetTargetNumber();
    }

    public void PlayCardButton()
    {
        CardPlayGameController.current.SummitEquation(OnEquationSummitionSucess);
    }

    void OnEquationSummitionSucess()
    {
        ActionGameController.current.AllCharecterAttack(CleanupFornextRound, RounndWin);


    }

    public void CleanupFornextRound()
    {
        if(gameState == GameState.Lose)
        {
            return;
        }
        foreach (GameObject cardAlreadyPlayed in CardPlayGameController.current.PlayCardList)
        {
            Destroy(cardAlreadyPlayed);
        }
        BuffController.current.AllCharactersTakeBuffsEffect();

        NextRoundStart();
    }

    public void RounndWin()
    {
        BuffController.current.RemoveAllBuff();

        //to round victory screen or check win
        if (!IsEndless && Wave >= 3)
        {
            this.SetGamestate(GameState.Win);
            //show sumary of this run
        }
        else
        {
            //this.SetGamestate(GameState.RoundVictory);
            Wave++;
            this.SetGamestate(GameState.Shop);
        }
    }

    public void Lose()
    {
        //attacker Hero.cs gonna be the caller

        this.SetGamestate(GameState.Lose);
        //show lose screen
    }

    public void SetGamestate(GameState GS)
    {
        GameState = GS;
    }


}
