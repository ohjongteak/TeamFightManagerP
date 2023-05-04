using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public CharacterPersnality target;

    void FixedUpdate()
    {
        if (target != null)
        {
            if (target.state != CharacterState.dead)
            {
                Vector3 dir = target.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
            gameObject.SetActive(false);
        }
    }

    public void SetBullet(float bulletSpeed, float bulletDamage, CharacterPersnality targetCharacter)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        target = targetCharacter;
    }
}
