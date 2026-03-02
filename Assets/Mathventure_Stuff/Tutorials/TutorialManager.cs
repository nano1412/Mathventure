using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class tutorial
{
    public Texture pic;
    public VideoClip video;
    public string text;
}
[System.Serializable]
public class StageTutorial
{
    public tutorial[] page;
}
public class TutorialManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private RawImage _tutorialPic;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _tutorialUI;
    [SerializeField] private GameObject _stageSelectUI;
    [SerializeField] private MenuNavigation menuNavigation;

    [Header("Tutorials")]
    [SerializeField] private StageTutorial[] stageTutorial_Data;

    private int _currentStage = 0;
    private int _currentPage = 0;

    public void LoadTutorial (int stage)
    {
        _currentPage = 0;
        _currentStage = stage - 1;

        if (stageTutorial_Data.Length < stage)
        {
            SceneManager.LoadScene("GameScene");
            return;
        }

        menuNavigation.Close(_stageSelectUI);
        menuNavigation.Open(_tutorialUI);
        UpdatePage();
    }

    public void UpdatePage()
    {
        tutorial current = stageTutorial_Data[_currentStage].page[_currentPage];

        _description.text = current.text;
        _tutorialPic.texture = current.pic;

        if (current.video != null)
        {
            _videoPlayer.clip = current.video;
            _videoPlayer.time = 0f;
            _videoPlayer.Play();
        }
    }

    public void NextPage()
    {
        _currentPage += 1;
        if (_currentPage >= stageTutorial_Data[_currentStage].page.Length)
        {
            SceneManager.LoadScene("GameScene");
            return;
        }
        UpdatePage();
    }

    public void PrevPage()
    {
        _currentPage -= 1;
        if (_currentPage < 0)
        {
            _currentPage = 0;
        }
        UpdatePage();
    }
}
