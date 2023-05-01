using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    stay,
    battle,
    end
}

public class InGameManager : MonoBehaviour
{
    [HideInInspector] BattleState battleState;
    [SerializeField] private SpawnManager spawnManager;

    [HideInInspector] public List<CharacterPersnality> listRTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listLTeamCharacters;

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.stay;
        spawnManager.SummonCharactor();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < listRTeamCharacters.Count; i++)
        {
            switch (listRTeamCharacters[i].state)
            {
                case CharacterState.idle:
                    listRTeamCharacters[i].TargetSerch(listLTeamCharacters);
                    break;
                case CharacterState.walk:
                    CharacterPersnality target = listRTeamCharacters[i].targetCharacter;

                    if (target.state == CharacterState.dead) listRTeamCharacters[i].state = CharacterState.idle;
                    else
                    {
                        Debug.Log(target.transform.position + "\n" + Vector2.Distance(listRTeamCharacters[i].transform.position, target.transform.position));

                        if (Vector2.Distance(listRTeamCharacters[i].transform.position, target.transform.position) > 0.5f)
                            listRTeamCharacters[i].transform.position = Vector2.MoveTowards(listRTeamCharacters[i].transform.position, target.transform.position, 1f * Time.deltaTime);
                        else 
                            listRTeamCharacters[i].state = CharacterState.attack;
                    }
                    break;
                case CharacterState.attack:
                    listRTeamCharacters[i].state = CharacterState.dead;
                    Debug.Log("АјАн");
                    break;
            }
        }
    }
}
