using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject buffHero;


    private Hero attackerHero;

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


        for (int i = 0; i < 3; i++)
        {
            OperationEnum heroType = CardPlayGameController.current.PlayCardList[operatorOrders[i]].GetChild(0).GetComponent<Operatorcard>().operation;

            double leftNumberCardEffectValue = CardPlayGameController.current.PlayCardList[operatorOrders[i] - 1].GetChild(0).GetComponent<Card>().GetEffectValue();
            double rightNumberCardEffectValue = CardPlayGameController.current.PlayCardList[operatorOrders[i] + 1].GetChild(0).GetComponent<Card>().GetEffectValue();

            switch (heroType)
            {
                case OperationEnum.Plus:
                    attackerHero = plusHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Minus:
                    attackerHero = minusHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Multiply:
                    attackerHero = multiplyHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Divide:
                    attackerHero = divideHero.transform.GetChild(0).GetComponent<Hero>();
                break;
            }

            Debug.Log(attackerHero.transform.name + " is attacking with Lcard:" + leftNumberCardEffectValue + " Rcard:" + rightNumberCardEffectValue + " multiplier:" + CardPlayGameController.current.Multiplier);

            attackerHero.Attack(CardPlayGameController.current.Multiplier, leftNumberCardEffectValue + rightNumberCardEffectValue);
        }
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