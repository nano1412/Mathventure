using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public abstract class Character : MonoBehaviour
{
    [field: Header("FX"), SerializeField] public GameObject HitFX { get; private set; }
    [field: SerializeField] public GameObject HealFX { get; private set; }

    [field:Header("SFX"), SerializeField] public AudioSource HealSFX { get; private set; }
    [field: SerializeField] public AudioSource HurtSFX { get; private set; }
    [field: SerializeField] public AudioSource DeadSFX { get; private set; }
    [field: SerializeField] public AudioSource AttackSFX { get; private set; }

    [field: Header("Character"), SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] protected Animator animator;
    [field: SerializeField] protected Slider HPbar;

    [field: Header("Hp and status"), SerializeField] public double BaseMaxHp { get; private set; }
    [field: SerializeField] public double EffectiveMaxHp { get; protected set; }
    [field: SerializeField] public double Hp { get; private set; }
    [field: SerializeField] public double Shield { get; private set; }
    [field: SerializeField] public bool IsStuned { get; private set; }

    [field: SerializeField] public List<CharacterBuff> CharacterBuffs { get; private set; }

    [field: Header("Attack data"), SerializeField] public Move CurrentMove { get; protected set; }
    [field: SerializeField] public Vector2 FacingDirection { get; private set; }
    [field: SerializeField] protected List<GameObject> targets = new();

    private void Awake()
    {
        EffectiveMaxHp = BaseMaxHp;
        Hp = EffectiveMaxHp;
    }

    private void Start()
    {
        CharacterBuffs = new List<CharacterBuff>();  
        BuffController.current.OnBuffTakeEffect += TakeHPBuffsEffect;
    }

    private void OnDestroy()
    {
        BuffController.current.OnBuffTakeEffect -= TakeHPBuffsEffect;
    }

    void Update()
    {
        HPbar.value = (float)Hp;
        HPbar.maxValue = (float)EffectiveMaxHp;
        CheckDead();
    }

    public void TakeDamage(double damage, string attacker)
    {
        if (damage < 0)
        {
            Debug.LogWarning("TakeDamage received a negative value. Use Heal() instead.");
            Heal(math.abs(damage), "Server");
            return;
        }

        if (HitFX != null)
        {
            GameObject fx = Instantiate(HitFX,transform);
            Destroy(fx,1);
        }

        animator.SetTrigger("Damaged");
        Hp -= damage - Shield;
        HurtSFX.Play();
        Debug.Log(transform.name + " take " + damage + " damages from " + attacker);
    }

    public void Heal(double heal, string healer)
    {
        if (heal < 0)
        {
            Debug.LogWarning("Heal received a negative value. Use TakeDamage() instead.");
            return;
        }

        if (HealFX != null)
        {
            GameObject fx = Instantiate(HealFX, transform);
            Destroy(fx, 1);
        }

        Hp += heal;
        if (Hp > EffectiveMaxHp)
        {
            Hp = EffectiveMaxHp;
        }
        HealSFX.Play();
        Debug.Log(transform.name + " gain " + heal + " Hp from " + healer);
    }

    public void BuffByWave()
    {
        if (!LevelCreator.current.IsEndless)
        {
            return;
        }

        BaseMaxHp *=  1 + (0.05f * GameController.current.Wave);
        Hp = BaseMaxHp;
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
        DeadSFX.Play();
        StartCoroutine(DestroyWhenFinishedPlayDeadSFX(DeadSFX));
        Destroy(gameObject);
    }

   

    IEnumerator DestroyWhenFinishedPlayDeadSFX(AudioSource audioSource)
{
    // Wait until it starts playing (optional safety)
    yield return new WaitUntil(() => audioSource.isPlaying);

    // Wait until it finishes
    yield return new WaitWhile(() => audioSource.isPlaying);

    Destroy(gameObject);
    BuffController.current.SelectedConsumable = null;
}

public virtual void ResolveAttack()
    {
        AttackSFX.Play();
        foreach (GameObject target in targets)
        {
            if (target != null && target.GetComponent<Character>())
            {
                if(CurrentMove.ApplyStatusViaAttack != null)
                {
                    target.GetComponent<Character>().AddCharacterBuffs(CurrentMove.ApplyStatusViaAttack);

                }
                target.GetComponent<Character>().TakeDamage(GetEffectiveAttackValue(), transform.name);
            }
        }
    }

    public virtual double GetEffectiveAttackValue()
    {
        double tempATK = CurrentMove.Value;
        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            switch (characterBuff.CharacterBuffTargetValue)
            {
                case CharacterBuffTargetValue.ATK:
                    tempATK += characterBuff.BuffMethodCalculation(tempATK);
                    break;
                default:
                    continue;
            }
        }

        return tempATK;
    }

    public void SendHeal()
    {
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<Character>())
            {
                if (CurrentMove.ApplyStatusViaAttack != null)
                {
                    target.GetComponent<Character>().AddCharacterBuffs(CurrentMove.ApplyStatusViaAttack);

                }
                target.GetComponent<Character>().Heal(CurrentMove.Value, transform.name);
            }
        }
    }



    protected List<GameObject> GetTargetByMove(Move move)
    {
        CharacterSlotsHolder ally;
        CharacterSlotsHolder opponent;

        Debug.Log(CharacterType + " " + move.TargetType);

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

        switch (move.TargetType)
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
        Hp = EffectiveMaxHp;
    }

    public void SelectedCharacter()
    {
        Debug.Log(this.name + " clicked");
        BuffController.current.SelectedCharacter = this.gameObject;
    }

    void TakeHPBuffsEffect(int duration)
    {
        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            Debug.Log(characterBuff.name);
            switch (characterBuff.CharacterBuffTargetValue)
            {
                case CharacterBuffTargetValue.HP:
                    Hp += characterBuff.BuffMethodCalculation(Hp);
                    break;
                default:
                    continue;
            }
        }
        UpdateEffectiveMaxHP();
        ReduceBuffsDuration(duration);
    }

    protected void UpdateEffectiveMaxHP()
    {
        double tempMaxHp = IncreaseMaxHPViaEquipment(BaseMaxHp);
        foreach (CharacterBuff characterBuff in CharacterBuffs)
        {
            switch (characterBuff.CharacterBuffTargetValue)
            {
                case CharacterBuffTargetValue.MaxHP:
                    tempMaxHp += characterBuff.BuffMethodCalculation(tempMaxHp);
                    break;
                default:
                    continue;
            }
        }

        EffectiveMaxHp = tempMaxHp;
    }

    protected virtual double IncreaseMaxHPViaEquipment(double tempMaxHp)
    {
        return tempMaxHp;
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
        foreach(CharacterBuff characterBuff in characterBuffs)
        {
            if (characterBuff.IsEffectInstant && characterBuff.CharacterBuffTargetValue == CharacterBuffTargetValue.HP)
            {
                Hp += characterBuff.BuffMethodCalculation(Hp);
            }
            else
            {
                CharacterBuffs.Add(characterBuff);
            }
        }
    }
}

