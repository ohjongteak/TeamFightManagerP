using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Framework.UI
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        SummonerManager summonerManager;
        [SerializeField]
        PlayerInformation playerInformation;
        [SerializeField]
        TrainingManager trainingManager;
        void PlayerSummonerSave()
        {
            SummonerCharacter summonerCharacter = summonerManager.SetSummonerCharacter();
          
            string json = JsonUtility.ToJson(summonerCharacter, true);
            File.WriteAllText(Application.dataPath + "/Resources" + "/PlayerSummoner.json", json);

        }

        void PlayerInformationSave()
        {
            playerInformation.AfterWeek(000, 0, 1);


        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))//�ӽ�
            {
                PlayerSummonerSave();
                PlayerInformationSave();
                trainingManager.SaveTraining();

            }
        }
    }
}
