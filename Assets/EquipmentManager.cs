using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Framework.UI
{
    public enum EquipStateType
    {
        None,
        Attack,
        Defend,
        Cooltime,
        TypeMastery,
        AttackSpeed

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

    }

   
    public class EquipmentManager : MonoBehaviour
    {
        [SerializeField]
        private Image wearHeadset;
        [SerializeField]
        private Image wearController;
        [SerializeField]
        private Image wearChair;
        [SerializeField]
        private Image wearUniform;
        [SerializeField]
        private ImageManager imgManager;

        public EquipInfoCollect equipInfoCollet = new EquipInfoCollect();
        public TextAsset equipInfoText;
        public PlayerInformation playerInformation;

        public void Init()
        {
            equipInfoText = Resources.Load("equipInfo") as TextAsset;
            equipInfoCollet = JsonUtility.FromJson<EquipInfoCollect>(equipInfoText.text);



            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearHeadset.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearHeadset[i] == true)
                {
                    wearHeadset.sprite = imgManager.arrHeadset[i];
                    break;
                }

            }

            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearUniform.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearUniform[i] == true)
                {
                    wearUniform.sprite = imgManager.arrUniform[i];
                    break;
                }

            }

            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearController.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearController[i] == true)
                {
                    wearController.sprite = imgManager.arrController[i];
                    break;
                }

            }

            for (int i = 0; i < playerInformation.GetPlayerInfo().arrWearChair.Length; i++)
            {
                if (playerInformation.GetPlayerInfo().arrWearChair[i] == true)
                {
                   wearChair.sprite = imgManager.arrChair[i];
                    break;
                }

            }

        }

        private void Start()
        {
            Init();

        }
    }

}
