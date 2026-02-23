using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    [SerializeField] private Ease scaleEase = Ease.OutBack;

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
    void Open(GameObject ui)
    {
        ui.SetActive(true);
        ui.transform.DOScale(1, 0.15f).SetEase(scaleEase);
    }

     async void Close(GameObject ui)
    {
        await ui.transform.DOScale(0, 0.15f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        ui.SetActive(false);
    }

    public void ToggleSettingMenu()
    {
        if (SettingCanvas.active == true)
        {
            Close(SettingCanvas);
        }
        else
        {
            Open(SettingCanvas);
        }
    }

    public void CloseSettingMenu()
    {
        SettingCanvas.SetActive(false);
    }
}
