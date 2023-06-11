using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public enum SummonerCondition//선수 컨디션
{
  
    VeryLow,
    Low,
    Normal,
    High,
    VeryHigh

}

public enum SummonerGift//선수의 재능 
{
    Local,
    Excellent,
    RisingStar,
    veteran,
    SuperRookie

}

namespace Framework.UI
{
    [System.Serializable]
    public class SummonerCharacter
    {
        public List<SummonerCharacterState> summonerCharacterState;

    }
    [System.Serializable]
    public class SummonerCharacterState//선수들의 스텟 및 정보
    {
        public string name;//이름
        public int atttack;//공격력
        public int defend;//수비력
        public List<int> mainHero;//메인히어로의 번호 100= 기사 /200= 궁수... 
        public int hairIndex;//헤어
        public int Age;//나이
        public int condition;//컨디션
        public int cost;//재계약 비용
        public int gift; //선수의 재능
        public float[] adventagePoint = new float[6]; //재능 최소 보정 스텟 
        public int[] heroUpPoint = new int[4];//메인히어로 상승확정된 업그레이드 포인트
        public int[] investPoint = new int[6];//포인트를 어떤 곳에 투자했는지. 1/공격 2/방어 3 4 5 6 영웅.....
        public float[] savePoint = new float[6];
        public int remainPoint;//투자한 포인트 /3개가 최대 투자를 안했다면/ 1이라도 남아있음/ 모두 투자했다면 0
     

    }

    public class SummonerManager : MonoBehaviour
    {
        [SerializeField]
        private ImageManager imgManager;
        [SerializeField]
       private GameObject summonerPrefab;//선수들 프리팹
        [SerializeField]
        SummonerCharacter summonerCharacter;//선수들의 정보 클래스
        
       
        
        public void Init()//초기화
        {
            SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
           
            summonerCharacter = summonerLoadManager.summonerCharacterList;
            var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;

            if (summonerStateList.Count > 0)//저장된 세이브 파일이 있다면 1 이상임
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
                GameObject playerSummoner = Instantiate(summonerPrefab, new Vector3((i*20)-20,-10,0),Quaternion.identity,mainCanvas);//선수들 위치 생성
               //DontDestroyOnLoad(playerSummoner);//선수진이 바뀌지 않는이상 캐릭터 외형이 바뀌지 않으므로 씬 전환 시 매번 생성 해줄 필요는 없다.
                playerSummoner.transform.GetChild(0).GetComponent<Image>().sprite = imgManager.hairSpirte[summonerStateList[i].hairIndex];
                playerSummoner.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = summonerStateList[i].name;
            }

        }

        //public void StateToLoadManager()
        //{
            
        //    SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();
          
        //    summonerCharacter = summonerLoadManager.summonerCharacterList;
        //    var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;
        //    summonerCharacter.summonerCharacterState = new List<SummonerCharacterState>();

        //    for (int i = 0; i < summonerStateList.Count; i++)
        //    {
        //       summonerCharacter.summonerCharacterState[i] = summonerStateList[i];
        //    }
        //}

        public SummonerCharacter SetSummonerCharacter()//선수들을 가져오기
        {
            return summonerCharacter;
        }

        public List<SummonerCharacterState> SetSummonerInfo()//선수들의 스텟 정보 가져오기
        {
            return SetSummonerCharacter().summonerCharacterState;
        }

        public GameObject InstantiateSummoner(Transform parent = null, int i =0)// 프리팹으로 선수들 초상화 불러오는게 아니라 그림으로만 절반 잘라서 생성해줘도 되기 떄문에 나중에 수정필요
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
