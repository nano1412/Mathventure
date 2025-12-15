using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class CardCalculationAnimation : MonoBehaviour
{
    public static CardCalculationAnimation current;

    private void Awake()
    {
        current = this;
    }

    public void PlayCalculationAnimation(double multiplier, List<int> operatorOrders, List<SimplifiedCard> simplifiedCardData)
    {
        GameController.current.SetGamestate(GameState.CardCalculation);

        for (int i = 0; i <= 2; i++)
        {
            Debug.Log("play something");
        }


    }
}
