using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;


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

public enum Debuff
{
    none,
    stun,
    freeze,
    airborne,
    slow
}

public abstract class CharacterPersnality : MonoBehaviour
{
    [HideInInspector] public StageManager stageManager;

    public int indexCharacter;
    public float maxHealthPoint;
    public float healthPoint;
    public float defense;
    public float attackDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;
    [HideInInspector] public float maxSkillCool; //스킬쿨 임시추가
    [HideInInspector] public float maxUltimateCool;
     public float buff_defence;

    private bool isUseUltimate = false;
    public CharacterType myCharacterType;
    public CharacterJsonRead characterJsonRead;
    public bool isDead = false;
    public bool isFakeUnit;
    public bool isTaunt = false;
    public float shield;
    [HideInInspector] public Queue<float> quShield;

    public CharacterState state;
    public TeamDivid teamDivid;
    [HideInInspector] public CharacterPersnality targetCharacter;

    [HideInInspector] public float attackCool;
    [HideInInspector] public float skillCool;
    private float ultimateCool;
    private bool isRevive = false;

    public Vector2 v2SpawnPoint;

    [HideInInspector] public List<CharacterPersnality> listTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listEnemyCharacters;
    [HideInInspector] public Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer sprHitMotion;

    // 이동제한용
    [HideInInspector] public float minX, maxX, minY, maxY;


    public abstract void Init();
    public abstract void CharacterAttack();
    public abstract void CharacterUltimate();
    public abstract void CharacterSkill();
    public abstract bool isCanSkill();
    public abstract bool isCanUltimate();


    public void BattleStart(List<CharacterPersnality> listTeam, List<CharacterPersnality> listEnemy)
    {
        listTeamCharacters = listTeam;
        listEnemyCharacters = listEnemy;

        maxSkillCool = 10000f;
        maxUltimateCool = 1f;

        UltimateCoolTime();
        AttackCoolTime();
        SkillCoolTime();
    }

    public void CharaterAction()
    {
        if (isDead || state == CharacterState.hit || isRevive) return;

        // 좌우반전
        if (targetCharacter != null && !targetCharacter.isDead && (state == CharacterState.idle || state == CharacterState.walk))
        {
            if (targetCharacter.transform.position.x > transform.position.x) spriteRenderer.flipX = false;
            else if (targetCharacter.transform.position.x < transform.position.x) spriteRenderer.flipX = true;
        }

        switch (state)
        {
            case CharacterState.idle:
                if (targetCharacter == null || targetCharacter.isDead)
                {
                    TargetSerch();
                }
                else if (!targetCharacter.isDead)
                {
                    if (ultimateCool >= maxUltimateCool && isCanUltimate())
                    {
                        Ultimate();
                    }
                    else if (skillCool >= maxSkillCool && isCanSkill()) ChangeState((int)CharacterState.skill);
                    else if (attackCool >= attackSpeed && isCanAttackRange()) ChangeState((int)CharacterState.attack);
                    else if (!isCanAttackRange()) ChangeState((int)CharacterState.walk);
                }
                break;

            case CharacterState.walk:
                Move();
                break;

            case CharacterState.attack:
                break;
            case CharacterState.skill:

                if(skillCool >= maxSkillCool)
                    SkillCoolTime();
                break;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));
    }

    public void Move()
    {
        if (targetCharacter.isDead) ChangeState(((int)CharacterState.idle));
        else
        {
            if (!isCanAttackRange())
            {
                if (ultimateCool >= maxUltimateCool && isCanUltimate())
                {
                    Ultimate();
                }
                else if (skillCool >= maxSkillCool && isCanSkill())
                    ChangeState((int)CharacterState.skill);
                else 
                    transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // 스킬과 공격 조건문 추가필요
                ChangeState(((int)CharacterState.idle));
            }
        }
    }


    public async UniTaskVoid AttackCoolTime()
    {
        if (state != CharacterState.attack) return;

        attackCool = 0f;
        while (attackSpeed > attackCool)
        {
            if (isDead) return;

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
        ultimateCool = 0;
        isUseUltimate = true;
        ChangeState(((int)CharacterState.ultimate));
    }

    public async UniTaskVoid UltimateCoolTime()
    {
        if (isFakeUnit || isUseUltimate) return;

        ultimateCool = 0f;
        while (maxUltimateCool > ultimateCool)
        {
            if (isUseUltimate) break;

            ultimateCool += Time.deltaTime;

            await UniTask.Yield();
        }
    }

    public async UniTaskVoid Hit(float attackDamage, Debuff debuff = Debuff.none)
    {
        float damage = attackDamage;

        if(shield > 0)
        {
            shield -= damage - defense - buff_defence;

            if (shield < 0) damage = damage - shield + defense + buff_defence;
        }

        healthPoint -= damage - defense - buff_defence;

        if (healthPoint <= 0)
        {
            isDead = true;
            ChangeState(((int)CharacterState.dead));
        }
        else if (debuff != Debuff.none)
        {
            ActiveDebuff(debuff);
        }
    }

    private async UniTaskVoid ActiveDebuff(Debuff debuff)
    {
        ChangeState((int)CharacterState.hit);
        switch(debuff)
        {
            case Debuff.airborne:

                sprHitMotion.enabled = true;
                spriteRenderer.enabled = false;
                sprHitMotion.flipX = spriteRenderer.flipX;

                await sprHitMotion.transform.DOLocalJump(Vector2.zero, 1.5f, 1, 1f).SetEase(Ease.Linear);

                sprHitMotion.enabled = false;
                spriteRenderer.enabled = true;
                ChangeState((int)CharacterState.idle);
                break;
        }
    }

    public async void KnockBack(Vector3 v3KnockBackPos)
    {
        ChangeState((int)CharacterState.hit);

        await transform.DOMove(v3KnockBackPos, 0.6f).SetEase(Ease.Linear);

        ChangeState((int)CharacterState.idle);
    }

    public void SetShield()
    {
        //////////////////////////////////////////////
    }

    public void Retire()
    {
        if (isFakeUnit)
        {
            gameObject.SetActive(false);
            return;
        }

        ReviveCharater();
        stageManager.KillScoreRefresh(teamDivid);
        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    //공격가능범위체크
    private bool isCanAttackRange()
    {
        if (Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange)
            return true;

        return false;
    }

    //적 탐색
    public void TargetSerch()
    {
        if (isTaunt) return;

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

        if (targetIndex < 0)
        {
            targetCharacter = null;
            return;
        }

        targetCharacter = listEnemyCharacters[targetIndex];

        if (state == CharacterState.ultimate) return;

        if (Vector2.Distance(transform.position, targetCharacter.transform.position) > attackRange)
            ChangeState(((int)CharacterState.walk));
        else if(attackCool >= attackSpeed)
            ChangeState(((int)CharacterState.attack));
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
        ResetStat();
        animator.enabled = true;
        spriteRenderer.enabled = true;
        isRevive = true;

        await UniTask.Delay(300);

        isRevive = false;
        Debug.Log("캐릭터 리젠");
    }

    private void ResetStat()
    {
        isTaunt = false;
        targetCharacter = null;
        buff_defence = 0f;
        isDead = false;
        ChangeState((int)CharacterState.idle);
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
                sprHitMotion.enabled = false;
                spriteRenderer.enabled = true;
                DOTween.Kill(transform);

                animator.SetBool("Dead", true);
                animator.Play("Dead");
                break;
        }
    }
}