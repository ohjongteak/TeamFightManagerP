using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class KnightCharacter : CharacterPersnality
{
    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for(int i = 0; i < CharacterStateArray.Length; i++) 
        {
            if (CharacterStateArray[i].indexCharacter == 100) //챔피언의 인덱스 번호값이 같다면 
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
        Init();
    }

    // 캐릭터 공격 (애니메이션 이벤트로 사용중) - 단일 공격
    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 도발(디버프)
    public override void CharacterSkill()
    {
        StartCoroutine(TauntTime());
    }

    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중) - 아군 방어력 버프
    public override void CharacterUltimate()
    {
        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            listTeamCharacters[i].buff_defence += 5f;
        }

        StartCoroutine(UltimateBuff());
    }

    // 궁극기 버프 지속시간
    IEnumerator UltimateBuff()
    {
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (listTeamCharacters[i].buff_defence > 0)
                listTeamCharacters[i].buff_defence = 0;
        }
    }

    // 도발 지속시간
    IEnumerator TauntTime()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        CharacterPersnality tauntTarget = targetCharacter;
        tauntTarget.isTaunt = true;
        tauntTarget.targetCharacter = this;

        float timer = 0f;

        while (timer < 4f)
        {
            yield return waitForSeconds;
            timer += 0.1f;

            if (tauntTarget.isDead || isDead)
            {
                tauntTarget.isTaunt = false;
                yield break;
            }
        }
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }

    // 궁극기 사용가능 체크
    public override bool isCanUltimate()
    {
        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (!listTeamCharacters[i].isDead && listTeamCharacters[i].healthPoint < listTeamCharacters[i].maxHealthPoint)
                return true;
        }

        return false;
    }
}
