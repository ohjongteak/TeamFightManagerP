using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Image objStage;
       
    [Header("���ּ�ȯ��ǥ�� ������Ʈ")]
    [SerializeField] private GameObject objSpawnR;
    [SerializeField] private GameObject objSpawnL;
    [SerializeField] private GameObject objSpawnBox;

    private List<Vector3> listV3SpawnPos;


    [Header("�׽�Ʈ��")]
    [SerializeField] GameObject objTestPrefab;

    public List<CharacterPersnality> SummonCharactor(int unitCount, TeamDivid teamDivid)
    {
        List<CharacterPersnality> characterPersnalities = new List<CharacterPersnality>();
        Vector2 v2SummonPos = teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;

        listV3SpawnPos = ListSpwanPos(v2SummonPos, unitCount);

        for (int i = 0; i < unitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPos[i]);

            //�ӽ�(������ ���־ߵ�)
            CharacterPersnality characterPersnality = Instantiate(objTestPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity, objSpawnBox.transform).GetComponent<CharacterPersnality>();
            characterPersnality.state = CharacterState.idle;
            characterPersnalities.Add(characterPersnality);

            characterPersnality.v2SpawnPoint = teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;
        }

        return characterPersnalities;
    }

    private List<Vector3> ListSpwanPos(Vector2 spawnCenterPos, int _n)
    {
        List<Vector3> listSpawnPos = new List<Vector3>();

        if (_n <= 0)
        {
            Debug.Log("������ 0���� ���� ����");
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