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
    [HideInInspector] public float maxSkillCool = 10f; //스킬쿨 임시추가
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
    [HideInInspector] public List<CharacterPersnality> listEnemyCharacters;
    [HideInInspector] public Animator animator;

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
        // 스킬쿨타임도 추가필요
        UltimateCoolTime();
        AttackCoolTime();
        SkillCoolTime();
    }

    public void CharaterAction()
    {
        if (teamDivid == TeamDivid.enemyTeam) return;
        if (isDead || state == CharacterState.hit) return;

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
                    else if (attackCool >= attackSpeed && isCanAttackRange()) ChangeState((int)CharacterState.attack);
                    else if (!isCanAttackRange()) ChangeState((int)CharacterState.walk);
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
            if (!isCanAttackRange())
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
    }

    public async UniTaskVoid KnockBack(Vector3 v3AttackPos)
    {
        ChangeState((int)CharacterState.hit);
        Vector3 v3MovePoint = v3AttackPos - transform.position;
        v3MovePoint = v3MovePoint.normalized;

        while (state == CharacterState.hit && !isDead)
        {
            transform.position -= v3MovePoint * 0.5f * Time.deltaTime;
            await UniTask.Yield();
        }
    }

    //공격가능범위체크
    private bool isCanAttackRange()
    {
        if (Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange * 0.5f)
            return true;

        return false;
    }

    //적 탐색
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

    //부활
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

    //이동제한
    public void SetLimitMoveStage(Vector2 v2MinPos, Vector2 v2MaxPos)
    {
        minX = v2MinPos.x * 0.9f;
        minY = v2MinPos.y * 0.9f;
        maxX = v2MaxPos.x * 0.9f;
        maxY = v2MaxPos.y * 0.9f;
    }

    //캐릭터 상태변환
    public void ChangeState(int stateNum)
    {
        switch (state)
        {
            case CharacterState.idle:
                animator.SetBool("Idle", false);
                break;
            case CharacterState.walk:
                animator.SetBool("Move", false);
                break;
            case CharacterState.attack:
                animator.SetBool("Attack", false);
                break;
            case CharacterState.skill:
                animator.SetBool("Skill", false);
                break;
            case CharacterState.ultimate:
                animator.SetBool("Ultimate", false);
                break;
            case CharacterState.hit:
                animator.SetBool("Hit", false);
                break;
            case CharacterState.dead:
                animator.SetBool("Dead", false);
                break;
        }

        state = (CharacterState)stateNum;

        switch (state)
        {
            case CharacterState.idle:
                animator.SetBool("Idle", true);
                animator.Play("Idle");
                break;
            case CharacterState.walk:
                animator.SetBool("Move", true);
                animator.Play("Move");
                break;
            case CharacterState.attack:
                animator.SetBool("Attack", true);
                animator.Play("Attack");
                break;
            case CharacterState.skill:
                animator.SetBool("Skill", true);
                animator.Play("Skill");
                break;
            case CharacterState.ultimate:
                animator.SetBool("Ultimate", true);
                animator.Play("Ultimate");
                break;
            case CharacterState.hit:
                animator.SetBool("Hit", true);
                animator.Play("Hit");
                break;
            case CharacterState.dead:
                animator.SetBool("Dead", true);
                animator.Play("Dead");
                break;
        }
    }
}