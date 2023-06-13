using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManCharacter : CharacterPersnality
{
    private ObjectPool objectPool;
    private int enemyIndex = 0;

    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;
        
        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].indexCharacter == 200) //챔피언의 인덱스 번호값이 같다면 
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
        objectPool = GetComponent<ObjectPool>();
        animator = GetComponent<Animator>();
    }

    // 캐릭터 공격 (애니메이션 이벤트로 사용중) - 투사체 공격(Object Pool)
    public override void CharacterAttack()
    {
        if (state == CharacterState.ultimate) UltimateTarget();

        if (!targetCharacter.isDead)
        {
            Bullet bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(10f, attackDamage, targetCharacter, objectPool);
        }
        AttackCoolTime();
    }

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 백점프, 투사체 공격
    public override void CharacterSkill()
    {
        StartCoroutine(SkillBackJump());
        Debug.Log("스킬 => 기본");
    }

    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중) - 지속시간동안 랜덤타겟 공격
    public override void CharacterUltimate()
    {
        TargetSerch();
    }

    // 스킬 백점프
    IEnumerator SkillBackJump()
    {
        Vector3 v3MovePoint = targetCharacter.transform.position - transform.position;
        v3MovePoint = v3MovePoint.normalized;

        while (state == CharacterState.skill)
        {
            transform.position -= v3MovePoint * 1f * Time.deltaTime;
            yield return null;
        }
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        CharacterPersnality tempCharacter = null;
        float tempDistance = 100f;

        // targetCharacter = listEnemyCharacters[i];
        for (int i = 0; i < listEnemyCharacters.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, listEnemyCharacters[i].transform.position);
            if (distance < 1f && distance < tempDistance)
            {
                tempCharacter = listEnemyCharacters[i];
            }
        }

        if (tempCharacter != null)
        {
            targetCharacter = tempCharacter;
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

    // 궁극기 랜덤 타겟 세팅
    private void UltimateTarget()
    {
        if (!listEnemyCharacters[enemyIndex].isDead)
        {
            targetCharacter = listEnemyCharacters[enemyIndex];
        }
        else
        {
            CharacterPersnality tempEnemy = null;

            for (int i = 0; i < listEnemyCharacters.Count; i++)
            {
                if (i == enemyIndex) continue;

                if (!listEnemyCharacters[i].isDead)
                {
                    if (i < enemyIndex && tempEnemy == null) tempEnemy = listEnemyCharacters[i];
                    else if (i > enemyIndex) tempEnemy = listEnemyCharacters[i];
                }

                targetCharacter = tempEnemy;
            }
        }

        enemyIndex++;
        if (listEnemyCharacters.Count <= enemyIndex) enemyIndex = 0;
    }
}
