using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour
{
    public static SceneNav current;
    [field: SerializeField] public string GameSceneName { get; private set; } = "GameScene";
    [field: SerializeField] public string LeaderBoardSceneName { get; private set; } = "LeaderBoard";
    [field: SerializeField] public string MenuSceneName { get; private set; } = "Menu";

    private void Awake()
    {
        current = this;
    }

    public void GoToLeaderBoardBtn()
    {
        Debug.Log("to leaderboard");
        SceneManager.LoadScene(LeaderBoardSceneName);
    }

    public void GoToGameSceneBtn()
    {
        Debug.Log("to gamescene");
        SceneManager.LoadScene(GameSceneName);
    }

    public void GoToMainMenuBtn()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene(MenuSceneName);
    }
}
