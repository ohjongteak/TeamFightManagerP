using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Framework.UI
{ 
    public class TrainingManager : MonoBehaviour
    {
        [SerializeField]
        SummonerManager summonerManager;
        [SerializeField]
        Transform summonerGridPanel;
        [SerializeField]
        private TextMeshProUGUI remainText;
        [SerializeField]
        private TextMeshProUGUI attackPointText;
        [SerializeField]
        private TextMeshProUGUI defendPointText;
        [SerializeField]
        private GameObject[] statBoxes;
        public void SummonerBox(string name)
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;

            for (int i = 0; i < summonerInfo.Count; i++)
            {
                if(summonerInfo[i].name == name)
                {
                    remainText.text = summonerInfo[i].remainPoint+"/3";
                    attackPointText.text = summonerInfo[i].statInvestPoint[0].ToString();
                    defendPointText.text = summonerInfo[i].statInvestPoint[1].ToString();

                    for (int z = summonerInfo[i].mainHero.Count; z <= 2; z++)
                    {
                        statBoxes[z + 1].gameObject.SetActive(false);
                    }
                   
                }

            }



        }

    }
}
