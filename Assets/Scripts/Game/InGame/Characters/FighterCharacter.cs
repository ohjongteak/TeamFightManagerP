using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FighterCharacter : CharacterPersnality
{
    float ultimateRange;
    
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
            if (CharacterStateArray[i].indexCharacter == 102) //챔피언의 인덱스 번호값이 같다면 
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
        ultimateRange = 3f;
    }

    private void Update()
    {
    }

    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    public override void CharacterUltimate()
    {
        for(int i = 0; i < listEnemyCharacters.Count; i++)
        {
            if (Vector2.Distance(listEnemyCharacters[i].transform.position, transform.position) < ultimateRange)
                listEnemyCharacters[i].Hit(attackDamage * 1.3f, Debuff.airborne);
        }
        Debug.Log("격투가 궁극기 : 범위 에어본");
    }

    public override void CharacterSkill()
    {

    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        if (targetCharacter != null && !targetCharacter.isDead && Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange * 0.5f)
            return true;

        return false;
    }

    public override bool isCanUltimate()
    {
        float distance = Vector2.Distance(transform.position, targetCharacter.transform.position);

        if (targetCharacter != null && !targetCharacter.isDead && distance < ultimateRange)
            return true;

        return false;
    }
}
