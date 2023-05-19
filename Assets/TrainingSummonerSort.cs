using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Framework.UI
{
    public class TrainingSummonerSort : MonoBehaviour
    {
        [SerializeField]
        GameObject summonerTraningBox;
        [SerializeField]
        private SummonerManager summonerManager;
        [SerializeField]
        private Transform summonerPanel;
        [SerializeField]
        private List<SummonerInfoBox> summonerInfoList;
        [SerializeField]
        private TextMeshProUGUI summonerName;
        [SerializeField]
        private TextMeshProUGUI summonerAttack;
        [SerializeField]
        private TextMeshProUGUI summonerDefend;
        [SerializeField]
        private TextMeshProUGUI summonerAgeText;
        [SerializeField]
        private Transform summonerFacePanel;
        [SerializeField]
        private GameObject summonerPrefab;
        public void init()
        {
            var summonerInfo = summonerManager.GetSummonerInfo();

            for(int i = 0; i <summonerInfo.Count; i++)
            {

               GameObject summomerInfoBox = Instantiate(summonerTraningBox, summonerPanel);
               summonerInfoList.Add(summomerInfoBox.GetComponent<SummonerInfoBox>());
               summonerInfoList[i].trainingSummonerSort = this;
                summonerInfoList[i].trainingManager = this.GetComponent<TrainingManager>();
               summomerInfoBox.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].name;
               summomerInfoBox.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].remainPoint.ToString(); 
            }

            Destroy((summonerPrefab = summonerManager.InstantiateSummoner(summonerFacePanel.transform)).transform.GetChild(1).gameObject);
            summonerName.text = summonerInfo[0].name;
            summonerAttack.text = summonerInfo[0].atttack.ToString();
            summonerDefend.text = summonerInfo[0].defend.ToString();
            summonerAgeText.text = summonerInfo[0].Age.ToString() +"¼¼";
        }

        public void SummonerBox(string name)
        {
            var summonerInfo = summonerManager.GetSummonerInfo();

            Destroy(summonerPrefab);
            for (int i =0; i < summonerInfoList.Count; i++)
            {
               if(summonerInfo[i].name == name)
               {
                    summonerName.text = summonerInfo[i].name;
                    summonerAttack.text = summonerInfo[i].atttack.ToString();
                    summonerDefend.text = summonerInfo[i].defend.ToString();
                    summonerAgeText.text = summonerInfo[i].Age.ToString()+"¼¼";
                    Destroy((summonerPrefab = summonerManager.InstantiateSummoner(summonerFacePanel.transform,i)).transform.GetChild(1).gameObject);
               }

            }

        }

    }
}
