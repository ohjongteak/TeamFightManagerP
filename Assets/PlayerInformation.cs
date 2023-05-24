using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Framework.UI
{
    [System.Serializable]
    public class PlayerInfoCollect
    {
        public PlayerInfo playerInfo;

    }
    [System.Serializable]
    public class PlayerInfo
    {
        public string Name;
        public string TeamName;
        public int TeamLogo;
        public string Date;
        public int Gold;
        public int OpenScoutSlot;
        public int Win;
        public int Lose;
        public int RanKing;
        public int League;
        public int OpenSponSlot;
        public int[] ScoutRemainDate;
        public int hairIndex;
    }


    public class PlayerInformation : MonoBehaviour
    {
        public PlayerInfoCollect playerInfoCollet = new PlayerInfoCollect();
        public TextAsset PlayerInfoText;
        public DateTime aa;
        // Start is called before the first frame update\

        public void Init()
        {
            PlayerInfoText = Resources.Load("playerInfo") as TextAsset;
            playerInfoCollet = JsonUtility.FromJson<PlayerInfoCollect>(PlayerInfoText.text);
            // Debug.Log(DateTime.Parse(playerInfoCollet.playerInfo.Date));
            //aa.ToOADate(DateTime.Parse(playerInfoCollet.playerInfo.Date);

            aa = DateTime.Parse(playerInfoCollet.playerInfo.Date);

            Debug.Log(aa.TimeOfDay);
        }

        private void Start()
        {
            Init();
        }
    }
}
