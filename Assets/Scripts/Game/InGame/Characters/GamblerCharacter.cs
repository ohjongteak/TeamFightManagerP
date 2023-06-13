using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class GamblerCharacter : CharacterPersnality
{
    private ObjectPool objectPool;
    [HideInInspector] public int skillAttack = 0;
    [SerializeField] private GameObject objDiceSkill;
    [SerializeField] private GameObject objCoin;
    [SerializeField] private GameObject ovjSlotMachine;

    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].indexCharacter == 202) //챔피언의 인덱스 번호값이 같다면 
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
        if (!targetCharacter.isDead)
        {
            Bullet bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(10f, attackDamage, targetCharacter, objectPool);

            if (skillAttack > 0) SkillAttack();
        }
        AttackCoolTime();
    }

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 랜덤 횟수 추가공격
    public override void CharacterSkill()
    {
        objDiceSkill.gameObject.SetActive(true);
        Debug.Log("스킬 => 기본");
    }

    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중) - 슬롯머신 생성
    public override void CharacterUltimate()
    {
        objCoin.SetActive(false);

        bool flip = GetComponent<SpriteRenderer>().flipX;
        float posX = 0;
        
        //0.2f
        if (flip) posX = 1f;
        else posX = -1f;

        Vector2 v2UltiPos = transform.position + new Vector3(posX, 1f, 0f);
        v2UltiPos = new Vector2(Mathf.Clamp(v2UltiPos.x, minX, maxX), v2UltiPos.y);

        Gambler_SlotMachine slotMachine = Instantiate(ovjSlotMachine, v2UltiPos, Quaternion.identity).GetComponent<Gambler_SlotMachine>();

        float random = UnityEngine.Random.Range(0, 5);

        // 0 = 방어, 1~9 = 공격 
        if(random == 0)
        {
            slotMachine.characters = listTeamCharacters;
            slotMachine.isAttack = false;
        }
        else
        {
            slotMachine.characters = listEnemyCharacters;
            slotMachine.isAttack = true;
        }
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
        if (targetCharacter != null && !targetCharacter.isDead)
            return true;

        return false;
    }

    // 스킬 공격
    public async void SkillAttack()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        Bullet bullet;
        int count = skillAttack;
        skillAttack = 0;

        for (int i = 0; i < count; i++)
        {
            bullet = objectPool.GetObject();
            bullet.transform.position = transform.position;
            bullet.SetBullet(10f, attackDamage, targetCharacter, objectPool);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }
    }

    // 스킬 이펙트 오브젝트
    public void ActiveCoinEffect() => objCoin.SetActive(true);
}
