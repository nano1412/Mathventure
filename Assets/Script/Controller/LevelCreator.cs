using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Utils;

public class LevelCreator : MonoBehaviour
{
    public static LevelCreator current;
    public event Action<int> OnGameStart;

    [SerializeField] private string GameSceneName = "GameScene";

    [field: Header("Data to game scene"), SerializeField]
    public Sprite BGSprite { get; private set; }
    public List<OperationEnum> PossibleOperators { get; private set; }

    [field: SerializeField]
    public int Level { get; private set; }

    [field: SerializeField]
    public bool IsEndless { get; private set; }

    [field: SerializeField]
    public Deck TemplateDeck { get; private set; }

    [field: Header("wave data"), SerializeField]
    public List<LevelData> LevelDatas { get; private set; }

    [field: SerializeField]
    public List<WaveData> EndlessWaveDatas { get; private set; }

    [field: Header("Shop Data"), SerializeField]
    public List<GameObject> SpawnableItems { get; private set; }

    private void Awake()
    {
        // Prevent duplicates
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameSceneReady(scene);
    }

    public void GameSceneReady(Scene scene)
    {
        if (scene.name == GameSceneName)   // <- Put your scene name here
        {
            OnGameStart?.Invoke(Level);
        } 
    }

    public void SetLevelCreator(Sprite bgSprite,int level, bool isEndless, List<GameObject> spawnableItemInShop, List<OperationEnum> possibleOperators)
    {
        BGSprite = bgSprite;
        Level = level;
        IsEndless = isEndless;
        SpawnableItems = spawnableItemInShop;
        PossibleOperators = possibleOperators;
    }
}
