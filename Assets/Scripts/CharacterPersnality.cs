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
    idle = 0,
    walk = 1,
    attack = 2,
    skill = 3,
    ultimate = 4,
    hit = 5,
    dead = 6
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
    [HideInInspector] public float maxSkillCool = 7f;
    public CharacterType myCharacterType;
    public CharacterJsonRead characterJsonRead;
    private float shield;
    public bool isDead = false;

    public CharacterState state;
    public TeamDivid teamDivid;
    [HideInInspector] public CharacterPersnality targetCharacter;

    [HideInInspector] public float attackCool;
    private float skillCool;
    private float ultimateCool;

    public Vector2 v2SpawnPoint;

    [HideInInspector] public List<CharacterPersnality> listTeamCharacters;
    private List<CharacterPersnality> listEnemyCharacters;
    [HideInInspector] public Animator animator;
    private int parameterCount;

    // 이동제한용
    private float minX, maxX, minY, maxY;


    public abstract void Init();
    public abstract void CharacterAttack();
    public abstract IEnumerator CharacterUltimate();
    public abstract IEnumerator CharacterSkill();


    public void BattleStart(List<CharacterPersnality> listTeam, List<CharacterPersnality> listEnemy)
    {
        listTeamCharacters = listTeam;
        listEnemyCharacters = listEnemy;
        parameterCount = animator.parameterCount;
        // 스킬쿨타임도 추가필요
        UltimateCoolTime();
        AttackCoolTime();
        SkillCoolTime();
    }

    public void CharaterAction()
    {
        if (isDead) return;

        switch (state)
        {
            case CharacterState.idle:
                if (targetCharacter == null || targetCharacter.isDead)
                {
                    TargetSerch();
                }
                else if (!targetCharacter.isDead)
                {
                    if (ultimateCool >= 10f)
                    {
                        Ultimate();
                    }
                    else if (skillCool >= maxSkillCool) ChangeState((int)CharacterState.skill);
                    else if (attackCool >= attackSpeed) ChangeState((int)CharacterState.attack);
                }
                break;

            case CharacterState.walk:
                Move();
                break;

            case CharacterState.attack:

                if (targetCharacter.isDead)
                {
                    ChangeState(((int)CharacterState.idle));
                    return;
                }

                if (attackCool >= attackSpeed)
                    AttackCoolTime();

                break;
            case CharacterState.skill:

                if(skillCool >= maxSkillCool)
                    SkillCoolTime();
                break;
        }

        //transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));
    }

    public void Move()
    {
        if (targetCharacter.isDead) ChangeState(((int)CharacterState.idle));
        else
        {
            if (Vector2.Distance(transform.position, targetCharacter.transform.position) > attackRange * 0.5f)
                transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, moveSpeed * Time.deltaTime);
            else
            {
                // 스킬과 공격 조건문 추가필요
                ChangeState(((int)CharacterState.attack));
            }
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

    private async UniTaskVoid SkillCoolTime()
    {
        skillCool = 0f;
        while (maxSkillCool > skillCool)
        {
            skillCool += Time.deltaTime;

            if (maxSkillCool <= skillCool) break;

            await UniTask.Yield();
        }
    }

    public async UniTaskVoid Ultimate()
    {
        if (ultimateCool >= 10f && !targetCharacter.isDead && !isDead)
        {
            ultimateCool = 0;
            Debug.Log("필살기");
            ChangeState(((int)CharacterState.ultimate));

            await CharacterUltimate();
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
        healthPoint -= damage - defense;

        if (healthPoint <= 0)
        {
            isDead = true;
            ChangeState(((int)CharacterState.dead));
            //gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            ReviveCharater();
            stageManager.KillScoreRefresh(teamDivid);
        }
        else if(state == CharacterState.hit)
        {
            // 넉백추가
        }
    }

    public void TargetSerch()
    {
        int targetIndex = -1;
        float distance = 1000f;

        for (int i = 0; i < listEnemyCharacters.Count; i++)
        {
            if (listEnemyCharacters[i].isDead) continue;

            if (distance > Vector2.Distance(listEnemyCharacters[i].transform.position, transform.position))
            {
                targetIndex = i;
                distance = Vector2.Distance(listEnemyCharacters[i].transform.position, transform.position);
            }
        }

        if (targetIndex < 0) return;

        targetCharacter = listEnemyCharacters[targetIndex];

        if (state == CharacterState.ultimate) return;

        if (Vector2.Distance(transform.position, targetCharacter.transform.position) > attackRange * 0.5f)
            ChangeState(((int)CharacterState.walk));
        else if(attackCool >= attackSpeed)
            ChangeState(((int)CharacterState.attack));


        //ChangeStage(((int)CharacterState.walk));
    }

    public async UniTaskVoid ReviveCharater()
    {
        await UniTask.Delay(5000);

        gameObject.SetActive(true);

        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(v2SpawnPoint.x + Random.Range(-75.0f, 75.1f), v2SpawnPoint.y + Random.Range(-150.0f, 150.1f)));

        transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);

        //-----------------체력등 초기화 스크립트 작성필요-------------------
        Init();
        ChangeState((int)CharacterState.idle);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("캐릭터 리젠");
    }

    public void SetLimitMoveStage(Vector2 v2MinPos, Vector2 v2MaxPos)
    {
        minX = v2MinPos.x * 0.9f;
        minY = v2MinPos.y * 0.9f;
        maxX = v2MaxPos.x * 0.9f;
        maxY = v2MaxPos.y * 0.9f;
    }

    public void ChangeState(int stateNum)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Skill", false);
        animator.SetBool("Ultimate", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Dead", false);

        state = (CharacterState)stateNum;

        switch (state)
        {
            case CharacterState.idle:
                animator.SetBool("Idle", true);
                break;
            case CharacterState.walk:
                animator.SetBool("Move", true);
                break;
            case CharacterState.attack:
                animator.SetBool("Attack", true);
                break;
            case CharacterState.skill:
                animator.SetBool("Skill", true);
                break;
            case CharacterState.ultimate:
                animator.SetBool("Ultimate", true);
                break;
            case CharacterState.hit:
                animator.SetBool("Hit", true);
                break;
            case CharacterState.dead:
                animator.SetBool("Dead", true);
                break;
        }
    }
}