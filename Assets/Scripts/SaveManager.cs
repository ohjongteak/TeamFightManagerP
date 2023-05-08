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
        void PlayerSummonerSave()
        {
            SummonerCharacter summonerCharacter = summonerManager.GetSummonerCharacter();
          
            string json = JsonUtility.ToJson(summonerCharacter, true);
            File.WriteAllText(Application.dataPath + "/Resources" + "/PlayerSummoner.json", json);

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))//юс╫ц
            {
                PlayerSummonerSave();

            }
        }
    }
}
