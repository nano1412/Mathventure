using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class ActionGameController : MonoBehaviour
{
    public static ActionGameController current;
    public GameObject charecterSlots;
    public GameObject enemySlots;

    private void Awake()
    {
        current = this;
    }

    public void CharecterAttack()
    {
        GameController.current.SetGamestate(GameState.CharecterAttack);
    }

    public void EmenyAttack()
    {
        GameController.current.SetGamestate(GameState.EnemyAttack);
    }

    private void Attack(GameObject Attacker, List<GameObject> Targets)
    {

    }
}
