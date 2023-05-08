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

        public void Init()
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;
            Transform sortBox = this.transform.GetChild(0).transform.GetChild(0);
            for( int i = 0; i < summonerInfo.Count; i++)
            {
                GameObject SummonerInfoBox = Instantiate(summonerInfoBoxPrefab,sortBox);

                Transform SummonerInfoText = SummonerInfoBox.transform.GetChild(2);

                SummonerInfoText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].name;
                SummonerInfoText.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].atttack.ToString();
                SummonerInfoText.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = summonerInfo[i].defend.ToString();

            }
            

        }

    }
}

