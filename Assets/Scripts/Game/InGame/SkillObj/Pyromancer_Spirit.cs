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

    // ���ݽ���
    private float damage;
    private float attackSpeed;

    // ��Ÿ��, ����ð�
    private float attackCool;
    private float time;

    // ����Ȯ�� ����
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

    // ��ȯ�� ����
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

    // ��ȯ�� ����(�ִϸ��̼� �̺�Ʈ�� �����) - ����ü ����(Object Pool)
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

    // ��ȯ�� ����(�ִϸ��̼� �̺�Ʈ�� �����)
    public void SpiritDead()
    {
        isAlive = false;
        animator.SetBool("Dead", false);
        pyromancer.isSummonSpirit = false;
        animator.enabled = false;
        spriteRenderer.sprite = null;
        gameObject.SetActive(false);
    }

    // ���ݰ���üũ - ���� �������� �ִ��� Ȯ��
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

    // ���� ��Ÿ��
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

    // ��ȯ�� ���ӽð�
    private async void DeadCount()
    {
        while (time > 0f)
        {
            time = Mathf.Floor((time - 0.1f) * 10f) / 10f;

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }

    // ��ȯ�� ���� �Է�
    public void SettingSpirit(float spiritDamage, float spiritAttackSpeed)
    {
        transform.position = new Vector2(1000f, 1000f);
        damage = spiritDamage;
        attackSpeed = spiritAttackSpeed;
    }
}
