using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Framework.UI
{
   

    [System.Serializable]
    public class EquipInfoCollect
    {
        public List<EquipInfo> equipInfo;

    }

    [System.Serializable]
    public class EquipInfo//플레이어의 팀 및 정보
    {
        public int itemIndex;
        public string ItemName;
        public bool unlock;
        public int[] arrBonusStateCount;
        public int[] arrBonusType;
        public int[] arrTypeMastery;
        public int ChampMastery;
        public int[] arrHeadsetStatePoint;

        
       
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


           
            for (int i = 0; i < equipInfoCollet.equipInfo.Count; i++)
            {
                if (equipInfoCollet.equipInfo[i].itemIndex == playerInformation.GetPlayerInfo().wearHeadset)
                {
                    arrEquipChild[0].GetChild(0).GetComponent<Image>().sprite = imgManager.arrEquipItem[i];
                    arrEquipChild[0].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo[i].ItemName;
                   // arrEquipChild[0].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipInfoCollet.equipInfo[i].
                   //arrEquipChild[0].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrHeadsetStatePoint[i].ToString();

                }

                if (equipInfoCollet.equipInfo[i].itemIndex == playerInformation.GetPlayerInfo().wearController)
                {
                    
                    arrEquipChild[1].GetChild(0).GetComponent<Image>().sprite = imgManager.arrEquipItem[i];
                    arrEquipChild[1].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo[i].ItemName;
                    //arrEquipChild[1].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrControllerStateType[i + 1].ToString();
                    //arrEquipChild[1].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrControllerStatePoint[i].ToString();
                   
                }

                if (equipInfoCollet.equipInfo[i].itemIndex == playerInformation.GetPlayerInfo().wearChair)
                {
                    arrEquipChild[2].GetChild(0).GetComponent<Image>().sprite = imgManager.arrEquipItem[i];
                    arrEquipChild[2].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo[i].ItemName;
                    //arrEquipChild[2].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrChairStateType[i + 1].ToString();
                    //arrEquipChild[2].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrChairStatePoint[i].ToString();

                }

                if (equipInfoCollet.equipInfo[i].itemIndex == playerInformation.GetPlayerInfo().wearUniform)
                {
                    arrEquipChild[3].GetChild(0).GetComponent<Image>().sprite = imgManager.arrEquipItem[i];
                    arrEquipChild[3].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo[i].ItemName;
                    //arrEquipChild[3].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStateType[i + 1].ToString();
                    //arrEquipChild[3].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStatePoint[i].ToString();
                    break;
                }

            }






            //for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearUniform.Length; i++)
            //{
            //    if (playerInformation.GetPlayerInfo().arrWearUniform[i] == true)
            //    {
            //        arrEquipChild[3].GetChild(0).GetComponent<Image>().sprite = imgManager.arrUniform[i];
            //        //arrEquipChild[3].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformName[i];
            //        //arrEquipChild[3].GetChild(2).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStateType[i + 1].ToString();
            //        //arrEquipChild[3].GetChild(3).GetComponent<TextMeshProUGUI>().text = equipInfoCollet.equipInfo.arrUniformStatePoint[i].ToString();
            //        break;
            //    }

            //}

        }

        
    }

}
