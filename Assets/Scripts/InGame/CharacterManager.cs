using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    stay,
    battle,
    endBattle
}

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] BattleState battleState;
    [SerializeField] private SpawnManager spawnManager;

    [HideInInspector] public List<CharacterPersnality> listRTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listLTeamCharacters;

    
    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.battle;
        listRTeamCharacters = spawnManager.SummonCharactor(1, TeamDivid.enemyTeam);
        listLTeamCharacters = spawnManager.SummonCharactor(3, TeamDivid.myTeam);
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case BattleState.stay:
                break;

            case BattleState.battle:
                Battle();
                break;

            case BattleState.endBattle:
                break;
        }
    }

    private void Battle()
    {
        for (int i = 0; i < listRTeamCharacters.Count; i++)
        {
            switch (listRTeamCharacters[i].state)
            {
                case CharacterState.idle:
                    listRTeamCharacters[i].TargetSerch(listLTeamCharacters);
                    break;

                case CharacterState.walk:
                    listRTeamCharacters[i].Move();
                    break;

                case CharacterState.attack:

                    if (listRTeamCharacters[i].targetCharacter.state == CharacterState.dead)
                    {
                        Debug.Log("공격 => 이동");
                        listRTeamCharacters[i].state = CharacterState.idle;
                        return;
                    }

                    listRTeamCharacters[i].Attack();
                    break;
            }
        }
    }
}