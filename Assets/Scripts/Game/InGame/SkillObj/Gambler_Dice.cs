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

    // �ֻ��� �ִϸ��̼� ������ �ֻ��� �� ��������Ʈ ����
    // �ֻ��� ���� ����ŭ ��ų����Ƚ�� ĳ���Ϳ� �Է�
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
    
    // �ֻ��� �ִϸ��̼� ����� �Ⱥ��̵��� ����
    public void BreakDice()
    {
        sprSkillDice.sprite = null;
    }
    
    // ��ų ����� ������Ʈ ��Ȱ��
    public void EndSkill()
    {
        gameObject.SetActive(false);
    }
}
