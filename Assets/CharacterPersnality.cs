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

    public Vector2 v2SpawnPoint;


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
                    Debug.Log("���� => �̵�");
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
                // ��ų�� ���� ���ǹ� �߰��ʿ�
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
        if (ultimateCool >= 10f && targetCharacter.state != CharacterState.dead && state != CharacterState.dead)
        {
            ultimateCool = 0;
            Debug.Log("�ʻ��");
            state = CharacterState.ultimate;
            await UniTask.Delay(1000);
            Debug.Log("�ʻ�� => �⺻");
            state = CharacterState.idle;
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

    public void Hit(float damage)
    {
        Debug.Log("����");
        healthPoint -= damage;

        if (healthPoint <= 0)
        {
            state = CharacterState.dead;
            //gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            ReviveCharater();
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

    public async UniTaskVoid ReviveCharater()
    {
        await UniTask.Delay(5000);

        gameObject.SetActive(true);

        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(v2SpawnPoint.x + Random.Range(-75.0f, 75.1f), v2SpawnPoint.y + Random.Range(-150.0f, 150.1f)));

        transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);

        //-----------------ü�µ� �ʱ�ȭ ��ũ��Ʈ �ۼ��ʿ�-------------------
        Init();
        state = CharacterState.idle;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("ĳ���� ����");
    }
}