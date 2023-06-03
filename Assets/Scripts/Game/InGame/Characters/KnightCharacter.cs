using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class KnightCharacter : CharacterPersnality
{
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        //공격할 대상자를 리스트에 넣기 거리를 계산해서 가장 가까운 거리에 있는 적을 공격하기 위함
        //for (int i = 0; i < listEnemy.Count; i++)
        //    listEnemyDistance.Add(Vector2.Distance(listEnemy[i].transform.position, this.transform.position));
    
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        //Debug.Log(CharacterStateList[0]);

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
                // 2가지 문제를 가지고 있는데 인덱스 번호값의 처리
                //Init함수를 Start에서 바로 실행해주면 JsonReader가 값을 넣기전에 실행되서 Out Of Range 현상이 발생한다는점
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        Init();
    }

    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    public override void CharacterUltimate()
    {
        Debug.Log("기사 궁극기 : 방증");

        for(int i = 0; i < listTeamCharacters.Count; i++)
        {
            listTeamCharacters[i].buff_defence += 5f;
        }

        Debug.Log("필살기 => 기본");

        StartCoroutine(UltimateBuff());
    }

    IEnumerator UltimateBuff()
    {
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            if (listTeamCharacters[i].buff_defence > 0)
                listTeamCharacters[i].buff_defence = 0;
        }

        Debug.Log("기사 궁극기 종료");
    }

    public override void CharacterSkill()
    {
        StartCoroutine(SkillEffect());
    }

    IEnumerator SkillEffect()
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

    public override bool isCanSkill()
    {
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }

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
