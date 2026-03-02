using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour
{
    public static SceneNav current;
    [field: SerializeField] public string GameSceneName { get; private set; } = "GameScene";
    [field: SerializeField] public string LeaderBoardSceneName { get; private set; } = "LeaderBoard";
    [field: SerializeField] public string MenuSceneName { get; private set; } = "Menu";
    [SerializeField] private TutorialManager tutorialManager;

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
        if(LevelCreator.current.Level <1 && LevelCreator.current.Level > 5)
        {
            return;
        }
        Debug.Log("to gamescene " + LevelCreator.current.Level);

        tutorialManager.LoadTutorial(LevelCreator.current.Level);
        //SceneManager.LoadScene(GameSceneName);
    }

    public void GoToMainMenuBtn()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene(MenuSceneName);
    }
}
