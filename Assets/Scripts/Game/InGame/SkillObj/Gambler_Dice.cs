using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Gambler_Dice : MonoBehaviour
{
    Animator skillAnim;

    [SerializeField] GamblerCharacter character;
    [SerializeField] Sprite[] arrDiceSprite;
    [SerializeField] SpriteRenderer sprSkillDice;

    private void Start()
    {
        skillAnim = GetComponent<Animator>();
    }

    // 주사위 애니메이션 보여줄 주사위 눈 스프라이트 설정
    // 주사위 눈의 값만큼 스킬공격횟수 캐릭터에 입력
    public async void ActiveDice()
    {
        int diceCount = 0;
        int diceIndex = -1;
        System.TimeSpan randomDiceTime = System.TimeSpan.FromSeconds(0.1f);
    
        while (diceCount < 4)
        {
            int random = Random.Range(0, 6);
    
            while (random == diceIndex) random = Random.Range(0, 6);
    
            diceIndex = random;
            sprSkillDice.sprite = arrDiceSprite[diceIndex];
            await UniTask.Delay(randomDiceTime);
            diceCount++;
        }

        character.skillAttack = diceIndex + 1;
        skillAnim.SetInteger("DiceCount", diceCount);
    }
    
    // 주사위 애니메이션 종료시 안보이도록 설정
    public void BreakDice()
    {
        sprSkillDice.sprite = null;
    }
    
    // 스킬 종료시 오브젝트 비활성
    public void EndSkill()
    {
        gameObject.SetActive(false);
    }
}
