using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum TeamDivid
{
    myTeam,
    enemyTeam
}

//[CreateAssetMenu(fileName = "CharacterPersnality" ,menuName = "scriptable Object/CharacterPersnality")]

public enum CharacterState
{
    idle,
    walk,
    attack,
    skill,
    ultimate,
    hit,
    dead
}

public abstract class CharacterPersnality : MonoBehaviour
{
    public int indexCharacter;
    public float healthPoint;
    public float defense;
    public float attackDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;
    public CharacterType myCharacterType;
    public List<CharacterPersnality> listEnemy;
    public List<float> listEnemyDistance;
    public CharacterJsonRead characterJsonRead;

    public CharacterState state;
    public TeamDivid teamDivid;
    public CharacterPersnality targetCharacter;

    [HideInInspector] public float attackCool;
    [HideInInspector] public float skillCool;
    private float ultimateCool;


    public abstract void Init();

    public void BattleStart()
    {
        UltimateCoolTime();
    }

    public void CharaterAction(List<CharacterPersnality> characterPersnalities)
    {
        Ultimate();

        switch (state)
        {
            case CharacterState.idle:
                TargetSerch(characterPersnalities);
                break;

            case CharacterState.walk:
                Move();
                break;

            case CharacterState.attack:

                if (targetCharacter.state == CharacterState.dead)
                {
                    Debug.Log("공격 => 이동");
                    state = CharacterState.idle;
                    return;
                }

                Attack();
                break;
        }
    }

    public void Move()
    {
        if (targetCharacter.state == CharacterState.dead) state = CharacterState.idle;
        else
        {
            if (Vector2.Distance(transform.position, targetCharacter.transform.position) > attackRange * 0.5f)
                transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, moveSpeed * Time.deltaTime);
            else
            {
                // 스킬과 공격 조건문 추가필요
                state = CharacterState.attack;
            }
        }
    }

    public void Attack()
    {
        if(attackCool >= attackSpeed)
        {
            targetCharacter.Hit(attackDamage);
            AttackCoolTime();
        }
        else
        {
            state = CharacterState.walk;
        }
    }

    public async UniTaskVoid Ultimate()
    {
        if (ultimateCool >= 10f)
        {
            ultimateCool = 0;
            Debug.Log("필살기");
            state = CharacterState.ultimate;
            await UniTask.Delay(3000);
            Debug.Log("필살기 => 기본");
            state = CharacterState.idle;
        }
    }

    public void Hit(float damage)
    {
        Debug.Log("공격");
        healthPoint -= damage;

        if (healthPoint <= 0)
        {
            state = CharacterState.dead;
            Debug.Log("사망");
        }
    }

    private async UniTaskVoid AttackCoolTime()
    {
        attackCool = 0f;
        while (attackSpeed > attackCool)
        {
            attackCool += Time.deltaTime;

            if (attackSpeed <= attackCool) break;

            await UniTask.Yield();
        }
    }

    private async UniTaskVoid UltimateCoolTime()
    {
        ultimateCool = 0f;
        while (10f > ultimateCool)
        {
            ultimateCool += Time.deltaTime;

            if (10f <= ultimateCool) break;

            await UniTask.Yield();
        }
    }

    public void TargetSerch(List<CharacterPersnality> listCharactors)
    {
        int targetIndex = -1;
        float distance = 1000f;

        for (int i = 0; i < listCharactors.Count; i++)
        {
            if (listCharactors[i].state == CharacterState.dead) continue;

            if (distance > Vector2.Distance(listCharactors[i].transform.position, transform.position))
            {
                targetIndex = i;
                distance = Vector2.Distance(listCharactors[i].transform.position, transform.position);
            }
        }

        if (targetIndex < 0) return;

        targetCharacter = listCharactors[targetIndex];

        state = CharacterState.walk;
    }
}