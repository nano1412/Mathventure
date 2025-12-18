using UnityEngine;
using static Utils;

public class PlayedCardHolder : MonoBehaviour
{
    private void Start()
    {
        GameController.current.OnGameStateChange += HandleGameStateChange;
        
    }

    private void HandleGameStateChange(GameState gameState)
    {
        if (gameState == GameState.HeroAttack ||
            gameState == GameState.EnemyAttack ||
            gameState == GameState.CardCalculation ||
            gameState == GameState.PlayerInput
            )

        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
