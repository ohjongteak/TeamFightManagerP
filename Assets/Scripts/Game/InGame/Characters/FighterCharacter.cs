using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class FighterCharacter : CharacterPersnality
{
    float ultimateRange;
    int skillIndex = 0;
    Vector3 v3KnockBackPos;
    
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
        ActiveSkill();
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        if (targetCharacter != null && !targetCharacter.isDead && Vector2.Distance(transform.position, targetCharacter.transform.position) <= attackRange)
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

    public void SkillAttack()
    {
        targetCharacter.Hit(attackDamage);
        Vector3 v3Dist = targetCharacter.transform.position - transform.position;
        Vector3 v3Dir = v3Dist.normalized;
        Vector3 v3HitPos = transform.position + v3Dir * 2f;
        targetCharacter.KnockBack(v3HitPos);
        v3KnockBackPos = transform.position + v3Dir * 1.3f;
    }

    private async UniTask ActiveSkill()
    {
        Vector3 v3Dist, v3Dir, v3SkillPos = Vector3.zero;
        float animSpeed = animator.speed;

        if (skillIndex == 0)
        {
            float distance = Vector2.Distance(transform.position, targetCharacter.transform.position);
            v3Dist = targetCharacter.transform.position - transform.position;
            v3Dir = v3Dist.normalized;
            v3SkillPos = transform.position + v3Dir * (distance * 1.8f);
            skillIndex++;
        }
        else
        {
            v3SkillPos = v3KnockBackPos;
            skillIndex = 0;
        }

        animator.speed = 0f;
        await transform.DOMove(v3SkillPos, 0.4f).SetEase(Ease.Linear);
        animator.speed = animSpeed;


        await UniTask.Yield();
    }
}
