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
    [HideInInspector] public StageManager stageManager;

    public int indexCharacter;
    public float healthPoint;
    public float defense;
    public float attackDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;
    public CharacterType myCharacterType;
    public CharacterJsonRead characterJsonRead;
    private float shield;

    public CharacterState state;
    public TeamDivid teamDivid;
    [HideInInspector] public CharacterPersnality targetCharacter;

    [HideInInspector] public float attackCool;
    [HideInInspector] public float skillCool;
    private float ultimateCool;

    public Vector2 v2SpawnPoint;

    // �̵����ѿ�
    private float minX, maxX, minY, maxY;


    public abstract void Init();
    public abstract void CharacterAttack();
    public abstract IEnumerator CharacterUltimate();

    [HideInInspector] public List<CharacterPersnality> listTeamCharacters;
    private List<CharacterPersnality> listEnemyCharacters;
    

    public void BattleStart(List<CharacterPersnality> listTeam, List<CharacterPersnality> listEnemy)
    {
        listTeamCharacters = listTeam;
        listEnemyCharacters = listEnemy;
        // ��ų��Ÿ�ӵ� �߰��ʿ�
        UltimateCoolTime();
    }

    public void CharaterAction()
    {
        Ultimate();

        switch (state)
        {
            case CharacterState.idle:
                TargetSerch();
                break;

            case CharacterState.walk:
                Move();
                break;

            case CharacterState.attack:

                if (targetCharacter.state == CharacterState.dead)
                {
                    state = CharacterState.idle;
                    return;
                }

                Attack();
                break;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));
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
            CharacterAttack();

            AttackCoolTime();
        }
        else
        {
            state = CharacterState.walk;
        }
    }

    public async UniTaskVoid Ultimate()
    {
        if (ultimateCool >= 10f && targetCharacter != null && targetCharacter.state != CharacterState.dead && state != CharacterState.dead)
        {
            ultimateCool = 0;
            Debug.Log("�ʻ��");
            state = CharacterState.ultimate;

            await CharacterUltimate();

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

    public async UniTaskVoid UltimateCoolTime()
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
        healthPoint -= damage;

        if (healthPoint <= 0)
        {
            state = CharacterState.dead;
            //gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            ReviveCharater();
            stageManager.KillScoreRefresh(teamDivid);
        }
    }

    public void TargetSerch()
    {
        int targetIndex = -1;
        float distance = 1000f;

        for (int i = 0; i < listEnemyCharacters.Count; i++)
        {
            if (listEnemyCharacters[i].state == CharacterState.dead) continue;

            if (distance > Vector2.Distance(listEnemyCharacters[i].transform.position, transform.position))
            {
                targetIndex = i;
                distance = Vector2.Distance(listEnemyCharacters[i].transform.position, transform.position);
            }
        }

        if (targetIndex < 0) return;

        targetCharacter = listEnemyCharacters[targetIndex];

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

    public void SetLimitMoveStage(Vector2 v2MinPos, Vector2 v2MaxPos)
    {
        minX = v2MinPos.x * 0.9f;
        minY = v2MinPos.y * 0.9f;
        maxX = v2MaxPos.x * 0.9f;
        maxY = v2MaxPos.y * 0.9f;
    }
}