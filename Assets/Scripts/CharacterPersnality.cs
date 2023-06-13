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
    //public CharacterType myCharacterType;

    public CharacterJsonRead characterJsonRead;          // Json

    // 캐릭터 스텟
    public int indexCharacter;                           // index번호
    public float maxHealthPoint;                         // 최대 체력
    public float healthPoint;                            // 현재 체력
    public float defense;                                // 방어력
    public float attackDamage;                           // 공격력
    public float attackSpeed;                            // 공격속도
    public float attackRange;                            // 공격 사거리
    public float moveSpeed;                              // 이동속도
    public CharacterState state;                         // 캐릭터 상태
    public TeamDivid teamDivid;                          // 팀

    // 추가스텟
    public float shield;                                 // 최대 쉴드량
    [HideInInspector] public List<(float, float)> listShield = new List<(float, float)>(); // 쉴드량 리스트
    private bool isUseUltimate = false;                  // 궁극기 사용여부 체크
    public bool isDead = false;                          // 사망여부 체크
    public bool isFakeUnit;                              // 분신 유닛
    private bool isRevive = false;                       // 부활 유닛
    public Vector2 v2SpawnPoint;                         // 부활생성용 좌표

    // 쿨타임(임시추가)
    [HideInInspector] public float maxSkillCool;         // 스킬 쿨타임
    [HideInInspector] public float maxUltimateCool;      // 궁극기 쿨타임
    [HideInInspector] public float attackCool;           // 공격 쿨타임용 변수
    [HideInInspector] public float skillCool;            // 스킬 쿨타임용 변수
    private float ultimateCool;                          // 궁극기 쿨타임용 변수

    // 버프, 디버프
    public float buff_defence;                           // 버프 방어력
    public bool isTaunt = false;                         // 도발 디버프

    // 추가 변수
    [HideInInspector] public CharacterPersnality targetCharacter;           // 타겟 캐릭터
    [HideInInspector] public List<CharacterPersnality> listTeamCharacters;  // 아군 캐릭터리스트
    [HideInInspector] public List<CharacterPersnality> listEnemyCharacters; // 적 캐릭터리스트
    [HideInInspector] public StageManager stageManager;
    [HideInInspector] public Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer sprHitMotion; // 피격모션

    // 이동범위 제한용
    [HideInInspector] public float minX, maxX, minY, maxY;


    public abstract void Init();
    public abstract void CharacterAttack();
    public abstract void CharacterUltimate();
    public abstract void CharacterSkill();
    public abstract bool isCanSkill();
    public abstract bool isCanUltimate();


    // 전투 시작 StageManager에서 사용
    public void BattleStart(List<CharacterPersnality> listTeam, List<CharacterPersnality> listEnemy)
    {
        listTeamCharacters = listTeam;
        listEnemyCharacters = listEnemy;

        maxSkillCool = 5f;
        maxUltimateCool = 30f;

        UltimateCoolTime();
        AttackCoolTime();
        SkillCoolTime();
    }

    // 캐릭터 상태 행동
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
                if (maxSkillCool <= skillCool)
                    skillCool = 0f;
                break;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));
    }

    // 이동
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

    // 공격 쿨타임
    public async void AttackCoolTime()
    {
        if (state != CharacterState.attack) return;

        attackCool = 0f;
        while (attackSpeed > attackCool)
        {
            if (isDead) return;

            attackCool += Time.fixedDeltaTime;

            if (attackSpeed <= attackCool) break;

            await UniTask.Yield();
        }
    }

    // 스킬 쿨타임
    public async void SkillCoolTime()
    {
        skillCool = 0f;
        while (maxSkillCool > skillCool)
        {
            skillCool += Time.fixedDeltaTime;

            if (maxSkillCool <= skillCool) break;

            await UniTask.Yield();
        }
    }

    // 궁극기 쿨타임
    public async void UltimateCoolTime()
    {
        if (isFakeUnit || isUseUltimate) return;

        ultimateCool = 0f;
        while (maxUltimateCool > ultimateCool)
        {
            if (isUseUltimate) break;

            ultimateCool += Time.fixedDeltaTime;

            await UniTask.Yield();
        }
    }

    // 궁극기 사용
    public async void Ultimate()
    {
        ultimateCool = 0;
        isUseUltimate = true;
        ChangeState(((int)CharacterState.ultimate));
    }

    //공격가능범위체크
    private bool isCanAttackRange()
    {
        if (Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange)
            return true;

        return false;
    }

    // 피격
    public async void Hit(float attackDamage, Debuff debuff = Debuff.none)
    {
        float damage = attackDamage - defense - buff_defence;

        if(shield > 0)
        {
            if (damage > 0)
            {
                HitShield(damage);
            }

            if (shield < damage) damage -= shield;
        }

        healthPoint -= damage;

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

    // 디버프
    private async void ActiveDebuff(Debuff debuff)
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

    // 넉백
    public async void KnockBack(Vector3 v3KnockBackPos)
    {
        ChangeState((int)CharacterState.hit);

        await transform.DOMove(v3KnockBackPos, 0.6f).SetEase(Ease.Linear);

        ChangeState((int)CharacterState.idle);
    }

    // 쉴드 추가
    public void HitShield(float shieldPower, float duration = 0)
    {
        if (duration > 0)
        {
            listShield.Add((shieldPower, duration));

            if (listShield.Count == 1) ShiledDuration();
        }
        else
        {
            float shieldGauge = listShield[0].Item1 - shieldPower;

            while (shieldGauge < 0)
            {
                shieldGauge = listShield[1].Item1 - shieldGauge;
                listShield.RemoveAt(0);
            }

            listShield[0] = (shieldGauge, listShield[0].Item2);
        }

        float totalShield = 0f;
        for (int i = 0; i < listShield.Count; i++)
        {
            totalShield += listShield[i].Item1;
        }

        shield = totalShield;
    }

    // 쉴드 지속시간
    private async void ShiledDuration()
    {
        while (listShield.Count > 0)
        {
            for (int i = 0; i < listShield.Count; i++)
            {
                float shiledTime = Mathf.Floor((listShield[i].Item2 - 0.1f) * 10f) / 10f;
                listShield[i] = (listShield[i].Item1, shiledTime);

                if (listShield[i].Item2 <= 0f)
                {
                    listShield.RemoveAt(i);

                    if (listShield.Count > 0)
                    {
                        for (int j = 0; j < listShield.Count; j++)
                            shield += listShield[j].Item1;
                    }
                    else
                        shield = 0f;
                }

                await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
            }
        }
    }

    // 사망
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

    // 부활
    public async void ReviveCharater()
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

    // 상태이상등 초기화
    private void ResetStat()
    {
        isTaunt = false;
        targetCharacter = null;
        buff_defence = 0f;
        isDead = false;
        ChangeState((int)CharacterState.idle);
    }

    // 이동제한
    public void SetLimitMoveStage(Vector2 v2MinPos, Vector2 v2MaxPos)
    {
        minX = v2MinPos.x * 0.9f;
        minY = v2MinPos.y * 0.9f;
        maxX = v2MaxPos.x * 0.9f;
        maxY = v2MaxPos.y * 0.9f;
    }

    // 캐릭터 상태변환
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