using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Image objStage;

    [HideInInspector] public List<CharacterPersnality> listRTeamCharacters;
    [HideInInspector] public List<CharacterPersnality> listLTeamCharacters;

    [Header("유닛소환좌표용 오브젝트")]
    [SerializeField] private GameObject arrObjSpawnR;
    [SerializeField] private GameObject arrObjSpawnL;
    
    private List<Vector3> listV3SpawnPosR;
    private List<Vector3> listV3SpawnPosL;


    [Header("테스트용")]
    [SerializeField] GameObject objTestPrefab;

    void Start()
    {
        int rTeamUnitCount = 1;//listV3SpawnPosL.Count;
        int lTeamUnitCount = 4;//listV3SpawnPosL.Count;

        listV3SpawnPosR = ListSpwanPos(arrObjSpawnR.transform.position, rTeamUnitCount);
        listV3SpawnPosL = ListSpwanPos(arrObjSpawnL.transform.position, lTeamUnitCount);

        for (int i = 0; i < rTeamUnitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPosR[i]);
             Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
        }
        for (int i = 0; i < lTeamUnitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPosL[i]);
            Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
        }
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