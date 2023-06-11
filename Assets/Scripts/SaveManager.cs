using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
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
          
            string json = JsonConvert.SerializeObject(summonerCharacter);
            File.WriteAllText(Application.dataPath + "/Resources" + "/PlayerSummoner.json", json);
            
        }

        void PlayerInformationSave()
        {
            playerInformation.AfterWeek(000, 0, 1);


        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))//юс╫ц
            {
                PlayerSummonerSave();
                PlayerInformationSave();
                trainingManager.SaveTraining();

            }
        }
    }
}
