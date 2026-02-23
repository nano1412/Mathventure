using UnityEngine;
using DG.Tweening;


public class MainGameUIController : MonoBehaviour
{
    [Header("MainUI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject equipUI;
    [SerializeField] private GameObject sideBarUI;
    [SerializeField] private GameObject playcardUI;
    [SerializeField] private GameObject gameDataDisplay;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject heroEquipment;


    [Header("GameScreen")]
    [SerializeField] private GameObject victoeyUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject pauseUI;

    [Header("uiTween")]
    [SerializeField] float openScale = 1f;
    [SerializeField] float closeScale = 0f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private bool scaleAnimations = true;

    public void Open(GameObject ui)
    {
        Debug.Log(openScale);
        ui.SetActive(true);
        if (scaleAnimations)
            ui.transform.DOScale(openScale, 0.15f).SetEase(scaleEase);
    }

    public async void Close(GameObject ui)
    {
        if (scaleAnimations)
            await ui.transform.DOScale(closeScale, 0.15f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
            ui.SetActive(false);
    }
    public void OpenUI(string ui)
    {
        Debug.Log(ui);
        switch (ui)
        {
            case "shop":
                Open(shopUI);
                break;
            case "equip":
                Open(equipUI);
                break;
            case "win":
                Open(victoeyUI);
                break;
            case "lose":
                Open(loseUI);
                break;
            case "pause":
                Open(pauseUI);
                break;
            case "sidebar":
                Open(sideBarUI);
                break;
            case "playcardUI":
                Open(playcardUI);
                break;
            case "gameData":
                Open(gameDataDisplay);
                break;
            case "inventory":
                Open(inventory);
                break;
            case "heroEquipment":
                Open(heroEquipment);
                break;
        }
    }
    public async void CloseUI(string ui)
    {
        Debug.Log(ui);
        switch (ui)
        {
            case "shop":
                Close(shopUI);
                break;
            case "equip":
                Close(equipUI);
                break;
            case "win":
                Close(victoeyUI);
                break;
            case "lose":
                Close(loseUI);
                break;
            case "pause":
                Close(pauseUI);
                break;
            case "sidebar":
                Close(sideBarUI);
                break;
            case "playcardUI":
                Close(playcardUI);
                break;
            case "gameData":
                Close(gameDataDisplay);
                break;
            case "inventory":
                Close(inventory);
                break;
            case "heroEquipment":
                Close(heroEquipment);
                break;
        }
    }
}
