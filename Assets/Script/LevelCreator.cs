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
    public List<OperationEnum> PossibleOperators { get; private set; }

    [field: SerializeField]
    public int Level { get; private set; }

    [field: SerializeField]
    public Deck TemplateDeck { get; private set; }

    [field:Header("Shop Data"), SerializeField]
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
    
    public void SetLevel(int level)
    {
        this.Level = level;
    }

    public void ToGameSceneBtn()
    {
        SceneManager.LoadScene(GameSceneName); 
    }

    public void GameSceneReady()
    {
        OnGameStart?.Invoke(Level);
    }
}
