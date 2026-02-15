using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public static MenuNavigation current;
    [field:SerializeField] public GameObject LevelSelecterCanvas { get; private set; }
    [field: SerializeField] public GameObject SettingCanvas { get; private set; }

    [field: Header("Level"), SerializeField] public int MaxLevelUnlock { get; private set; }
    [field: SerializeField] public LevelSelectButton Level1Btn { get; private set; }
    [field: SerializeField] public LevelSelectButton Level2Btn { get; private set; }
    [field: SerializeField] public LevelSelectButton Level3Btn { get; private set; }
    [field: SerializeField] public LevelSelectButton Level4Btn { get; private set; }
    [field: SerializeField] public LevelSelectButton LevelEndlessBtn { get; private set; }

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        LevelSelecterCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
    }

    public void OpenLevelSelecterMenu()
    {
        LevelSelecterCanvas.SetActive(true);
    }

    public void CloseLevelSelecterMenu()
    {
        LevelSelecterCanvas.SetActive(false);
    }

    public void OpenSettingMenu()
    {
        Debug.Log("open setting");
        SettingCanvas.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        Debug.Log("close setting");
        SettingCanvas.SetActive(false);
    }
}
