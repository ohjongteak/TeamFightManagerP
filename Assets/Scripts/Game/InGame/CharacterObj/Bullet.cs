using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private CharacterPersnality target;
    private bool isHitEffect;
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
            if (!target.isDead)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                // 애니메이션 투사체 Hit 이펙트 적위치에서 보여지도록
                if (animator.GetBool("Hit")) transform.position = target.transform.position;
                else gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            if (damage > 0) // 공격
                target.Hit(damage);
            else // 아군 쉴드 버프
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

        if(circleCollider == null) circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = true;

        if (this.objectPool == null) this.objectPool = objectPool;
    }

    public void ReturnBullet()
    {
        GetComponent<Animator>().SetBool("Hit", false);
        objectPool.ReturnObject(this);
    }
}
