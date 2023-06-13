using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public List<CharacterPersnality> listCharacters;
    [HideInInspector] public float damage;

    // 애니메이션 이벤트용 함수 - 범위공격
    public void AttactDamage()
    {
        for(int i = 0; i < listCharacters.Count; i++)
        {
            if (Vector2.Distance(transform.position, listCharacters[i].transform.position) <= 0.6f)
                listCharacters[i].Hit(damage);
        }
    }

    // 애니메이션 이벤트용 함수 - 애니메이션 종료시 오브젝트 비활성
    public void EndEffect() => gameObject.SetActive(false);
}
