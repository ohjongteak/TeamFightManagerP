using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쉴드 5초지속

public class MonkCharacter : CharacterPersnality
{
    List<CharacterPersnality> listBuffCharacter = new List<CharacterPersnality>();

    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        //공격할 대상자를 리스트에 넣기 거리를 계산해서 가장 가까운 거리에 있는 적을 공격하기 위함
        //for (int i = 0; i < listEnemy.Count; i++)
        //    listEnemyDistance.Add(Vector2.Distance(listEnemy[i].transform.position, this.transform.position));

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;
        
        //Debug.Log(CharacterStateList[0]);

        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].indexCharacter == 300) //챔피언의 인덱스 번호값이 같다면 
            {
                //등등 스텟 넣기
                name = CharacterStateArray[i].characterName;
                maxHealthPoint = CharacterStateArray[i].healthPoint;
                healthPoint = CharacterStateArray[i].healthPoint;
                attackDamage = CharacterStateArray[i].attackDamage;
                attackSpeed = CharacterStateArray[i].attackSpeed;
                moveSpeed = CharacterStateArray[i].moveSpeed * 0.5f;
                attackRange = CharacterStateArray[i].attackRange;
                defense = CharacterStateArray[i].defence;

                attackCool = attackSpeed;
                // 2가지 문제를 가지고 있는데 인덱스 번호값의 처리
                //Init함수를 Start에서 바로 실행해주면 JsonReader가 값을 넣기전에 실행되서 Out Of Range 현상이 발생한다는점

            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    public override void CharacterUltimate()
    {
        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (!listTeamCharacters[i].isDead)
            {
                listTeamCharacters[i].HitShield(50f, 5f);
                listTeamCharacters[i].moveSpeed += 0.5f;
                listBuffCharacter.Add(listTeamCharacters[i]);
            }
        }

        Debug.Log("몽크 궁극기 - 쉴드생성");

        StartCoroutine(UltimateBuff());
    }

    IEnumerator UltimateBuff()
    {
        yield return new WaitForSeconds(7f);

        for (int i = 0; i < listBuffCharacter.Count; i++)
        {
            listBuffCharacter[i].moveSpeed -= 0.5f;
        }

        Debug.Log("몽크 궁극기 - 버프 제거");

    }

    public override void CharacterSkill()
    {
        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (!listTeamCharacters[i].isDead)
            {
                float hp = listTeamCharacters[i].healthPoint;

                hp += 30;

                if (listTeamCharacters[i].maxHealthPoint < hp) listTeamCharacters[i].healthPoint = listTeamCharacters[i].maxHealthPoint;
                else listTeamCharacters[i].healthPoint = hp;
            }
        }

        Debug.Log("스킬 => 기본");
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        for(int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (!listTeamCharacters[i].isDead && listTeamCharacters[i].healthPoint < listTeamCharacters[i].maxHealthPoint)
                return true;
        }

        return false;
    }

    public override bool isCanUltimate()
    {
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }
}
