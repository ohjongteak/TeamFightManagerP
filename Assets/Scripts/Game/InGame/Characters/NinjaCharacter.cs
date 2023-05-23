using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class NinjaCharacter : CharacterPersnality
{
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
            if (CharacterStateArray[i].indexCharacter == 400) //챔피언의 인덱스 번호값이 같다면 
            {
                //등등 스텟 넣기
                name = CharacterStateArray[i].characterName;
                maxHealthPoint = CharacterStateArray[i].healthPoint;
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
        animator = GetComponent<Animator>();
        Init();
    }

    private void Update()
    {

    }

    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    public override IEnumerator CharacterUltimate()
    {
        if (isFakeUnit) yield break;

        CharacterPersnality characterPersnality =
        Instantiate(this.gameObject, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.3f, 0f), Quaternion.identity).GetComponent<CharacterPersnality>();

        characterPersnality.teamDivid = teamDivid;
        characterPersnality.Init();
        characterPersnality.maxX = maxX;
        characterPersnality.minX = minX;
        characterPersnality.maxY = maxY;
        characterPersnality.minY = minY;
        characterPersnality.isFakeUnit = true;
        characterPersnality.isDead = true;
        characterPersnality.state = CharacterState.idle;
        characterPersnality.animator.SetBool("Fake", true);
        characterPersnality.animator.Play("Fake");
        characterPersnality.BattleStart(listTeamCharacters, listEnemyCharacters);
        listTeamCharacters.Add(characterPersnality);
        Debug.Log("닌자 궁극기");

        yield return new WaitUntil(() => state != CharacterState.ultimate);

        characterPersnality.animator.SetBool("Fake", false);
        characterPersnality.isDead = false;
        characterPersnality.skillCool = skillCool;
        TargetSerch();

        Debug.Log("필살기 => 기본");
    }

    public override IEnumerator CharacterSkill()
    {
        float pointX = targetCharacter.transform.position.x;
        float pointY = targetCharacter.transform.position.y;
        transform.position = new Vector2(pointX + Random.Range(-0.2f, 0.3f), pointY + Random.Range(-0.2f, 0.3f));
        targetCharacter.Hit(attackDamage * 1.5f);
        Debug.Log("스킬 => 기본");
        yield break;
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }

    public override bool isCanUltimate()
    {
        if (targetCharacter != null && !targetCharacter.isDead && healthPoint == maxHealthPoint)
            return true;

        return false;
    }
}
