using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쉴드 5초지속

public class MonkCharacter : CharacterPersnality
{
    List<CharacterPersnality> listBuffCharacter = new List<CharacterPersnality>();
    List<float> arrOriginalSpeed = new List<float>();

    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;
        
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
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // 캐릭터 공격 (애니메이션 이벤트로 사용중)
    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 아군 회복
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
        SkillCoolTime();
    }

    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중) - 아군 쉴드, 이속 버프
    public override void CharacterUltimate()
    {
        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (!listTeamCharacters[i].isDead)
            {
                arrOriginalSpeed.Add(listTeamCharacters[i].moveSpeed);
                listTeamCharacters[i].HitShield(50f, 5f);
                listTeamCharacters[i].moveSpeed += 0.5f;
                listBuffCharacter.Add(listTeamCharacters[i]);
            }
        }

        StartCoroutine(UltimateBuff());
    }

    // 궁극기 버프 지속시간 체크
    IEnumerator UltimateBuff()
    {
        yield return new WaitForSeconds(7f);

        for (int i = 0; i < listBuffCharacter.Count; i++)
        {
            if(listBuffCharacter[i].moveSpeed > arrOriginalSpeed[i])
                listBuffCharacter[i].moveSpeed -= 0.5f;
        }
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

    // 궁극기 사용가능 체크
    public override bool isCanUltimate()
    {
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }
}
