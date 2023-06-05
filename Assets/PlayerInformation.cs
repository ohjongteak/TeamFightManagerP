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
    public class PlayerInfo//플레이어의 팀 및 정보
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
        public bool[] arrHaveHeadset;
        public bool[] arrHaveController;
        public bool[] arrHaveChair;
        public bool[] arrHaveUniform;
        public int wearHeadset;
        public int wearController;
        public int wearChair;
        public int wearUniform;

    }


    public class PlayerInformation : MonoBehaviour
    {
        public PlayerInfoCollect playerInfoCollet = new PlayerInfoCollect();
        public TextAsset PlayerInfoText;
        
        // Start is called before the first frame update\

        public void Init()//초기화
        {
            PlayerInfoText = Resources.Load("playerInfo") as TextAsset;
            playerInfoCollet = JsonUtility.FromJson<PlayerInfoCollect>(PlayerInfoText.text);
            //Debug.Log(DateTime.Parse(playerInfoCollet.playerInfo.Date));
            //aa.ToOADate(DateTime.Parse(playerInfoCollet.playerInfo.Date);
        }

        public void AfterWeek(int PlusGold =0, int Win =0, int Lose =0)//한주 지난뒤 플레이어 상태 변경
        {
            SetPlayerInfo().Gold += PlusGold;
            SetPlayerInfo().Lose += Lose;
            SetPlayerInfo().Win += Win;
            SetPlayerInfo().Week ++;

            for(int i=0; i < SetPlayerInfo().ScoutRemainDate.Length; i++)
            {
                if(SetPlayerInfo().ScoutRemainDate[i] < 6)
                {
                    SetPlayerInfo().ScoutRemainDate[i] -= 1;

                    if (SetPlayerInfo().ScoutRemainDate[i] <= 0)
                    {
                        SetPlayerInfo().ScoutRemainDate[i] = 9;//스카웃이 완료됫다면 9로 다시 초기화
                        //스카웃 일정이 끝나서 스카웃 된 사람을 넣어줘야함.
                    }
                }

            }

            if(SetPlayerInfo().Week >= 5)//5주차가 되면 초기화
            {
                SetPlayerInfo().Month += 1;//5주차가 되면 1달+
                SetPlayerInfo().Week = 1;//1주로 초기화
               
                if(SetPlayerInfo().Month >= 13)
                {
                    SetPlayerInfo().Month = 1;//12개월이 지났으면 다시 1개월로 초기화
                    SetPlayerInfo().Year += 1;//1년 추가
                }
            }
            

        }

        public PlayerInfo SetPlayerInfo()//플레이어의 정보 반환
        {
            return playerInfoCollet.playerInfo;
        }


        public PlayerInfo GetPlayerInfo()//플레이어의 정보 반환
        {
            var playerinfo = playerInfoCollet.playerInfo;

            return playerinfo;
        }

    }

  
   
}
