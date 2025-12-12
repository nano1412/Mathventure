using System;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class ActionGameController : MonoBehaviour
{
    public static ActionGameController current;
    public Transform HeroSlots;
    public Transform enemySlots;

    public GameObject plusHero;
    public GameObject minusHero;
    public GameObject multiplyHero;
    public GameObject divideHero;

    private void Awake()
    {
        current = this;
    }

    public void AllCharecterAttack(Action onCompleteAttack,Action onWin)
    {
        HeroAttack();

        if (IsEnemyRemain())
        {
            EmenyAttack(GetEnemyAttackQueue());

            onCompleteAttack?.Invoke();
        } 
        else
        {
            onWin?.Invoke();
        }
    }

    private void HeroAttack()
    {
        GameController.current.SetGamestate(GameState.HeroAttack);

        double multiplier = CardPlayGameController.current.Multiplier;
        List<int> operatorOrders = CardPlayGameController.current.OperatorOrders;
        List<SimplifiedCard> simplifiedCardData = PlayCardCalculation.simplified;

        List<Hero> heroQueue = new List<Hero>();

        for (int i = 0; i < 3; i++)
        {
            switch (CardPlayGameController.current.PlayCardList[operatorOrders[i]].GetChild(0).GetComponent<Operatorcard>().operation)
            {
                case OperationEnum.Plus:
                    heroQueue.Add(plusHero.transform.GetChild(0).GetComponent<Hero>());
                    break;
                case OperationEnum.Minus:
                    heroQueue.Add(minusHero.transform.GetChild(0).GetComponent<Hero>());
                    break;
                case OperationEnum.Multiply:
                    heroQueue.Add(multiplyHero.transform.GetChild(0).GetComponent<Hero>());
                    break;
                case OperationEnum.Divide:
                    heroQueue.Add(divideHero.transform.GetChild(0).GetComponent<Hero>());
                    break;
            }
        }

        Debug.Log("turn order are");
        Debug.Log(heroQueue[0].transform.name);
        Debug.Log(heroQueue[1].transform.name);
        Debug.Log(heroQueue[2].transform.name);

    }

    private bool IsEnemyRemain()
    {
        foreach(Transform enemySlot in enemySlots)
        {
            if(enemySlot.childCount >= 1)
            {
                return true;
            }
        }

        return false;
    }

    private List<Enemy> GetEnemyAttackQueue()
    {
        List<Enemy> enemyQueue = new List<Enemy>();

        foreach (Transform enemyslot in enemySlots)
        {
            if (enemyslot.childCount != 0)
            {
                enemyQueue.Add(enemyslot.GetChild(0).GetComponent<Enemy>());
            }
        }

        return enemyQueue;
    }

    private void EmenyAttack(List<Enemy> enemyQueue)
    {
        GameController.current.SetGamestate(GameState.EnemyAttack);

        foreach(Enemy enemy in enemyQueue)
        {
            enemy.Attack();
        }
    }
}