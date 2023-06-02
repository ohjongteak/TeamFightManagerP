using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Framework.UI
{
    public enum EquipStateType
    {
        None,
        공격력증가,
        방어력증가,
        쿨타임감소,
        계열숙련도,
        공격속도

    }

    [System.Serializable]
    public class EquipInfoCollect
    {
        public EquipInfo equipInfo;

    }

    [System.Serializable]
    public class EquipInfo//플레이어의 팀 및 정보
    {
        public EquipStateType[] arrHeadsetStateType;
        public EquipStateType[] arrControllerStateType;
        public EquipStateType[] arrChairStateType;
        public EquipStateType[] arrUniformStateType;

        public int[] arrHeadsetStatePoint;
        public int[] arrControllerStatePoint;
        public int[] arrChairStatePoint;
        public int[] arrUniformStatePoint;

        public string[] arrHeadsetName;
        public string[] arrControllerName;
        public string[] arrChairName;
        public string[] arrUniformName;

    }

   
    public class EquipmentManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] arrEquipChild;
        [SerializeField]
        private ImageManager imgManager;

        public EquipInfoCollect equipInfoCollet = new EquipInfoCollect();
        public TextAsset equipInfoText;
        public PlayerInformation playerInformation;


        public void Init()
        {
            equipInfoText = Resources.Load("equipInfo") as TextAsset;
            equipInfoCollet = JsonUtility.FromJson<EquipInfoCollect>(equipInfoText.text);


            //Debug.Log(playerInformation.GetPlayerInfo().arrWearHeadset);
            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearHeadset.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearHeadset[i] == true)
                {
                    arrEquipChild[0].GetChild(0).GetComponent<Image>().sprite = imgManager.arrHeadset[i];
                    arrEquipChild[0].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrHeadsetName[i];
                    arrEquipChild[0].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrHeadsetStateType[i + 1].ToString();
                    arrEquipChild[0].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrHeadsetStatePoint[i].ToString();
                    break;
                }

            }


            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearController.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearController[i] == true)
                {
                    arrEquipChild[1].GetChild(0).GetComponent<Image>().sprite = imgManager.arrController[i];
                    arrEquipChild[1].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrControllerName[i];
                    arrEquipChild[1].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrControllerStateType[i + 1].ToString();
                    arrEquipChild[1].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrControllerStatePoint[i].ToString();
                    break;
                }

            }

            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearChair.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearChair[i] == true)
                {
                    arrEquipChild[2].GetChild(0).GetComponent<Image>().sprite = imgManager.arrChair[i];
                    arrEquipChild[2].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrChairName[i];
                    arrEquipChild[2].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrChairStateType[i + 1].ToString();
                    arrEquipChild[2].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrChairStatePoint[i].ToString();
                    break;
                }

            }

            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearUniform.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearUniform[i] == true)
                {
                    arrEquipChild[3].GetChild(0).GetComponent<Image>().sprite = imgManager.arrUniform[i];
                    arrEquipChild[3].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformName[i];
                    arrEquipChild[3].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStateType[i + 1].ToString();
                    arrEquipChild[3].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStatePoint[i].ToString();
                    break;
                }

            }

        }

        
    }

}
