using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
namespace Framework.UI
{
    public enum LimitScout//선수 스카웃 창 상태
    {
        Lock,
        Searching,
        Can
    }

    [System.Serializable]
    public class ScoutList
    {
        public List<playerScout> summonerScout;

    }
    [System.Serializable]
    public class playerScout//스카웃 정보
    {
        public string name; // 영입될 지역인재 베테랑 슈퍼루키 ... 등등 5가지 종류
        public string ment;// 영입될 인재의 간략한 정보 
        public int date;//영입 소요 시간
        public int cost;//영입에 필요한 골드 

    }


    public class ScoutManager : MonoBehaviour
    {

        public ScoutList scoutList = new ScoutList();
    
        public TextAsset scoutText;
        // Start is called before the first frame update

        [SerializeField]
        private int[] arrNowLukieNumber;

        public LimitScout[] arrlimitScout;
        
        public GameObject[] arrScoutSlots;

        public TextMeshProUGUI[] arrLukieGuidText;
        public TextMeshProUGUI[] arrScoutTitleText;
        public TextMeshProUGUI[] arrScoutCountText;
        public TextMeshProUGUI[] arrDateCountText;
        public TextMeshProUGUI[] arrCostCountText;
        public GameObject[] arrLockImage;//영입 잠겨있을때 잠기는 이미지 
      

        public void Init()//초기화
        {
            scoutText = Resources.Load("ScoutList") as TextAsset;
            scoutList = JsonConvert.DeserializeObject<ScoutList>(scoutText.text);

            for(int i = 0; i < arrScoutSlots.Length; i++)
            {
                GameObject leftArrow = arrScoutSlots[i].transform.Find("TitleImage").Find("LeftGreenArrowButton").gameObject;
                GameObject RightArrow = arrScoutSlots[i].transform.Find("TitleImage").Find("RightGreenArrowButton").gameObject;
                GameObject ArrowParent = leftArrow.transform.parent.transform.parent.gameObject;
                leftArrow.GetComponent<Button>().onClick.AddListener(() => LeftArrowButton(ArrowParent));
                RightArrow.GetComponent<Button>().onClick.AddListener(() => RightArrowButton(ArrowParent));
             
                arrScoutTitleText[i].text = scoutList.summonerScout[0].name;
                arrDateCountText[i].text = scoutList.summonerScout[0].date+"주";
                arrCostCountText[i].text = scoutList.summonerScout[0].cost.ToString();
              
            }
            LockImage();

           
        }

        public void LeftArrowButton(GameObject arrow)//왼쪽 버튼
        {
            int Index = arrow.transform.GetSiblingIndex();

            if (arrNowLukieNumber[Index] > 0)
                arrNowLukieNumber[Index] --;

            arrLukieGuidText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].ment;
            arrScoutTitleText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].name;
            arrScoutCountText[Index].text = "(" + (arrNowLukieNumber[Index] +1) + "/5)";
            arrDateCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].date + "주";
            arrCostCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].cost.ToString();

        }

        public void RightArrowButton(GameObject arrow)//오른쪽 버튼
        {
            int Index = arrow.transform.GetSiblingIndex();

            if (arrNowLukieNumber[Index] < 4)
                arrNowLukieNumber[Index]++ ;

            arrLukieGuidText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].ment;
            arrScoutTitleText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].name;
            arrScoutCountText[Index].text = "(" + (arrNowLukieNumber[Index] +1) + "/5)";
            arrDateCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].date + "주";
            arrCostCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].cost.ToString();

        }

        public void LockImage() //잠긴상태 체크 후 잠겨있다면 잠긴상태 이미지 true
        {
            for(int i = 0; i<arrlimitScout.Length; i++)
            {
                if(arrlimitScout[i] == LimitScout.Lock)
                {
                    arrLockImage[i].gameObject.SetActive(true);
                }
                else if(arrlimitScout[i] == LimitScout.Can)
                {
                    arrLockImage[i].gameObject.SetActive(false);
                }
            }
        }

    }
}

