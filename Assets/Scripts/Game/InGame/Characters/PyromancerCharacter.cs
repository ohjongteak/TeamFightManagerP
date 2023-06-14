using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class PyromancerCharacter : CharacterPersnality
{
    [SerializeField] private GameObject objFireBombPrefab;
    [SerializeField] private GameObject objSpiritPrefab;
    [SerializeField] private GameObject objFireStormPrefab;
    private Fire fireBomb;
    private GameObject objUltimate;
    private Pyromancer_Spirit sprit;
    public bool isSummonSpirit;


    // 캐릭터 데이터 입력
    public override void Init()
    {
        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].indexCharacter == 500) // 챔피언의 인덱스 번호값이 같다면 
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
        fireBomb = Instantiate(objFireBombPrefab, transform.position, Quaternion.identity).GetComponent<Fire>();
        sprit = Instantiate(objSpiritPrefab, transform.position, Quaternion.identity).GetComponent<Pyromancer_Spirit>();
        sprit.SettingSpirit(attackDamage, 2f);
        isSummonSpirit = false;
    }

    // 캐릭터 공격 (애니메이션 이벤트로 사용중) - 범위 공격(오브젝트 ON/OFF)
    public override void CharacterAttack()
    {   
        fireBomb.gameObject.SetActive(true);
        fireBomb.transform.position = targetCharacter.transform.position;
        fireBomb.damage = attackDamage;

        if (fireBomb.listCharacters.Count == 0) fireBomb.listCharacters = listEnemyCharacters;

        AttackCoolTime();
    }

    // 캐릭터 스킬 (애니메이션 이벤트로 사용중) - 몬스터 소환
    public override void CharacterSkill()
    {
        if (sprit.pyromancer == null)
        {
            sprit.listEnemeyCharacters = listEnemyCharacters;
            sprit.pyromancer = this;
        }
        sprit.transform.position = targetCharacter.transform.position;
        sprit.gameObject.SetActive(true);
        sprit.SummonSprit();
        maxSkillCool = 15f;
        SkillCoolTime();
    }


    // 캐릭터 궁극기 (애니메이션 이벤트로 사용중)
    public override void CharacterUltimate()
    {
    }

    // 스킬 사용가능 체크
    public override bool isCanSkill()
    {
        if (Vector2.Distance(targetCharacter.transform.position, transform.position) <= attackRange
            && !isSummonSpirit && targetCharacter != null && !targetCharacter.isDead)
        {
            isSummonSpirit = true;
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

    private async void ActiveUltimate()
    {
        objUltimate = Instantiate(objFireStormPrefab, transform.position, Quaternion.identity);
        await objUltimate.transform.DOScale(5f, 2.5f).SetEase(Ease.Linear);
    }
}
