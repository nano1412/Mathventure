using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public abstract class Character : MonoBehaviour
{
    [field: Header("Character"), SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] protected Animator animator;
    [field: SerializeField] protected Slider HPbar;

    [field: Header("Hp and status"), SerializeField] public double BaseMaxHp { get; private set; }
    [field: SerializeField] public double Hp { get; private set; }
    [field: SerializeField] public double Shield { get; private set; }
    [field: SerializeField] public bool IsStuned { get; private set; }

    [field: SerializeField] public List<CharacterBuff> CharacterBuffs { get; private set; } = new List<CharacterBuff>();

    [field: Header("Attack data"), SerializeField] public Move DefaultMove { get; private set; }
    [field: SerializeField] public Vector2 FacingDirection { get; private set; }
    [field: SerializeField] protected List<GameObject> targets = new();

    private void Start()
    {
        Hp = BaseMaxHp;
        BuffController.current.OnBuffTakeEffect += TakeBuffsEffect;
    }

    private void OnDestroy()
    {
        BuffController.current.OnBuffTakeEffect -= TakeBuffsEffect;
    }

    void Update()
    {
        HPbar.value = (float)Hp;
        HPbar.maxValue = (float)BaseMaxHp;
    }

    public void TakeDamage(double damage, string attacker)
    {
        if (damage < 0)
        {
            Debug.LogWarning("TakeDamage received a negative value. Use Heal() instead.");
            return;
        }

        animator.SetTrigger("Damaged");
        Hp -= damage - Shield;
        Debug.Log(transform.name + " take " + damage + " damages from " + attacker);
        CheckDead();
    }

    public void Heal(double heal, string healer)
    {
        if (heal < 0)
        {
            Debug.LogWarning("Heal received a negative value. Use TakeDamage() instead.");
            return;
        }

        Hp += heal;
        Debug.Log(transform.name + " gain " + heal + " Hp from " + healer);
    }

    public virtual void CheckDead()
    {
        if (Hp <= 0)
        {
            Debug.Log(transform.name + "is dead");
            Dead();
        }
    }

    protected void Dead()
    {
        if(CharacterType == CharacterType.Enemy)
        {
            ActionGameController.current.EnemySlotsHolder.characters.Remove(gameObject);
            ActionGameController.current.EnemySlotsHolder.UpdateCharacters();
        }
        Destroy(gameObject);
    }

    public virtual void ResolveAttack()
    {
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<Character>())
            {
                target.GetComponent<Character>().TakeDamage(DefaultMove.Value, transform.name);
            }
        }
    }

    public void SendHeal()
    {
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<Character>())
            {
                target.GetComponent<Character>().Heal(DefaultMove.Value, transform.name);
            }
        }
    }



    protected List<GameObject> GetTargetByMove()
    {
        CharacterSlotsHolder ally;
        CharacterSlotsHolder opponent;

        if (CharacterType == CharacterType.Enemy)
        {
            ally = ActionGameController.current.EnemySlotsHolder;
            opponent = ActionGameController.current.HeroSlotsHolder;
        }
        else
        {
            ally = ActionGameController.current.HeroSlotsHolder;
            opponent = ActionGameController.current.EnemySlotsHolder;
        }

        switch (DefaultMove.TargetType)
        {
            case AttackTargetType.front:
                return opponent.GetFirstCharacterAsList();

            case AttackTargetType.firstTwo:
                return opponent.GetFirstTwoCharactersAsList();

            case AttackTargetType.back:
                return opponent.GetLastCharacterAsList();

            case AttackTargetType.all:
                return opponent.GetAllCharactersAsList();

            case AttackTargetType.allFriendly:
                return ally.GetAllCharactersAsList();

        }

        return new List<GameObject>();
    }

    public void ResetHP()
    {
        Hp = BaseMaxHp;
    }

    public void SelectedCharacter()
    {
        Debug.Log(this.name + " clicked");
        BuffController.current.SelectedCharacter = this.gameObject;
    }

    void TakeBuffsEffect(int duration)
    {
        Debug.Log(gameObject.name + " take buff!");
        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            //take effect
        }

        ReduceBuffsDuration(duration);
    }

    public void ReduceBuffsDuration(int duration)
    {
        if (duration == -1)
        {
            CharacterBuffs.Clear();
            return;
        }

        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            characterBuff.ReduceDuration(duration);
        }
        CharacterBuffs.RemoveAll(characterBuff => characterBuff.Duration <= 0);
    }

    public void AddCharacterBuffs(List<CharacterBuff> characterBuffs)
    {
        CharacterBuffs.AddRange(characterBuffs);
    }
}

