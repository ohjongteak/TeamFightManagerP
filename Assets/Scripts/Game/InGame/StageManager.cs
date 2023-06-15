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

    void Start()
    {
        gameTurn.text = battleState.ToString();
        arrTmpKillScore[0].text = arrTmpKillScore[1].text = "0";

        battleState = BattleState.ready;
        listLTeamCharacters = spawnManager.SummonCharactor(1, TeamDivid.myTeam);
        listRTeamCharacters = spawnManager.SummonCharactor(2, TeamDivid.enemyTeam);        

        BattleStart();
    }

    // 스테이지 상태별 유닛 상태 입력 - 추후 추가예정
    private void FixedUpdate()
    {
        if (battleState == BattleState.battle)
        {
            for (int i = 0; i < listRTeamCharacters.Count; i++)
            {
                listRTeamCharacters[i].CharaterAction();
            }

            for (int i = 0; i < listLTeamCharacters.Count; i++)
            {
                listLTeamCharacters[i].CharaterAction();
            }
        }
    }

    // 전투 시작
    private async void BattleStart()
    {
        await UniTask.Delay(new TimeSpan(0, 0, 5));

        for (int i = 0; i < listRTeamCharacters.Count; i++)
        {
            listRTeamCharacters[i].BattleStart(listRTeamCharacters, listLTeamCharacters);
            listRTeamCharacters[i].stageManager = this;
        }

        for (int i = 0; i < listLTeamCharacters.Count; i++)
        {
            listLTeamCharacters[i].BattleStart(listLTeamCharacters, listRTeamCharacters);
            listLTeamCharacters[i].stageManager = this;
        }

        battleState = BattleState.battle;
        InGameTimer();
        gameTurn.text = battleState.ToString();
    }

    // 대결 제한시간 타이머
    private async void InGameTimer()
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

    // 킬카운트 텍스트 설정
    public void KillScoreRefresh(TeamDivid teamDivid)
    {
        int teamIndex = teamDivid != TeamDivid.myTeam ? 0 : 1;

        arrKillScore[teamIndex]++;
        arrTmpKillScore[teamIndex].text = arrKillScore[teamIndex].ToString();
    }

    // 승부 결과
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