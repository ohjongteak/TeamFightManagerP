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
        public int Year;
        public int Month;
        public int Week;
        public int Gold;
        public int OpenScoutSlot;
        public int Win;
        public int Lose;
        public int RanKing;
        public int League;
        public int OpenSponSlot;
        public int[] ScoutRemainDate;
        public int HairIndex;
        
    }


    public class PlayerInformation : MonoBehaviour
    {
        public PlayerInfoCollect playerInfoCollet = new PlayerInfoCollect();
        public TextAsset PlayerInfoText;
        
        // Start is called before the first frame update\

        public void Init()
        {
            PlayerInfoText = Resources.Load("playerInfo") as TextAsset;
            playerInfoCollet = JsonUtility.FromJson<PlayerInfoCollect>(PlayerInfoText.text);
            // Debug.Log(DateTime.Parse(playerInfoCollet.playerInfo.Date));
            //aa.ToOADate(DateTime.Parse(playerInfoCollet.playerInfo.Date);
        }

        public void AfterWeek(int PlusGold =0, int Win=0, int Lose=0)
        {
            GetPlayerInfo().Gold += PlusGold;
            GetPlayerInfo().Lose += Lose;
            GetPlayerInfo().Win += Win;
            GetPlayerInfo().Week ++;

            for(int i=0; i < GetPlayerInfo().ScoutRemainDate.Length; i++)
            {
                if(GetPlayerInfo().ScoutRemainDate[i] < 6)
                {
                    GetPlayerInfo().ScoutRemainDate[i] -= 1;

                    if (GetPlayerInfo().ScoutRemainDate[i] <= 0)
                    {
                        GetPlayerInfo().ScoutRemainDate[i] = 9;
                        //스카웃 된 사람을 넣어줘야함.
                    }
                }

            }

            if(GetPlayerInfo().Week >= 5)
            {
                GetPlayerInfo().Month += 1;
                GetPlayerInfo().Week = 1;
               
                if(GetPlayerInfo().Month >= 13)
                {
                    GetPlayerInfo().Month = 1;
                    GetPlayerInfo().Year += 1;
                }
            }
            

        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfoCollet.playerInfo;
        }

        
    }

  
   
}
