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
        // ����ġ�� �ƴ� �ñر� ������ ���� �ִ� ��ǥ�� ����
        // �̵� �� ���� ������
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

    // �ʿ亯�� �Է�
    public void SetGranade(float bulletSpeed, float bulletDamage, Vector2 v2Tartget, List<CharacterPersnality> listEnemyCharacters)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        v2TargetPos = v2Tartget;
        listEnemy = listEnemyCharacters;
    }
}
