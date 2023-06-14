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

    // ĳ���� ����
    public int indexCharacter;                           // index��ȣ
    public float maxHealthPoint;                         // �ִ� ü��
    public float healthPoint;                            // ���� ü��
    public float defense;                                // ����
    public float attackDamage;                           // ���ݷ�
    public float attackSpeed;                            // ���ݼӵ�
    public float attackRange;                            // ���� ��Ÿ�
    public float moveSpeed;                              // �̵��ӵ�
    public CharacterState state;                         // ĳ���� ����
    public TeamDivid teamDivid;                          // ��

    // �߰�����
    public float shield;                                 // �ִ� ���差
    [HideInInspector] public List<(float, float)> listShield = new List<(float, float)>(); // ���差 ����Ʈ
    private bool isUseUltimate = false;                  // �ñر� ��뿩�� üũ
    public bool isDead = false;                          // ������� üũ
    public bool isFakeUnit;                              // �н� ����
    private bool isRevive = false;                       // ��Ȱ ����
    public Vector2 v2SpawnPoint;                         // ��Ȱ������ ��ǥ

    // ��Ÿ��(�ӽ��߰�)
    [HideInInspector] public float maxSkillCool;         // ��ų ��Ÿ��
    [HideInInspector] public float maxUltimateCool;      // �ñر� ��Ÿ��
    [HideInInspector] public float attackCool;           // ���� ��Ÿ�ӿ� ����
    [HideInInspector] public float skillCool;            // ��ų ��Ÿ�ӿ� ����
    private float ultimateCool;                          // �ñر� ��Ÿ�ӿ� ����

    // ����, �����
    public float buff_defence;                           // ���� ����
    public bool isTaunt = false;                         // ���� �����

    // �߰� ����
    [HideInInspector] public CharacterPersnality targetCharacter;           // Ÿ�� ĳ����
    [HideInInspector] public List<CharacterPersnality> listTeamCharacters;  // �Ʊ� ĳ���͸���Ʈ
    [HideInInspector] public List<CharacterPersnality> listEnemyCharacters; // �� ĳ���͸���Ʈ
    [HideInInspector] public StageManager stageManager;
    [HideInInspector] public Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer sprHitMotion; // �ǰݸ��

    // �̵����� ���ѿ�
    [HideInInspector] public float minX, maxX, minY, maxY;


    public abstract void Init();
    public abstract void CharacterAttack();
    public abstract void CharacterUltimate();
    public abstract void CharacterSkill();
    public abstract bool isCanSkill();
    public abstract bool isCanUltimate();


    // ���� ���� StageManager���� ���
    public void BattleStart(List<CharacterPersnality> listTeam, List<CharacterPersnality> listEnemy)
    {
        listTeamCharacters = listTeam;
        listEnemyCharacters = listEnemy;

        maxSkillCool = 10f;
        maxUltimateCool = 1f;

        UltimateCoolTime();
        AttackCoolTime();
        SkillCoolTime();
    }

    // ĳ���� ���� �ൿ
    public void CharaterAction()
    {
        if (isDead || state == CharacterState.hit || isRevive) return;

        // �¿����
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
                if (attackSpeed <= attackCool)
                    attackCool = 0f;
                break;
            case CharacterState.skill:
                if (maxSkillCool <= skillCool)
                    skillCool = 0f;
                break;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY));
    }

    // �̵�
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
                // ��ų�� ���� ���ǹ� �߰��ʿ�
                ChangeState(((int)CharacterState.idle));
            }
        }
    }

    // ���� ��Ÿ��
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

    // ��ų ��Ÿ��
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

    // �ñر� ��Ÿ��
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

    // �ñر� ���
    public async void Ultimate()
    {
        ultimateCool = 0;
        isUseUltimate = true;
        ChangeState(((int)CharacterState.ultimate));
    }

    //���ݰ��ɹ���üũ
    private bool isCanAttackRange()
    {
        if (Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange)
            return true;

        return false;
    }

    // �ǰ�
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

    // �����
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

    // �˹�
    public async void KnockBack(Vector3 v3KnockBackPos)
    {
        ChangeState((int)CharacterState.hit);

        await transform.DOMove(v3KnockBackPos, 0.6f).SetEase(Ease.Linear);

        ChangeState((int)CharacterState.idle);
    }

    // ���� �߰�
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

    // ���� ���ӽð�
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

    // ���
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

    //�� Ž��
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

    // ��Ȱ
    public async void ReviveCharater()
    {
        await UniTask.Delay(5000);

        gameObject.SetActive(true);

        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(v2SpawnPoint.x + Random.Range(-75.0f, 75.1f), v2SpawnPoint.y + Random.Range(-150.0f, 150.1f)));

        transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);

        //-----------------ü�µ� �ʱ�ȭ ��ũ��Ʈ �ۼ��ʿ�-------------------
        Init();
        ResetStat();
        animator.enabled = true;
        spriteRenderer.enabled = true;
        isRevive = true;

        await UniTask.Delay(300);

        isRevive = false;
        Debug.Log("ĳ���� ����");
    }

    // �����̻�� �ʱ�ȭ
    private void ResetStat()
    {
        isTaunt = false;
        targetCharacter = null;
        buff_defence = 0f;
        isDead = false;
        ChangeState((int)CharacterState.idle);
    }

    // �̵�����
    public void SetLimitMoveStage(Vector2 v2MinPos, Vector2 v2MaxPos)
    {
        minX = v2MinPos.x * 0.9f;
        minY = v2MinPos.y * 0.9f;
        maxX = v2MaxPos.x * 0.9f;
        maxY = v2MaxPos.y * 0.9f;
    }

    // ĳ���� ���º�ȯ
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