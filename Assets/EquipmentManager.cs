using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
namespace Framework.UI
{
   

    [System.Serializable]
    public class EquipInfoCollect
    {
        public List<EquipInfo> equipInfo;
        public equipExplain equipExplain;
    }

    [System.Serializable]
    public class EquipInfo//�÷��̾��� �� �� ����
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
    public class equipExplain//�÷��̾��� �� �� ����
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
        private ItemInfoBox[] itemInfoBox  = new ItemInfoBox[4];
        [SerializeField]
        private GameObject equipChangeBG;
        public void Init()
        {
            
            equipInfoText = Resources.Load("equipInfo") as TextAsset;
            equipInfoCollet = JsonConvert.DeserializeObject<EquipInfoCollect>(equipInfoText.text);
            var equipInfo = equipInfoCollet.equipInfo;
            var equipExplain = equipInfoCollet.equipExplain;
            var characterState = GameObject.Find("CharaceterState").GetComponent<CharacterJsonRead>().characterStateList.characterState;

            int[] arrWearUniform = {playerInformation.GetPlayerInfo().wearHeadset, playerInformation.GetPlayerInfo().wearController,
                playerInformation.GetPlayerInfo().wearChair, playerInformation.GetPlayerInfo().wearUniform };

            for(int i = 0; i <arrWearUniform.Length; i++)
            {
                equipSetting(equipInfo, equipExplain, characterState, arrWearUniform[i], i);
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
                            if (z == 0)//���ݷ�
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";

                                attackBonus += equipInfo[i].arrBonusStatCount[z];
                                
                            }

                            if (z == 1)//����
                            {

                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                defenceBonus += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == 2)//���ݼӵ�
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                attackSpeedBonus += equipInfo[i].arrBonusStatCount[z];
                            }

                            if (z == 3)//��Ÿ��
                            {
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];
                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + "%" + " ";
                                coolTimeBonus += equipInfo[i].arrBonusStatCount[z];
                            }


                            if (z == 4)//�迭 ���õ�
                            {   
                                for (int x = 0; x < equipInfo[equipIndex].arrTypeMastery.Length; x++)
                                {
                                    if (equipInfo[equipIndex].arrTypeMastery[x] > 0)
                                    {   
                                        arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMastery[x] +"+" + equipInfo[equipIndex].arrTypeMastery[x];
                                        //arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += " +" + equipInfo[i].arrBonusStatCount[z] + " ";
                                        //�극��ũ�� �ɾ����� �ʴ´� 2�� �̻� ������ �ֱ⶧��
                                    }

                                }

                            }

                            if (z == 5)//Ư�� è�Ǿ�
                            {
                                for (int x = 0; x < characterState.Length; x++)
                                {
                                    if (characterState[x].indexCharacter == equipInfo[i].dicChampMastery[])
                                    {
                                        arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += characterState[x].characterName;
                                        //�극��ũ�� �ɾ����� �ʴ´� 2�� �̻� ������ �ֱ⶧��
                                    }
                                }

                                arrEquipChild[equipIndex].GetChild(2).GetComponent<TextMeshProUGUI>().text += equipExplain.arrTypeMent[z];

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

        public float GetCoolTimeBonus()//���� ��ü�� ��Ÿ�� ���� ����
        {
            float coolTimeBo = coolTimeBonus;

            return coolTimeBo;
        }

        public float GetAttackBonus()//���� ��ü�� ���ݷ� ���� ����
        {
            float attackBo = attackBonus;
            return attackBo;
        }

        public float GetDefenceBonus()//���� ��ü�� ��� ���� ����
        {
            int defenceBo = defenceBonus;
            return defenceBo;

        }

        public float GetAttackSpeedBonus()//���� ��ü�� ���ݼӵ� ���� ����
        {
            int attackSpeedBo = attackSpeedBonus;

            return attackSpeedBo;
        }

        public List<int> GetChampTypeBonus()//���� ������ ���ʽ�
        {
            List<int> listTypeBonus = new List<int>();

            for (int i = 0; i<quChampTypeBonus.Count; i++)
            {
                if(quChampTypeBonus.Peek() > 0)
                {
                    listTypeBonus.Add(quChampTypeBonus.Dequeue());

                }

            }

            return listTypeBonus;
        }

        public int GetChampStatBonus(int Index)//ã���ִ� è�Ǿ��� �̸��� ������ ������ ã�� �� ����
        {
            var equipInfo = equipInfoCollet.equipInfo;

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
