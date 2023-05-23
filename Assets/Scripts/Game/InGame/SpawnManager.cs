using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private RectTransform rtStage;
       
    [Header("À¯´Ö¼ÒÈ¯ÁÂÇ¥¿ë ¿ÀºêÁ§Æ®")]
    [SerializeField] private GameObject objSpawnR;
    [SerializeField] private GameObject objSpawnL;
    [SerializeField] private GameObject objSpawnBox;

    private List<Vector3> listV3SpawnPos;


    [Header("Å×½ºÆ®¿ë")]
    [SerializeField] List<GameObject> listObjTestPrefab;

    public List<CharacterPersnality> SummonCharactor(int unitCount, TeamDivid teamDivid)
    {
        List<CharacterPersnality> characterPersnalities = new List<CharacterPersnality>();
        Vector2 v2SummonPos = teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;
        listV3SpawnPos = ListSpwanPos(v2SummonPos, unitCount);

        float width = rtStage.rect.width * 0.5f;
        float height = rtStage.rect.height * 0.5f;
        Vector2 v2MinPos = Camera.main.ScreenToWorldPoint(new Vector2(rtStage.position.x - width, rtStage.position.y - height));
        Vector2 v2MaxPos = Camera.main.ScreenToWorldPoint(new Vector2(rtStage.position.x + width, rtStage.position.y + height));


        for (int i = 0; i < unitCount; i++)
        {
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(listV3SpawnPos[i]);

            CharacterPersnality characterPersnality = null;

            //ÀÓ½Ã(»ý¼º¸¸ ³ÀµÖ¾ßµÊ)
            if (teamDivid == TeamDivid.myTeam)
                characterPersnality = Instantiate(listObjTestPrefab[3], new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity, objSpawnBox.transform).GetComponent<CharacterPersnality>();
            else
                characterPersnality = Instantiate(listObjTestPrefab[3], new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity, objSpawnBox.transform).GetComponent<CharacterPersnality>();

            characterPersnality.state = CharacterState.idle;
            characterPersnality.teamDivid = teamDivid;
            characterPersnality.SetLimitMoveStage(v2MinPos, v2MaxPos);
            characterPersnality.isFakeUnit = false;
            characterPersnality.v2SpawnPoint = teamDivid == TeamDivid.myTeam ? objSpawnL.transform.position : objSpawnR.transform.position;

            characterPersnalities.Add(characterPersnality);
        }

        return characterPersnalities;
    }

    private List<Vector3> ListSpwanPos(Vector2 spawnCenterPos, int _n)
    {
        List<Vector3> listSpawnPos = new List<Vector3>();

        if (_n <= 0)
        {
            Debug.Log("ÆÀÀ¯´Ö 0¸¶¸® ÁöÁ¤ ¿À·ù");
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