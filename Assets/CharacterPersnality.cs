using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamDivid
{
    myTeam,
    enemyTeam
}

//[CreateAssetMenu(fileName = "CharacterPersnality" ,menuName = "scriptable Object/CharacterPersnality")]

public enum CharacterState
{
    idle,
    walk,
    attack,
    skill,
    hit,
    dead
}

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

    public CharacterState state;
    public TeamDivid teamDivid;
    public CharacterPersnality targetCharacter;


    public abstract void Init();

    private void Start()
    {
        state = CharacterState.idle;
    }

    private void Update()
    {
        
    }

    public void TargetSerch(List<CharacterPersnality> listCharactors)
    {
        int targetIndex = 0;
        float distance = Vector2.Distance(listCharactors[0].transform.position, transform.position);

        for (int i = 1; i < listCharactors.Count; i++)
        {
            if (distance > Vector2.Distance(listCharactors[i].transform.position, transform.position))
            {
                targetIndex = i;
                distance = Vector2.Distance(listCharactors[i].transform.position, transform.position);
            }
        }

        targetCharacter = listCharactors[targetIndex];

        state = CharacterState.walk;
    }
}