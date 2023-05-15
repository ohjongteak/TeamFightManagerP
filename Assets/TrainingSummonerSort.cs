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
        private GameObject summonerPrefab;
        public void init()
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;

            for(int i = 0; i <summonerInfo.Count; i++)
            {

               GameObject summomerInfoBox = Instantiate(summonerTraningBox, summonerPanel);
               summonerInfoList.Add(summomerInfoBox.GetComponent<SummonerInfoBox>());
               summonerInfoList[i].trainingSummonerSort = this;
               summomerInfoBox.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].name;
               summomerInfoBox.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].remainPoint.ToString(); 
            }
        }

        public void SummonerBox(string name)
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;

            for (int i =0; i < summonerInfoList.Count; i++)
            {
               if( summonerInfo[i].name == name)
                {
                    summonerName.text = summonerInfo[i].name;
                    summonerAttack.text = summonerInfo[i].atttack.ToString();
                    summonerDefend.text = summonerInfo[i].defend.ToString();
                }
            }
            
        }

    }
}
