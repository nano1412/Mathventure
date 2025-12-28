using UnityEngine;
using static Utils;

public class CharacterSpriteRelay : MonoBehaviour
{
    [field: SerializeField] private Character character;
    public void HandleAnimationEvent(string evt)
    {
        if (evt == "Hit")
        {
            character.ResolveAttack();
        }
        else if (evt == "Heal")
        {
            character.SendHeal();
        }

        else if (evt == "Finish")
        {
            if (character.CharacterType == CharacterType.Enemy)
            {
                ActionGameController.current.OnEnemyAnimationFinished();
            }
            else
            {
                ActionGameController.current.OnHeroAnimationFinished();
            }
        }
    }
}
