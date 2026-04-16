using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.XR;


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
    [SerializeField] private GameObject UnderUI;

    [Header("GameScreen")]
    [SerializeField] private GameObject victoeyUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject pauseUI;

    [Header("uiTween")]
    [SerializeField] float openScale = 1f;
    [SerializeField] float closeScale = 0f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private bool scaleAnimations = true;


    public void OpenMainInterface()
    {
        sideBarUI.GetComponent<RectTransform>().DOAnchorPosX(-15, 0.3f).SetEase(Ease.OutBack);
        UnderUI.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
    }

    public void CloseMainInterface()
    {
        sideBarUI.GetComponent<RectTransform>().DOAnchorPosX(220, 0.3f).SetEase(Ease.OutBack);
        UnderUI.GetComponent<RectTransform>().DOAnchorPosY(-350, 0.3f).SetEase(Ease.OutBack);
    }

    public void Open(GameObject ui)
    {
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
                CloseMainInterface();
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
                OpenMainInterface();
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
