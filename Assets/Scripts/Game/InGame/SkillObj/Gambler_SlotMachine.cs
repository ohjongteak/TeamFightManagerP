using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Gambler_SlotMachine : MonoBehaviour
{
    [HideInInspector] public List<CharacterPersnality> characters;
    [HideInInspector] public bool isAttack;
    [SerializeField] private Sprite[] arrIconSprite;
    [SerializeField] private Sprite[] arrNumberSprite;
    [SerializeField] private SpriteRenderer[] arrIcon;
    [SerializeField] private SpriteRenderer[] arrDamageNumber;
    [SerializeField] private SpriteRenderer[] arrAttackNumber;
    [SerializeField] private RuntimeAnimatorController[] arrChipAnim;

    private ObjectPool objectPool;
    private Animator animator;

    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
        animator = GetComponent<Animator>();
        ActiveSlotMachine();
    }

    // 슬롯머신생성 후 떨어지는 모션
    private async void ActiveSlotMachine()
    {
        float posY = transform.position.y - 1.35f;
        await transform.DOMoveY(posY, 0.5f).SetEase(Ease.Linear);

        animator.enabled = true;
    }

    // 슬롯머신
    // ? 00 - ?표부터 설정 후 뒤 숫자 랜덤값 적용
    // ? 00 - 윗줄?가 설정되면 아래?는 동일적용, 그 후 숫자 랜덤값적용
    public async void RandomSlot()
    {
        float delayTime = 0.05f;
        System.TimeSpan time = System.TimeSpan.FromSeconds(delayTime);
        float checkTime = 0f;
        float saveTime = 0f;
        Sprite iconSprite = null;

        int power = Random.Range(1, 100);
        int number = Random.Range(1, 30);

        if (isAttack) iconSprite = arrIconSprite[0];
        else iconSprite = arrIconSprite[1];

        while (checkTime < 4.5f)
        {
            if (checkTime < 1.5f)
                arrIcon[0].sprite = arrIconSprite[Random.Range(0, 2)];

            if (checkTime < 3f)
            {
                arrDamageNumber[0].sprite = arrNumberSprite[Random.Range(0, 10)];
                arrDamageNumber[1].sprite = arrNumberSprite[Random.Range(0, 10)];
            }
            else if (checkTime < 4.5f)
            {
                arrDamageNumber[1].sprite = arrNumberSprite[Random.Range(0, 10)];
            }

            await UniTask.Delay(time);
            saveTime = checkTime;
            checkTime += delayTime;

            if (checkTime >= 1.5f && saveTime < 1.5f)
            {
                arrIcon[0].sprite = iconSprite;
            }
            else if (checkTime >= 3f && saveTime < 3f)
            {
                arrIcon[1].sprite = iconSprite;
                arrDamageNumber[0].sprite = arrNumberSprite[(int)(power * 0.1f)];
                arrDamageNumber[1].sprite = arrNumberSprite[Random.Range(0, 10)];
                arrAttackNumber[0].sprite = arrNumberSprite[number / 10];
                arrAttackNumber[1].sprite = arrNumberSprite[number % 10];
            }
            else if (checkTime >= 4.5f && saveTime < 4.5f)
            {
                arrDamageNumber[1].sprite = arrNumberSprite[power % 10];
            }
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        for (int i = 0; i < number; i++)
        {
            Chip(power);

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));
        this.gameObject.SetActive(false);
    }

    // 궁극기 공격 - 랜덤 타겟에게 공격(적), 쉴드버프(아군) 기능 투사체 생성
    private void Chip(int damage)
    {
        int indexNum = characters.Count;

        // 유닛 전멸 체크
        for (int i = 0; i < indexNum; i++)
        {
            if (!characters[i].isDead) break;
        }

        CharacterPersnality target = characters[Random.Range(0, indexNum)];

        while (target.isDead) target = characters[Random.Range(0, indexNum)];

        if (!isAttack) damage *= -1;

        Bullet bullet = objectPool.GetObject();
        bullet.transform.position = transform.position;
        bullet.SetBullet(10f, damage, target, objectPool, true);

        int animatorIndex = 0;
        if (!isAttack) animatorIndex = 1;

        bullet.GetComponent<Animator>().runtimeAnimatorController = arrChipAnim[animatorIndex];
    }
}
