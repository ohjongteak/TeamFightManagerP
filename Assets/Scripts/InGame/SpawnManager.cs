using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Image objStage;
       
    [Header("유닛소환좌표용 오브젝트")]
    [SerializeField] private GameObject objSpawnR;
    [SerializeField] private GameObject objSpawnL;
    [SerializeField] private GameObject objSpawnBox;

    private List<Vector3> listV3SpawnPos;


    [Header("테스트용")]
    [SerializeField] GameObject objTestPrefab;

    public List<CharacterPersnality> SummonCharactor(int unitCount, TeamDivid teamDivid)
    {
        List<CharacterPersnality> characterPersnalities = new List<CharacterPersnality>();
        Vector2 v2SummonPos = teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;

        listV3SpawnPos = ListSpwanPos(v2SummonPos, unitCount);

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPos[i]);

            //임시(생성만 냅둬야됨)
            CharacterPersnality characterPersnality = Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity, objSpawnBox.transform).GetComponent<CharacterPersnality>();
            characterPersnality.state = CharacterState.idle;
            characterPersnalities.Add(characterPersnality);
        }

        return characterPersnalities;
    }

    public void ReviveCharater(CharacterPersnality character)
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