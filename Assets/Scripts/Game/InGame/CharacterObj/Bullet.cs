using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private CharacterPersnality target;
    private bool isHitEffect;
    private bool isMove;
    private Animator animator;
    private CircleCollider2D circleCollider;
    public ObjectPool objectPool;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            if (isMove)
            {
                Vector3 dir = target.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                // �ִϸ��̼� ����ü Hit ����Ʈ ����ġ���� ����������
                if (animator.GetBool("Hit")) transform.position = target.transform.position;
                else objectPool.ReturnObject(this);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            isMove = false;

            if (damage > 0) // ����
                target.Hit(damage);
            else // �Ʊ� ���� ����
                target.HitShield(damage * -1f, 3f);

            if (!isHitEffect) objectPool.ReturnObject(this);
            else
            {
                circleCollider.enabled = false;
                animator.SetBool("Hit", true);
            }
        }
    }

    public void SetBullet(float bulletSpeed, float bulletDamage, CharacterPersnality targetCharacter, ObjectPool objectPool, bool isEffect = false)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        target = targetCharacter;
        isHitEffect = isEffect;
        isMove = true;

        if (circleCollider == null) circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = true;

        if (this.objectPool == null) this.objectPool = objectPool;
    }

    public void ReturnBullet()
    {
        GetComponent<Animator>().SetBool("Hit", false);
        objectPool.ReturnObject(this);
    }
}
