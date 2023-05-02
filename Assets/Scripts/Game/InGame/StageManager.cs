using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    ready,
    battle,
    endBattle
}

public class StageManager : MonoBehaviour
{
    [HideInInspector] BattleState battleState;
    [SerializeField] private SpawnManager spawnManager;

    [HideInInspector] public List<CharacterPersnality> listRTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listLTeamCharacters;


    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.ready;
        listRTeamCharacters = spawnManager.SummonCharactor(3, TeamDivid.enemyTeam);
        listLTeamCharacters = spawnManager.SummonCharactor(3, TeamDivid.myTeam);
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case BattleState.ready:
                for (int i = 0; i < listRTeamCharacters.Count; i++)
                {
                    listRTeamCharacters[i].BattleStart();
                }

                for (int i = 0; i < listLTeamCharacters.Count; i++)
                {
                    listLTeamCharacters[i].BattleStart();
                }
                battleState = BattleState.battle;
                break;

            case BattleState.battle:
                for (int i = 0; i < listRTeamCharacters.Count; i++)
                {
                    listRTeamCharacters[i].CharaterAction(listLTeamCharacters);
                }

                for (int i = 0; i < listLTeamCharacters.Count; i++)
                {
                    listLTeamCharacters[i].CharaterAction(listRTeamCharacters);
                }
                break;

            case BattleState.endBattle:
                break;
        }
    }
}