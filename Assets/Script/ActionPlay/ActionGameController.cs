using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using static Utils;

public class ActionGameController : MonoBehaviour
{
    public static ActionGameController current;

    [field: SerializeField]
    public Transform HeroSlotsTransform { get; private set; }

    [field: SerializeField]
    public Transform EnemySlotsTransform { get; private set; }

    [field: SerializeField]
    public GameObject PlusHero { get; private set; }

    [field: SerializeField]
    public GameObject MinusHero { get; private set; }

    [field: SerializeField]
    public GameObject MultiplyHero { get; private set; }

    [field: SerializeField]
    public GameObject DivideHero { get; private set; }

    [field: SerializeField]
    public GameObject BuffHero { get; private set; }


    private Hero attackerHero;

    private void Awake()
    {
        current = this;
    }

    private void OnDestroy()
    {
        if (current == this) current = null;
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
                    attackerHero = PlusHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Minus:
                    attackerHero = MinusHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Multiply:
                    attackerHero = MultiplyHero.transform.GetChild(0).GetComponent<Hero>();
                break;
                case OperationEnum.Divide:
                    attackerHero = DivideHero.transform.GetChild(0).GetComponent<Hero>();
                break;
            }

            Debug.Log(attackerHero.transform.name + " is attacking with Lcard:" + leftNumberCardEffectValue + " Rcard:" + rightNumberCardEffectValue + " multiplier:" + CardPlayGameController.current.Multiplier);

            attackerHero.Attack(CardPlayGameController.current.Multiplier, leftNumberCardEffectValue + rightNumberCardEffectValue);
        }
    }

    private bool IsEnemyRemain()
    {
        foreach(Transform enemySlot in EnemySlotsTransform)
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

        foreach (Transform enemyslot in EnemySlotsTransform)
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