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
        public equipExplain equipExplain;
    }

    [System.Serializable]
    public class EquipInfo//플레이어의 팀 및 정보
    {
        public int itemIndex;
        public string ItemName;
        public bool unlock;
        public int[] arrBonusStatCount;
        public int[] arrBonusType;
        public int[] arrTypeMastery;
        public int ChampMastery;
        public int[] arrHeadsetStatePoint;

    }
    [System.Serializable]
    public class equipExplain//플레이어의 팀 및 정보
    {
        public string[] arrTypeMent;
        public string[] arrTypeMastery;

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

        public TextMeshProUGUI attackText;
        public TextMeshProUGUI deffenceText;

        public int equipBonusAttack;
        public int equipBonusDefence;
        public int coolTimeBonus;
        public List<int> arrChampBonusType = new List<int>();// 챔피언의 인덱스값 챔피언 보너스가 여러개 있을수도 있어서 List
        public List<int> arrChampBonusStat = new List<int>();
        public int masteryBonusType;//챔피언 마스터리 인덱스값 1 전사 2 궁수 3마법사  4 전투보조 , 5어썌신
        public int masteryBonusStat;
        [SerializeField]
        private ItemInfoBox[] itemInfoBox  = new ItemInfoBox[4];
        [SerializeField]
        private GameObject equipChangeBG;
        public void Init()
        {
            equipInfoText = Resources.Load("equipInfo") as TextAsset;
            equipInfoCollet = JsonUtility.FromJson<EquipInfoCollect>(equipInfoText.text);
            var equipInfo = equipInfoCollet.equipInfo;
            var equipExplain = equipInfoCollet.equipExplain;
            var characterState = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>().characterStateList.characterState;

            int[] arrWearUniform = {playerInformation.GetPlayerInfo().wearHeadset, playerInformation.GetPlayerInfo().wearController,
                playerInformation.GetPlayerInfo().wearChair, playerInformation.GetPlayerInfo().wearUniform };

            for(int i = 0; i <arrWearUniform.Length; i++)
            {
                equipSetting(equipInfo, equipExplain, characterState, arrWearUniform[i], i);
            }

            attackText.text = "+" + equipBonusAttack;
            deffenceText.text = "+" + equipBonusDefence;

        }

        public void equipSetting(List<EquipInfo> equipInfo, equipExplain equipExplain, CharacterJsonRead.CharacterState[] characterState, int itemIndex, int equipIndex)
        {

            for (int i = 0; i < equipInfoCollet.equipInfo.Count; i++)
            {
                if (equipInfoCollet.equipInfo[i].itemIndex == itemIndex)
                {
                    arrEquipChild[equipIndex].GetChild(0).GetComponent<Image>().sprite = imgManager.arrEquipItem[i];
                    arrEquipChild[equipIndex].GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfo[i].ItemName;

                    for (int z = 0; z < 6; z++)
                    {
                        if (equipInfo[i].arrBonusType[z] > 0)
                        {
                            if (z == 0)
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";

                                equipBonusAttack += equipInfo[i].arrBonusStatCount[z];
                                
                            }

                            if (z == 1)
                            {

                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                equipBonusDefence += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == 2)
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                            }

                            if (z == 3)
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + "%" + " ";
                            }


                            if (z == 4)
                            {
                                for (int x = 0; x < 5; x++)
                                {
                                    if (x > 0)
                                    {
                                        arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMastery[x];
                                        arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                        break;
                                    }

                                }

                            }

                            if (z == 5)
                            {
                                for (int x = 0; x < characterState.Length; x++)
                                {
                                    if (characterState[x].indexCharacter == equipInfo[i].ChampMastery)
                                    {
                                        arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += characterState[x].characterName;
                                        break;
                                    }
                                }

                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                            }
                        }

                       
                    }

                }

             

            }


        }

        public void OpenEquipChangeButton(int index)
        {
            //itemInfoBox[index]
            equipChangeBG.gameObject.SetActive(true);
            


        }


    }

    

}
