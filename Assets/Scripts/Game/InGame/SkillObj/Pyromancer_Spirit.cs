using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Pyromancer_Spirit : MonoBehaviour
{
    [SerializeField] private GameObject objCircle;
    [SerializeField] private GameObject objSummonEffect;
    [HideInInspector] public List<CharacterPersnality> listEnemeyCharacters;
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
    private bool isAttack;
    private bool isAlive;
    //1.2

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttack = false;
        isAlive = false;
    }

    private void FixedUpdate()
    {
        if (time <= 0 && isAlive)
        {////////////////////// 여기부터 작업
            objCircle.SetActive(false);
            objCircle.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

            animator.SetBool("Dead", true);
        }
        else if (attackCool > attackSpeed && CanAttack())
        {
            if (listEnemeyCharacters[targetIndex].transform.position.x > transform.position.x) spriteRenderer.flipX = false;
            else if (listEnemeyCharacters[targetIndex].transform.position.x < transform.position.x) spriteRenderer.flipX = true;

            attackCool = 0f;
            animator.SetBool("Attack", true);
        }
    }

    // 소환수 생성
    public async void SummonSprit()
    {
        attackCool = 0f;
        time = 6.1f;
        isAlive = true;

        await objCircle.transform.DOScale(1.2f, 2.5f).SetEase(Ease.Linear);
        //await UniTask.Delay(System.TimeSpan.FromSeconds(2f));

        animator.enabled = true;
        objSummonEffect.SetActive(true);
        DeadCount();
        AttackCoolTime();
    }

    // 소환수 공격(애니메이션 이벤트로 사용중) - 투사체 공격(Object Pool)
    public void SpiritAttack()
    {
        if (!listEnemeyCharacters[targetIndex].isDead)
        {
            Bullet bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(10f, damage, listEnemeyCharacters[targetIndex], objectPool);
        }
        attackCool = 0f;
        animator.SetBool("Attack", false);
        AttackCoolTime();
        isAttack = true;
    }

    // 소환수 제거(애니메이션 이벤트로 사용중)
    public void SpiritDead()
    {
        animator.SetBool("Dead", false);
        gameObject.SetActive(false);
    }

    // 공격가능체크 - 적이 범위내에 있는지 확인
    private bool CanAttack()
    {
        if (isAttack) return false;

        float tempDistance = 1000f;
        float distance = 1000f;
        targetIndex = -1;

        for (int i = 0; i < listEnemeyCharacters.Count; i++)
        {
            if (listEnemeyCharacters[i].isDead) continue;

            tempDistance = Vector2.Distance(listEnemeyCharacters[i].transform.position, transform.position);

            if (tempDistance <= 0.6f && distance < tempDistance)
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
        while (attackSpeed > attackCool)
        {
            if (time <= 0f) return;

            attackCool += Time.fixedDeltaTime;

            if (attackSpeed <= attackCool) break;

            await UniTask.Yield();
        }
    }

    // 소환수 지속시간
    private async void DeadCount()
    {
        while (time > 0f)
        {
            Debug.Log(time);
            time = Mathf.Floor((time - 0.1f) * 10f) / 10f;

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
        }
    }

    // 소환수 변수 입력
    public void SettingSpirit(float spiritDamage, float spiritAttackSpeed, List<CharacterPersnality> listCharacters)
    {
        listEnemeyCharacters = listCharacters;
        damage = spiritDamage;
        attackSpeed = spiritAttackSpeed;
    }
}
