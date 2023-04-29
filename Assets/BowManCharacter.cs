using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManCharacter : CharacterPersnality
{
  

   

    // Start is called before the first frame update
    public override void Init()
    {

        //hp = 10;
        //Def = 5;
        //Attack = 1;
        //attackSpeed = 0.5f;
        //moveSpeed = 1;
        //AttackRange = 10;
        //myCharacterType = CharacterType.MarkMan;

        //for (int i = 0; i < listEnemy.Count; i++)
        //    listEnemyDistance.Add(Vector2.Distance(listEnemy[i].transform.position, this.transform.position));

        characterJsonRead = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>();

        //적 들 리스트에 넣기 거리를 계산해서 가장 가까운 거리에 있는 적을 공격하기 위함
        //for (int i = 0; i < listEnemy.Count; i++)
        //    listEnemyDistance.Add(Vector2.Distance(listEnemy[i].transform.position, this.transform.position));

    }

   
}
