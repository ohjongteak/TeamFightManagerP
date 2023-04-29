using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        for (int i = 0; i < listEnemy.Count; i++)
            listEnemyDistance.Add(Vector2.Distance(listEnemy[i].transform.position, this.transform.position));

    }

    
}
