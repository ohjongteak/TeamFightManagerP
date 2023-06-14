using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
namespace Framework.UI
{
    public enum BonusStatType
    {
        attackBonus,
        defenceBonus,
        attackSpeedBonus,
        coolTimeBonus,
        champTypeBonus,
        champMasteryBonus
    }

    public enum itemType
    {
        None,
        Headset,
        Controller,
        Chair,
        Uniform,
        
    }


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
        public Dictionary<int, int> dicChampMastery;


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
        public TextMeshProUGUI defenceText;

        private int attackBonus;
        private int defenceBonus;
        private int attackSpeedBonus;
        private int coolTimeBonus;

        public Queue<int> quChampTypeBonus = new Queue<int>();// 
        public Queue<int> quChampBonusIndex = new Queue<int>();
        public int champStatBonus;

        [SerializeField]
        private GameObject equipChangeBG;
        [SerializeField]
        List<EquipInfo> equipInfo;




        public void Init()
        {

            equipInfoText = Resources.Load("equipInfo") as TextAsset;
            equipInfoCollet = JsonConvert.DeserializeObject<EquipInfoCollect>(equipInfoText.text);
            equipInfo = equipInfoCollet.equipInfo;
            var equipExplain = equipInfoCollet.equipExplain;
            var characterState = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>().characterStateList.characterState;

            int[] arrWearUniform = {playerInformation.GetPlayerInfo().wearHeadset, playerInformation.GetPlayerInfo().wearController,
                playerInformation.GetPlayerInfo().wearChair, playerInformation.GetPlayerInfo().wearUniform };





            for (int i = 0; i < arrWearUniform.Length; i++)
            {
                ChangeSetting(equipExplain, characterState, arrWearUniform[i], i, arrEquipChild[i].GetChild(1).GetComponent<TextMeshProUGUI>()
                    , arrEquipChild[i].GetChild(2).GetComponent<TextMeshProUGUI>(), arrEquipChild[i].GetChild(0).GetComponent<Image>());
            }

            attackText.text = "+" + attackBonus;
            defenceText.text = "+" + defenceBonus;


            //if (equipInfo[0].dicChampMastery.Count > 0)
            //{
            //    for (int i = 0; i < equipInfo[0].dicChampMastery.Count; i++)
            //    {
            //        Debug.Log(equipInfo[0]);
            //    }
            //}
            //Debug.Log(equipInfo[1].ItemName);
            //Debug.Log( equipInfo[1].dicChampMastery.ContainsKey(0));
            //Debug.Log(equipInfo[0].dicChampMastery[]);
        }

        public void ChangeSetting(equipExplain equipExplain, CharacterJsonRead.CharacterState[] characterState, int itemIndex, int equipIndex,
            TextMeshProUGUI itemName, TextMeshProUGUI itemEffect, Image itemImage = null)
        {
            itemName.text = "";
            itemEffect.text = "";

            for (int i = 0; i < equipInfoCollet.equipInfo.Count; i++)
            {
                if (equipInfoCollet.equipInfo[i].itemIndex == itemIndex)
                {
                    if (itemImage != null)
                        itemImage.sprite = imgManager.arrEquipItem[i];

                    itemName.text = equipInfo[i].ItemName;
                    //equipChild.GetChild(1).GetComponent<TextMeshProUGUI>().text = equipInfo[i].ItemName;

                    for (int z = 0; z < 6; z++)
                    {
                        if (equipInfo[i].arrBonusType[z] > 0)
                        {
                            if (z == (int)BonusStatType.attackBonus)//공격력
                            {
                                itemEffect.text += equipExplain.arrTypeMent[z] + " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                attackBonus += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == (int)BonusStatType.defenceBonus)//방어력
                            {
                                itemEffect.text += equipExplain.arrTypeMent[z] + " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                defenceBonus += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == (int)BonusStatType.attackSpeedBonus)//공격속도
                            {
                                itemEffect.text += equipExplain.arrTypeMent[z] + " +" + equipInfo[i].arrBonusStatCount[z] + " ";

                                attackSpeedBonus += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == (int)BonusStatType.coolTimeBonus)//쿨타임
                            {
                                itemEffect.text += equipExplain.arrTypeMent[z] + " +" + equipInfo[i].arrBonusStatCount[z] + "%" + " ";

                                coolTimeBonus += equipInfo[i].arrBonusStatCount[z];
                            }


                            if (z == (int)BonusStatType.champTypeBonus)//계열 숙련도
                            {
                                for (int x = 0; x < equipInfo[equipIndex].arrTypeMastery.Length; x++)
                                {
                                    if (equipInfo[equipIndex].arrTypeMastery[x] > 0)
                                    {
                                        itemEffect.text += " " + equipExplain.arrTypeMastery[x] + "+" + equipInfo[equipIndex].arrTypeMastery[x];
                                        //arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                        //브레이크는 걸어주지 않는다 2개 이상 있을수 있기때문
                                    }

                                }

                            }

                            if (z == (int)BonusStatType.champMasteryBonus)//특정 챔피언
                            {
                                foreach (int key in equipInfo[i].dicChampMastery.Keys)
                                {
                                    for (int x = 0; x < characterState.Length; x++)
                                    {
                                        if (characterState[x].indexCharacter == key)
                                        {
                                            itemEffect.text += " " + characterState[x].characterName
                                                + equipExplain.arrTypeMent[z] + "+" + equipInfo[i].dicChampMastery[key];
                                            break;
                                        }
                                    }

                                }

                            }
                        }
                    }

                }

            }

        }

        public void OpenEquipChangeButton(int index)//장비 바꾸기 버튼 눌렀을 때 셋팅
        {
            int[] arrWearUniform = {playerInformation.GetPlayerInfo().wearHeadset, playerInformation.GetPlayerInfo().wearController,
                playerInformation.GetPlayerInfo().wearChair, playerInformation.GetPlayerInfo().wearUniform };

            var equipExplain = equipInfoCollet.equipExplain;
            var characterState = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>().characterStateList.characterState;

            var bgChild = equipChangeBG.transform.GetChild(0);
            var wearingEquip = bgChild.transform.Find("WearItemInfoBox");
            var changeBoxEquip = bgChild.transform.Find("ChangeItemInfoBox");


            for (int i = 0; i < equipInfo.Count; i++)
            {
                if (equipInfoCollet.equipInfo[i].itemIndex == arrWearUniform[index])
                {
                    ChangeSetting(equipExplain, characterState, arrWearUniform[index], i,
                        wearingEquip.GetChild(0).GetComponent<TextMeshProUGUI>(), wearingEquip.GetChild(2).GetComponent<TextMeshProUGUI>());

                    ChangeSetting(equipExplain, characterState, arrWearUniform[index], i,
                       changeBoxEquip.GetChild(0).GetComponent<TextMeshProUGUI>(), changeBoxEquip.GetChild(1).GetComponent<TextMeshProUGUI>());

                }
            }


            List<Sprite> listAA= new List<Sprite>();

            var aa = bgChild.transform.Find("itemImage");

            for(int pp = 0; pp < aa.childCount; pp++)
                listAA.Add( aa.GetChild(pp).GetChild(0).GetComponent<Image>().sprite);//아이템의 모든 리스트

            List<Sprite> headsetAllItem = new List<Sprite>();//중간부터 접근하기 위해서 List선언
            int forBreakCount =0;

            for (int i = 0; i < equipInfo.Count; i++ )
            {
               

                    switch (int.Parse(equipInfo[i].itemIndex.ToString()[index].ToString()))
                    {

                        case (int)itemType.Headset:

                        Debug.Log(equipInfo[i].ItemName);

                        if (equipInfo[i].unlock == true)//해금된것 먼저 앞열에 넣어주기
                            headsetAllItem.Add(imgManager.arrEquipItem[i]);


                        forBreakCount++;
                        break;

                        case (int)itemType.Controller:

                        headsetAllItem.Add(imgManager.arrEquipItem[i]);
                        forBreakCount++;

                        break;
                        case (int)itemType.Chair:

                        headsetAllItem.Add(imgManager.arrEquipItem[i]);
                        forBreakCount++;

                        break;
                        case (int)itemType.Uniform:

                        headsetAllItem.Add(imgManager.arrEquipItem[i]);
                        forBreakCount++;
                        break;

                    }

                    if (forBreakCount >= 5)
                    {
                        break;
                    }

            }

            for (int aac = 0; aac < headsetAllItem.Count; aac++)
                Debug.Log(headsetAllItem[aac] + "이기맞나");

            //    for (int i = 0; i < equipInfo.Count; i++)
            //{
            //    if (equipInfo[i].itemIndex.ToString()[index + 1] == 1)
            //        Debug.Log(equipInfo[i].itemIndex.ToString()[index + 1]);
            //}
            //Debug.Log(arrWearUniform[index]);
            equipChangeBG.gameObject.SetActive(true);



        }

        public float GetCoolTimeBonus()//선수 전체의 쿨타임 지속 버프
        {
            float coolTimeBo = coolTimeBonus;

            return coolTimeBo;
        }

        public float GetAttackBonus()//선수 전체의 공격력 지속 버프
        {
            float attackBo = attackBonus;
            return attackBo;
        }

        public float GetDefenceBonus()//선수 전체의 방어 지속 버프
        {
            int defenceBo = defenceBonus;
            return defenceBo;

        }

        public float GetAttackSpeedBonus()//선수 전체의 공격속도 지속 버프
        {
            int attackSpeedBo = attackSpeedBonus;

            return attackSpeedBo;
        }

        public List<int> GetChampTypeBonus()//선수 포지션 보너스
        {
            List<int> listTypeBonus = new List<int>();

            for (int i = 0; i < quChampTypeBonus.Count; i++)
            {
                if (quChampTypeBonus.Peek() > 0)
                {
                    listTypeBonus.Add(quChampTypeBonus.Dequeue());

                }

            }

            return listTypeBonus;
        }

        public int GetChampStatBonus(int Index)//찾고있는 챔피언의 이름만 넣으면 스텟을 찾을 수 있음
        {
            int champBonus = 0;

            for (int i = 0; i < equipInfo.Count; i++)
            {
                try
                {
                    champBonus += equipInfo[i].dicChampMastery[Index];
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);

                }

            }

            return champBonus;
        }




    }



}
