using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FighterCharacter : CharacterPersnality
{ 
    // Start is called before the first frame update
    public override void Init()
    {
        healthPoint = 10;
        defense = 5;
        attackDamage = 1;
        attackSpeed = 0.5f;
        moveSpeed = 1;
        attackRange = 10;
        myCharacterType = CharacterType.MarkMan;


    }

    public override void CharacterAttack()
    {
    }

    public override IEnumerator CharacterUltimate()
    {
        yield return null;
    }

    public override IEnumerator CharacterSkill()
    {
        yield return null;
    }
}
