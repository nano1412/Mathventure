using UnityEngine;
using static Utils;

public class ScreenController : MonoBehaviour
{
    [field: Header("GameScreen"), SerializeField] private GameObject WinScreen;
    [field: SerializeField] private GameObject LoseScreen;
    [field: SerializeField] private GameObject PauseScreen;

    public void OnEnable()
    {
        GameController.current.OnGameStateChange += FullScreenPopUp;
    }

    public void OnDisable()
    {
        GameController.current.OnGameStateChange -= FullScreenPopUp;
    }

    //public void TogglePauseSceene()
    //{
    //    PauseScreen.SetActive(!PauseScreen.active);
    //    PauseScreen.GetComponent<Animator>().SetBool("IsOpen", PauseScreen.active);
    //}

    //public void OpenPauseScreen()
    //{
    //    PauseScreen.SetActive(true);
    //}

    //public void ClosePauseScreen()
    //{
    //    PauseScreen.SetActive(false);
    //}

    public void FullScreenPopUp(GameState gameState)
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        //PauseScreen.SetActive(false);

        switch (gameState)
        {
            case GameState.Win:
                WinScreen.SetActive(true);
                break;
            case GameState.Lose:
                LoseScreen.SetActive(true);
                break;
        }
    }

}
