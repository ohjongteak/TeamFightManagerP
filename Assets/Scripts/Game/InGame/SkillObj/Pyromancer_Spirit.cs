using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

public class Pyromancer_Spirit : MonoBehaviour
{
    [SerializeField] private GameObject objCircle;
    [SerializeField] private GameObject objSummonEffect;
    [HideInInspector] public List<CharacterPersnality> listEnemeyCharacters;
    [HideInInspector] public PyromancerCharacter pyromancer = null;
    private ObjectPool objectPool;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int targetIndex;

    // 공격스텟
    private float damage;
    private float attackSpeed;

    // 쿨타임, 사망시간
    private float attackCool;
    private float time;

    // 상태확인 변수
    private bool isAlive;
    //1.2

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAlive = false;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        //Debug.Log(CanAttack());
        if (time <= 0)
        {
            objCircle.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            objCircle.SetActive(false);

            animator.SetBool("Dead", true);
        }
        else if (attackCool >= attackSpeed && CanAttack())
        {
            if (listEnemeyCharacters[targetIndex].transform.position.x > transform.position.x) spriteRenderer.flipX = false;
            else if (listEnemeyCharacters[targetIndex].transform.position.x < transform.position.x) spriteRenderer.flipX = true;

            animator.SetBool("Attack", true);
            AttackCoolTime();
        }
    }

    // 소환수 생성
    public async void SummonSprit()
    {
        objCircle.SetActive(true);
        attackCool = 0f;
        time = 6.1f;

        await objCircle.transform.DOScale(1.2f, 2.5f).SetEase(Ease.Linear);
        //await UniTask.Delay(System.TimeSpan.FromSeconds(2f));

        animator.enabled = true;
        objSummonEffect.SetActive(true);
        isAlive = true;
        AttackCoolTime();
        DeadCount();
    }

    // 소환수 공격(애니메이션 이벤트로 사용중) - 투사체 공격(Object Pool)
    public void SpiritAttack()
    {
        if (!listEnemeyCharacters[targetIndex].isDead)
        {
            Bullet bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(1.5f, damage, listEnemeyCharacters[targetIndex], objectPool, true);
        }
        attackCool = 0f;
        animator.SetBool("Attack", false);
    }

    // 소환수 제거(애니메이션 이벤트로 사용중)
    public void SpiritDead()
    {
        isAlive = false;
        animator.SetBool("Dead", false);
        pyromancer.isSummonSpirit = false;
        animator.enabled = false;
        spriteRenderer.sprite = null;
        gameObject.SetActive(false);
    }

    // 공격가능체크 - 적이 범위내에 있는지 확인
    private bool CanAttack()
    {
        if (animator.GetBool("Attack")) return false;

        float tempDistance;
        float distance = 1000f;
        targetIndex = -1;

        for (int i = 0; i < listEnemeyCharacters.Count; i++)
        {
            if (listEnemeyCharacters[i].isDead) continue;

            tempDistance = Vector2.Distance(listEnemeyCharacters[i].transform.position, transform.position);

            if (tempDistance <= 1.2f && tempDistance < distance)
            {
                distance = tempDistance;
                targetIndex = i;
            }
        }

        if (targetIndex >= 0)
            return true;

        return false;
    }

    // 공격 쿨타임
    private async void AttackCoolTime()
    {
        attackCool = 0f;
        while (attackSpeed > attackCool)
        {
            if (time <= 0f) return;

            attackCool += 0.1f;

            if (attackSpeed <= attackCool) break;

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }

    // 소환수 지속시간
    private async void DeadCount()
    {
        while (time > 0f)
        {
            time = Mathf.Floor((time - 0.1f) * 10f) / 10f;

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }

    // 소환수 변수 입력
    public void SettingSpirit(float spiritDamage, float spiritAttackSpeed)
    {
        transform.position = new Vector2(1000f, 1000f);
        damage = spiritDamage;
        attackSpeed = spiritAttackSpeed;
    }
}
