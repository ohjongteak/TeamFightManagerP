using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BowManCharacter : CharacterPersnality
{
    private ObjectPool objectPool;

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
        Bullet bullet = objectPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.SetBullet(5f, attackDamage, targetCharacter, objectPool);
    }

    public override IEnumerator CharacterUltimate()
    {
        float ultimateTime = 2.5f;

        while (ultimateTime > 0f)
        {
            if (isDead) break;

            if (targetCharacter != null && targetCharacter.state != CharacterState.dead)
            {
                CharacterAttack();
            }
            else
                TargetSerch();

            yield return new WaitForSeconds(0.1f);

            ultimateTime -= 0.1f;
        }

        Debug.Log("필살기 => 기본");
    }
}
