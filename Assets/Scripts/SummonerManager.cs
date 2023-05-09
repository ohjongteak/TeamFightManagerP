using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public enum SummonerCondition
{
  
    VeryLow,
    Low,
    Normal,
    High,
    VeryHigh

}

namespace Framework.UI
{
    [System.Serializable]
    public class SummonerCharacter
    {
        public List<SummonerCharacterState> summonerCharacterState;

    }
    [System.Serializable]
    public class SummonerCharacterState
    {
        public string name;
        public int atttack;
        public int defend;
        public int[] mainHero;
        public int hairIndex;
        public int Age;
        public int condition;
        public int cost;
    }

    public class SummonerManager : MonoBehaviour
    {
        [SerializeField]
        private ImageManager imgManager;
        [SerializeField]
       public GameObject summonerPrefab;
        [SerializeField]
        SummonerCharacter summonerCharacter;
        
       
        
        public void Init()
        {
            SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
           
            summonerCharacter = summonerLoadManager.summonerCharacterList;
            var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;

            if (summonerStateList.Count > 0)//저장된 세이브 파일이 있다면
            {
                summonerCharacter.summonerCharacterState = new List<SummonerCharacterState>();
                for (int i = 0; i < summonerStateList.Count; i++)
                {
                    summonerCharacter.summonerCharacterState.Add(new SummonerCharacterState());
                    summonerCharacter.summonerCharacterState[i] = new SummonerCharacterState();
                    summonerCharacter.summonerCharacterState[i] = summonerStateList[i];
                }
            }
            else//조건: 선수단의 정보(세이브Json파일이)가 없으면 선수단 3명 랜덤 생성 랜덤 아직 미구현
            {
                //소환사 랜덤 생성
                summonerCharacter.summonerCharacterState = new List<SummonerCharacterState>();

                int DefaultCount = 3;
                for (int i = 0; i < DefaultCount; i++)
                    summonerCharacter.summonerCharacterState.Add(new SummonerCharacterState());

                summonerCharacter.summonerCharacterState[0].name = "육종택";
                summonerCharacter.summonerCharacterState[0].atttack = 9;
                summonerCharacter.summonerCharacterState[0].defend = 6;
                summonerCharacter.summonerCharacterState[0].Age = 16;
                summonerCharacter.summonerCharacterState[0].condition = (int)SummonerCondition.VeryLow;
                summonerCharacter.summonerCharacterState[0].cost = 96;
                summonerCharacter.summonerCharacterState[1].name = "칠종택";
                summonerCharacter.summonerCharacterState[1].atttack = 8;
                summonerCharacter.summonerCharacterState[1].defend = 7;
                summonerCharacter.summonerCharacterState[1].Age = 17;
                summonerCharacter.summonerCharacterState[1].condition = (int)SummonerCondition.VeryHigh;
                summonerCharacter.summonerCharacterState[1].cost = 96;
                summonerCharacter.summonerCharacterState[2].name = "팔팔종택";
                summonerCharacter.summonerCharacterState[2].atttack = 7;
                summonerCharacter.summonerCharacterState[2].defend = 8;
                summonerCharacter.summonerCharacterState[2].Age = 18;
                summonerCharacter.summonerCharacterState[2].condition = (int)SummonerCondition.Normal;
                summonerCharacter.summonerCharacterState[2].cost = 96;

                summonerStateList = summonerCharacter.summonerCharacterState;
            }

            Transform mainCanvas = GameObject.Find("MainCanvas").GetComponent<Transform>();
            for(int i = 0; i < summonerStateList.Count; i++)
            {
                GameObject playerSummoner = Instantiate(summonerPrefab, new Vector3((i*20)-20,-10,0),Quaternion.identity,mainCanvas);
               //DontDestroyOnLoad(playerSummoner);//선수진이 바뀌지 않는이상 캐릭터 외형이 바뀌지 않으므로 씬 전환 시 매번 생성 해줄 필요는 없다.
                playerSummoner.transform.GetChild(0).GetComponent<Image>().sprite = imgManager.hairSpirte[summonerStateList[i].hairIndex];
                playerSummoner.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = summonerStateList[i].name;
            }

        }

        public void StateToLoadManager()
        {
            
            SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
          
            summonerCharacter = summonerLoadManager.summonerCharacterList;
            var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;
            summonerCharacter.summonerCharacterState = new List<SummonerCharacterState>();

            for (int i = 0; i < summonerStateList.Count; i++)
            {
               summonerCharacter.summonerCharacterState[i] = summonerStateList[i];
            }
        }

        public SummonerCharacter GetSummonerCharacter()
        {
            return summonerCharacter;
        }

        public GameObject InstantiateSummoner(Transform parent = null, int i =0)
        {
            SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
            var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;
            GameObject playerSummoner = Instantiate(summonerPrefab, parent);
            playerSummoner.transform.GetChild(0).GetComponent<Image>().sprite = imgManager.hairSpirte[summonerStateList[i].hairIndex];



            return playerSummoner;
        }

        //public int SummonerConditionState(int i)
        //{
        //    SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
        //    var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;

        //    return summonerStateList[i].condition;
        //}

        //public int SummonerCost(int i)
        //{
        //    SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
        //    var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;

        //    return summonerStateList[i].cost;
        //}

    }
}
