using System.Collections.Generic;
using UnityEngine;

public class Character_Plugin_RandomMove : MonoBehaviour
{
    [field:SerializeField] public List<Move> RandomMoves { get; private set; }

    public Move GetRandomMove()
    {
        Move rndMove = RandomMoves[Random.Range(0, RandomMoves.Count)];
        Debug.Log("get random move: " + rndMove.name);
        return rndMove;
    }
}
