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
        public TextAsset summonerStateText;
        
     
        // Start is called before the first frame update
        public void Init()
        {
            try
            {
                summonerStateText = Resources.Load("PlayerSummoner") as TextAsset;
                summonerCharacterList = JsonUtility.FromJson<SummonerCharacter>(summonerStateText.text);
            }
            catch
            {
               
            }

        }
    }
}
