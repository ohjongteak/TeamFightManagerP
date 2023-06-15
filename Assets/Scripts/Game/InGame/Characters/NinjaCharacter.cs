using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class NinjaCharacter : CharacterPersnality
{
    CharacterPersnality fakeUnit;

    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;
        
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

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 적에게 순간이동 공격
    public override void CharacterSkill()
    {
        float pointX = targetCharacter.transform.position.x;
        float pointY = targetCharacter.transform.position.y;
        transform.position = new Vector2(pointX + Random.Range(-0.2f, 0.3f), pointY + Random.Range(-0.2f, 0.3f));
        targetCharacter.Hit(attackDamage * 1.5f);

        SkillCoolTime();
    }

    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중) - 분신 생성
    public override void CharacterUltimate()
    {
        if (isFakeUnit) return;

        fakeUnit =
        Instantiate(this.gameObject, new Vector3(transform.position.x - 0.5f, transform.position.y - 0.3f, 0f), Quaternion.identity).GetComponent<CharacterPersnality>();

        fakeUnit.teamDivid = teamDivid;
        fakeUnit.Init();
        fakeUnit.maxX = maxX;
        fakeUnit.minX = minX;
        fakeUnit.maxY = maxY;
        fakeUnit.minY = minY;
        fakeUnit.isFakeUnit = true;
        fakeUnit.isDead = true;
        fakeUnit.state = CharacterState.idle;
        fakeUnit.animator.SetBool("Fake", true);
        fakeUnit.animator.Play("Fake");
        fakeUnit.BattleStart(listTeamCharacters, listEnemyCharacters);
        listTeamCharacters.Add(fakeUnit);

        StartCoroutine(ActiveFakeUnit());
    }

    // 분신유닛 생성시 입력
    IEnumerator ActiveFakeUnit()
    {
        yield return new WaitUntil(() => state != CharacterState.ultimate);

        fakeUnit.animator.SetBool("Fake", false);
        fakeUnit.isDead = false;
        fakeUnit.skillCool = skillCool;
        TargetSerch();
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
        if (targetCharacter != null && !targetCharacter.isDead && healthPoint == maxHealthPoint)
            return true;

        return false;
    }
}
