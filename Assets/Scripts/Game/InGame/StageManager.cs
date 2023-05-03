using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;
using TMPro;

public enum BattleState
{
    ready,
    battle,
    endBattle
}

public class StageManager : MonoBehaviour
{
    private BattleState battleState;

    [SerializeField] private SpawnManager spawnManager;

    [HideInInspector] public List<CharacterPersnality> listRTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listLTeamCharacters;

    [Header("상단 UI")]
    private int timer = 90;
    [SerializeField] private TextMeshProUGUI tmpTimer;

    private int[] arrKillScore = new int[] { 0, 0 };
    [SerializeField] private TextMeshProUGUI[] arrTmpKillScore;

    // 테스트용
    [SerializeField] private Text gameTurn;
    [SerializeField] private TextMeshProUGUI gameResult;

    // Start is called before the first frame update
    void Start()
    {
        gameTurn.text = battleState.ToString();
        arrTmpKillScore[0].text = arrTmpKillScore[1].text = "0";

        battleState = BattleState.ready;
        listRTeamCharacters = spawnManager.SummonCharactor(1, TeamDivid.enemyTeam);
        listLTeamCharacters = spawnManager.SummonCharactor(3, TeamDivid.myTeam);

        BattleStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleState == BattleState.battle)
        {
            for (int i = 0; i < listRTeamCharacters.Count; i++)
            {
                listRTeamCharacters[i].CharaterAction(listLTeamCharacters);
            }

            for (int i = 0; i < listLTeamCharacters.Count; i++)
            {
                listLTeamCharacters[i].CharaterAction(listRTeamCharacters);
            }
        }
    }

    private async UniTaskVoid BattleStart()
    {
        await UniTask.Delay(new TimeSpan(0, 0, 5));

        for (int i = 0; i < listRTeamCharacters.Count; i++)
        {
            listRTeamCharacters[i].BattleStart();
            listRTeamCharacters[i].stageManager = this;
        }

        for (int i = 0; i < listLTeamCharacters.Count; i++)
        {
            listLTeamCharacters[i].BattleStart();
            listLTeamCharacters[i].stageManager = this;
        }

        battleState = BattleState.battle;
        IngameTimer();
        gameTurn.text = battleState.ToString();
    }

    private async UniTaskVoid IngameTimer()
    {
        while (timer > 0)
        {
            await UniTask.Delay(new TimeSpan(0, 0, 1));
            timer--;

            int minute = timer / 60;
            int second = timer % 60;
            tmpTimer.text = string.Format("{0} : {1}", minute, second);
        }

        for (int i = 0; i < listRTeamCharacters.Count; i++)
        {
            listRTeamCharacters[i].state = CharacterState.idle;
        }

        for (int i = 0; i < listLTeamCharacters.Count; i++)
        {
            listLTeamCharacters[i].state = CharacterState.idle;
        }

        ResultGame();
    }

    public void KillScoreRefresh(TeamDivid teamDivid)
    {
        Debug.Log("킬카운트증가 " + teamDivid);
        int teamIndex = teamDivid != TeamDivid.myTeam ? 0 : 1;

        arrKillScore[teamIndex]++;
        arrTmpKillScore[teamIndex].text = arrKillScore[teamIndex].ToString();
    }

    private void ResultGame()
    {
        battleState = BattleState.endBattle;
        gameTurn.text = battleState.ToString();

        if (arrKillScore[0] > arrKillScore[1])
        {
            gameResult.text = "BLUETEAM WIN";
        }
        else if (arrKillScore[0] < arrKillScore[1])
        {
            gameResult.text = "REDTEAM WIN";
        }
        else
        {
            gameResult.text = "DRAW";
        }
    }
}