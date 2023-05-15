using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{ 
    public class TrainingManager : MonoBehaviour
    {
        [SerializeField]
        SummonerManager summonerManager;


        [SerializeField]
        Transform summonerGridPanel;
        public void Init()
        {
            var summonerInfo = summonerManager.GetSummonerCharacter().summonerCharacterState;
        }
    }
}
