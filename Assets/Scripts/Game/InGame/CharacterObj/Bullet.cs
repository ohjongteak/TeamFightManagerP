using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private CharacterPersnality target;
    public ObjectPool objectPool;

    void FixedUpdate()
    {
        if (target != null)
        {
            if (!target.isDead)
            {
                //Vector3 dir = target.transform.position - transform.position;
                //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            target.Hit(damage);
            objectPool.ReturnObject(this);
            //gameObject.SetActive(false);
        }
    }

    public void SetBullet(float bulletSpeed, float bulletDamage, CharacterPersnality targetCharacter, ObjectPool objectPool)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        target = targetCharacter;

        if(this.objectPool == null) this.objectPool = objectPool;
    }
}
