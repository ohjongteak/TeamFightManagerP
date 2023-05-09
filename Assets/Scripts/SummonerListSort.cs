using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Framework.UI
{
    public class SummonerListSort : MonoBehaviour
    {
        [SerializeField]
        private GameObject summonerInfoBoxPrefab;
        [SerializeField]
        private SummonerManager summonerManager;
        [SerializeField]
        private ImageManager imgManager;

        public void Init()
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;
            Transform sortBox = this.transform.GetChild(0).transform.GetChild(0);
            for( int i = 0; i < summonerInfo.Count; i++)
            {
                GameObject SummonerInfoBox = Instantiate(summonerInfoBoxPrefab,sortBox);
                Destroy(summonerManager.InstantiateSummoner(SummonerInfoBox.transform.GetChild(1), i).transform.GetChild(1).gameObject);             
                Transform summonerInfoText = SummonerInfoBox.transform.GetChild(2);
                SummonerInfoBox.transform.GetChild(4).GetComponent<UnityEngine.UI.Image>().sprite = imgManager.conditionArrow[summonerInfo[i].condition];
               //GameObject summonerFace = Instantiate(summonerManager.summonerPrefab, SummonerInfoBox.transform.GetChild(1));
               //summonerFace.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite= imgManager.hairSpirte[summonerStateList[i].hairIndex];

                summonerInfoText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].name;
                summonerInfoText.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].atttack.ToString();
                summonerInfoText.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].defend.ToString();
                summonerInfoText.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].cost.ToString();

            }

        }

    }
}

