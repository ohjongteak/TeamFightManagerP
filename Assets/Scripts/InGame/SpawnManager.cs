using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Image objStage;
    [SerializeField] InGameManager inGameManager;
       
    [Header("유닛소환좌표용 오브젝트")]
    [SerializeField] private GameObject objSpawnR;
    [SerializeField] private GameObject objSpawnL;
    
    private List<Vector3> listV3SpawnPosR;
    private List<Vector3> listV3SpawnPosL;


    [Header("테스트용")]
    [SerializeField] GameObject objTestPrefab;

    public void SummonCharactor()
    {
        // 추후 함수 압축필요
        int rTeamUnitCount = 1;//listV3SpawnPosL.Count;
        int lTeamUnitCount = 1;//listV3SpawnPosL.Count;

        listV3SpawnPosR = ListSpwanPos(objSpawnR.transform.position, rTeamUnitCount);
        listV3SpawnPosL = ListSpwanPos(objSpawnL.transform.position, lTeamUnitCount);

        for (int i = 0; i < rTeamUnitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPosR[i]);

            //임시(생성만 냅둬야됨)
            CharacterPersnality characterPersnality = Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity).GetComponent<CharacterPersnality>();
            characterPersnality.state = CharacterState.idle;
            inGameManager.listRTeamCharacters.Add(characterPersnality);
        }
        for (int i = 0; i < lTeamUnitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPosL[i]);

            //임시(생성만 냅둬야됨)
            CharacterPersnality characterPersnality = Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity).GetComponent<CharacterPersnality>();
            characterPersnality.state = CharacterState.idle;
            inGameManager.listLTeamCharacters.Add(characterPersnality);
        }
    }

    public void ReviveCharator(CharacterPersnality character)
    {
        Vector2 spawnCenterPos = character.teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;

        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(spawnCenterPos.x + Random.Range(-75.0f, 75.1f), spawnCenterPos.y + Random.Range(-150.0f, 150.1f)));

        character.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);


        //-----------------체력등 초기화 스크립트 작성필요-------------------


    }

    private List<Vector3> ListSpwanPos(Vector2 spawnCenterPos, int _n)
    {
        List<Vector3> listSpawnPos = new List<Vector3>();

        if (_n <= 0)
        {
            Debug.Log("팀유닛 0마리 지정 오류");
            return null;
        }
        else
        {
            for (int i = 0; i < _n; i++)
            {
                Vector2 spawnPos = new Vector2(spawnCenterPos.x + Random.Range(-75.0f, 75.1f), spawnCenterPos.y + Random.Range(-150.0f, 150.1f));

                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        listSpawnPos.Add(spawnPos);
                        break;
                    }

                    if (Vector2.Distance(spawnPos, listSpawnPos[j]) < 10f)
                    {                        
                        i--;
                        break;
                    }
                }
            }
        }
            
        return listSpawnPos;
    }
}