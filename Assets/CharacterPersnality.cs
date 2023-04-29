using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDivid
{
    myTeam,
    enemyTeam

}

//[CreateAssetMenu(fileName = "CharacterPersnality" ,menuName = "scriptable Object/CharacterPersnality")]


public abstract class CharacterPersnality : MonoBehaviour
{
    public int indexCharacter;
    public int healthPoint;
    public int defense;
    public int attackDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;
    public CharacterType myCharacterType;
    public List<CharacterPersnality> listEnemy;
    public List<float> listEnemyDistance;
    public CharacterJsonRead characterJsonRead;


    public abstract void Init();

  

   
    // Start is called before the first frame update

}