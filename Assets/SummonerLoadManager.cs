using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Framework.UI
{
    public class SummonerLoadManager : MonoBehaviour
    {
        [SerializeField]
        public SummonerCharacter summonerCharacterList = new SummonerCharacter();
        public TextAsset textJson;

      
        // Start is called before the first frame update
        public void Init()
        {
            

            try
            {
                textJson = Resources.Load("PlayerSummoner") as TextAsset;
                summonerCharacterList = JsonUtility.FromJson<SummonerCharacter>(textJson.text);
            }
            catch
            {
               
            }
            

           
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Init();
            }
        }
    }
}
