using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector2 v2TargetPos;
    private List<CharacterPersnality> listEnemy;

    void FixedUpdate()
    {
        // 적위치가 아닌 궁극기 시전시 적이 있던 좌표에 공격
        // 이동 후 범위 데미지
        if (Vector2.Distance(transform.position, v2TargetPos) > 0.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, v2TargetPos, speed * Time.deltaTime);
        }
        else
        {
            for(int i = 0; i < listEnemy.Count; i++)
            {
                if(Vector2.Distance(v2TargetPos, listEnemy[i].transform.position) < 1.7f)
                {
                    listEnemy[i].Hit(damage);
                }
            }
            gameObject.SetActive(false);
        }
    }

    // 필요변수 입력
    public void SetGranade(float bulletSpeed, float bulletDamage, Vector2 v2Tartget, List<CharacterPersnality> listEnemyCharacters)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        v2TargetPos = v2Tartget;
        listEnemy = listEnemyCharacters;
    }
}
