using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public List<CharacterPersnality> listCharacters;
    [HideInInspector] public float damage;

    // �ִϸ��̼� �̺�Ʈ�� �Լ� - ��������
    public void AttactDamage()
    {
        for(int i = 0; i < listCharacters.Count; i++)
        {
            if (Vector2.Distance(transform.position, listCharacters[i].transform.position) <= 0.6f)
                listCharacters[i].Hit(damage);
        }
    }

    // �ִϸ��̼� �̺�Ʈ�� �Լ� - �ִϸ��̼� ����� ������Ʈ ��Ȱ��
    public void EndEffect() => gameObject.SetActive(false);
}
