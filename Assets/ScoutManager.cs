using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Framework.UI
{
    public enum LimitScout
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
    public class playerScout
    {
        public string name;
        public string ment;
        public int date;
        public int cost;

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
        public GameObject[] arrLockImage;
      

        public void Init()
        {
            scoutText = Resources.Load("ScoutList") as TextAsset;
            scoutList = JsonUtility.FromJson<ScoutList>(scoutText.text);

            for(int i = 0; i < arrScoutSlots.Length; i++)
            {
                GameObject leftArrow = arrScoutSlots[i].transform.Find("TitleImage").Find("LeftGreenArrowButton").gameObject;
                GameObject RightArrow = arrScoutSlots[i].transform.Find("TitleImage").Find("RightGreenArrowButton").gameObject;
                GameObject ArrowParent = leftArrow.transform.parent.transform.parent.gameObject;
                leftArrow.GetComponent<Button>().onClick.AddListener(() => LeftArrowButton(ArrowParent));
                RightArrow.GetComponent<Button>().onClick.AddListener(() => RightArrowButton(ArrowParent));
             
                arrScoutTitleText[i].text = scoutList.summonerScout[0].name;
                arrDateCountText[i].text = scoutList.summonerScout[0].date+"аж";
                arrCostCountText[i].text = scoutList.summonerScout[0].cost.ToString();
              
            }
            LockImage();

           
        }

        public void LeftArrowButton(GameObject arrow)
        {
            int Index = arrow.transform.GetSiblingIndex();

            if (arrNowLukieNumber[Index] > 0)
                arrNowLukieNumber[Index] --;

            arrLukieGuidText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].ment;
            arrScoutTitleText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].name;
            arrScoutCountText[Index].text = "(" + (arrNowLukieNumber[Index] +1) + "/5)";
            arrDateCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].date + "аж";
            arrCostCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].cost.ToString();

        }

        public void RightArrowButton(GameObject arrow)
        {
            int Index = arrow.transform.GetSiblingIndex();

            if (arrNowLukieNumber[Index] < 4)
                arrNowLukieNumber[Index]++ ;

            arrLukieGuidText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].ment;
            arrScoutTitleText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].name;
            arrScoutCountText[Index].text = "(" + (arrNowLukieNumber[Index] +1) + "/5)";
            arrDateCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].date + "аж";
            arrCostCountText[Index].text = scoutList.summonerScout[arrNowLukieNumber[Index]].cost.ToString();

        }

        public void LockImage()
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

