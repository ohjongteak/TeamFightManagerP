using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BowManCharacter : CharacterPersnality
{
    private ObjectPool objectPool;
    private int enemyIndex = 0;

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


                attackCool = attackSpeed;
                // 2가지 문제를 가지고 있는데 인덱스 번호값의 처리
                //Init함수를 Start에서 바로 실행해주면 JsonReader가 값을 넣기전에 실행되서 Out Of Range 현상이 발생한다는점

            }
        }
    }

    private void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        animator = GetComponent<Animator>();
        Init();
    }

    private void Update()
    {

    }

    public override void CharacterAttack()
    {
        if (state == CharacterState.ultimate) UltimateTarget();

        if (!targetCharacter.isDead)
        {
            Bullet bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(10f, attackDamage, targetCharacter, objectPool);
        }
    }

    public override IEnumerator CharacterUltimate()
    {
        yield return new WaitForSeconds(3f);

        TargetSerch();
        ChangeState((int)CharacterState.idle);
        Debug.Log("필살기 => 기본");
    }

    public override IEnumerator CharacterSkill()
    {
        Vector3 v3MovePoint = targetCharacter.transform.position - transform.position;
        v3MovePoint = v3MovePoint.normalized;

        while (state == CharacterState.skill)
        {
            transform.position -= v3MovePoint * 1f * Time.deltaTime;
            yield return null;
        }
        Debug.Log("스킬 => 기본");
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

    // 궁극기 타겟 세팅
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
