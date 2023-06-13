using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SwordmanCharacter : CharacterPersnality
{
    int skillCount = 0;
    float ultimateRange = 5f;
    Vector3 v3TargetPos;
    Vector2 v2StartPos;
    [SerializeField] private GameObject objUltimate;

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
            if (CharacterStateArray[i].indexCharacter == 101) //챔피언의 인덱스 번호값이 같다면 
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
    }
    
    public override void CharacterAttack()
    {
        targetCharacter.Hit(attackDamage);
        AttackCoolTime();
    }

    public override void CharacterUltimate()
    {
        // 궁극기 좌표
        Vector3 v3Dist = v3TargetPos - transform.position;
        Vector3 v3Dir = v3Dist.normalized;
        Vector3 v3UltimatePos = transform.position + v3Dir * ultimateRange;
        v3UltimatePos = new Vector2(Mathf.Clamp(v3UltimatePos.x, minX, maxX), Mathf.Clamp(v3UltimatePos.y, minY, maxY));

        // 이펙트 생성
        Vector3 v3CenterPos = (transform.position + v3UltimatePos) * 0.5f;
        float angle = Mathf.Atan2(v3Dir.y, v3Dir.x) * Mathf.Rad2Deg;
        GameObject objUltimateEffect = Instantiate(objUltimate, v3CenterPos, Quaternion.AngleAxis(angle - 90, Vector3.forward));
        objUltimateEffect.transform.localScale = new Vector3(0.8f, Vector2.Distance(v2StartPos, v3UltimatePos), 1f);

        // 유닛 궁극기 좌표로 이동
        transform.position = v3UltimatePos;

        Vector2[] arrV2Square = new Vector2[4];

        for (int i = 0; i < 4; i++)
        {
            arrV2Square[i] = objUltimateEffect.transform.GetChild(i).transform.position;
        }

        for (int i = 0; i < listEnemyCharacters.Count; i++)
        {
            if (listEnemyCharacters[i].isDead) continue;

            if(IsInside(listEnemyCharacters[i].transform.position, arrV2Square))
                listEnemyCharacters[i].Hit(100f);
        }

        objUltimateEffect.SetActive(false);
    }

    public override void CharacterSkill()
    {
        skillCount++;
        if (skillCount == 1)
        {
            if (targetCharacter != null && !targetCharacter.isDead)
                targetCharacter.Hit(attackDamage);
        }
        else
        {
            skillCount = 0;
            if (targetCharacter != null && !targetCharacter.isDead)
                targetCharacter.Hit((int)(attackDamage * 0.4f));

            Debug.Log("스킬 => 기본");
        }
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

    public void UltimateSet()
    {
        v3TargetPos = targetCharacter.transform.position;
        v2StartPos = transform.position;
    }

    private bool IsInside(Vector2 B, Vector2[] arrV2SquarePos)
    {
        //crosses는 점q와 오른쪽 반직선과 다각형과의 교점의 개수
        int crosses = 0;
        for (int i = 0; i < arrV2SquarePos.Length; i++)
        {
            int j = (i + 1) % arrV2SquarePos.Length;
            //점 B가 선분 (p[i], p[j])의 y좌표 사이에 있음
            if ((arrV2SquarePos[i].y > B.y) != (arrV2SquarePos[j].y > B.y))
            {
                //atX는 점 B를 지나는 수평선과 선분 (p[i], p[j])의 교점
                double atX = (arrV2SquarePos[j].x - arrV2SquarePos[i].x) * (B.y - arrV2SquarePos[i].y) / (arrV2SquarePos[j].y - arrV2SquarePos[i].y) + arrV2SquarePos[i].x;
                //atX가 오른쪽 반직선과의 교점이 맞으면 교점의 개수를 증가시킨다.
                if (B.x < atX)
                    crosses++;
            }
        }
        return crosses % 2 > 0;
    }
}
