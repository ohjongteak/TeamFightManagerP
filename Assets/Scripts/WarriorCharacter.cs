using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class WarriorCharacter : CharacterPersnality
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
           if(CharacterStateArray[i].indexCharacter == 100) //챔피언의 인덱스 번호값이 같다면 
           {
                //등등 스텟 넣기
                name = CharacterStateArray[i].characterName;
                healthPoint = CharacterStateArray[i].healthPoint;
                attackDamage = CharacterStateArray[i].attackDamage;
                attackSpeed = CharacterStateArray[i].attackSpeed;
                moveSpeed = CharacterStateArray[i].moveSpeed * 0.5f;
                attackRange = CharacterStateArray[i].attackRange;

                attackCool = attackSpeed;
                // 2가지 문제를 가지고 있는데 인덱스 번호값의 처리
                //Init함수를 Start에서 바로 실행해주면 JsonReader가 값을 넣기전에 실행되서 Out Of Range 현상이 발생한다는점
            }

        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {

    }

    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        ChangeState((int)CharacterState.idle);
    }

    public override IEnumerator CharacterUltimate()
    {
        float ultimateTime = 4f;

        Debug.Log("기사 궁극기 : 방증");

        for(int i = 0; i < listTeamCharacters.Count; i++)
        {
            listTeamCharacters[i].defense += 5f;
        }

        Debug.Log("필살기 => 기본");
        state = CharacterState.idle;

        yield return new WaitForSeconds(5f);

        for (int i = 0; i < listTeamCharacters.Count; i++)
        {
            listTeamCharacters[i].defense -= 5f;
        }

        Debug.Log("기사 궁극기 종료");

        yield return null;
    }

    public override IEnumerator CharacterSkill()
    {
        yield return null;
    }

    public override bool isCanSkill()
    {

        return true;
    }
}
