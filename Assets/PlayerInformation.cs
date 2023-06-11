using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
namespace Framework.UI
{
    [System.Serializable]
    public class PlayerInfoCollect
    {
        public PlayerInfo playerInfo;

    }
    [System.Serializable]
    public class PlayerInfo//�÷��̾��� �� �� ����
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

        public void Init()//�ʱ�ȭ
        {
            PlayerInfoText = Resources.Load("playerInfo") as TextAsset;
            playerInfoCollet = JsonConvert.DeserializeObject<PlayerInfoCollect>(PlayerInfoText.text);
            //Debug.Log(DateTime.Parse(playerInfoCollet.playerInfo.Date));
            //aa.ToOADate(DateTime.Parse(playerInfoCollet.playerInfo.Date);
        }

        public void AfterWeek(int PlusGold =0, int Win =0, int Lose =0)//���� ������ �÷��̾� ���� ����
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
                        SetPlayerInfo().ScoutRemainDate[i] = 9;//��ī���� �Ϸ�̴ٸ� 9�� �ٽ� �ʱ�ȭ
                        //��ī�� ������ ������ ��ī�� �� ����� �־������.
                    }
                }

            }

            if(SetPlayerInfo().Week >= 5)//5������ �Ǹ� �ʱ�ȭ
            {
                SetPlayerInfo().Month += 1;//5������ �Ǹ� 1��+
                SetPlayerInfo().Week = 1;//1�ַ� �ʱ�ȭ
               
                if(SetPlayerInfo().Month >= 13)
                {
                    SetPlayerInfo().Month = 1;//12������ �������� �ٽ� 1������ �ʱ�ȭ
                    SetPlayerInfo().Year += 1;//1�� �߰�
                }
            }
            

        }

        public PlayerInfo SetPlayerInfo()//�÷��̾��� ���� ��ȯ
        {
            return playerInfoCollet.playerInfo;
        }


        public PlayerInfo GetPlayerInfo()//�÷��̾��� ���� ��ȯ
        {
            var playerinfo = playerInfoCollet.playerInfo;

            return playerinfo;
        }

    }

  
   
}
