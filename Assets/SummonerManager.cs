using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
namespace Framework.UI
{
    [System.Serializable]
    public class SummonerCharacter
    {
        public SummonerCharacterState[] summonerCharacterState;

    }
    [System.Serializable]
    public class SummonerCharacterState
    {
        public string name;
        public int atttack;
        public int defend;
        public int[] mainHero;
        public int hairIndex;

    }

    public class SummonerManager : MonoBehaviour
    {
        [SerializeField]
        private ImageManager imgManager;
        [SerializeField]
        private GameObject summonerPrefab;
        [SerializeField]
        SummonerCharacter summonerCharacter;
        


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.H))
            {
                Init();
            }
        }
        public void Init()
        {
            SummonerLoadManager summonerLoadManager = GameObject.Find("SummonerLoadManager").GetComponent<SummonerLoadManager>();

            summonerCharacter = summonerLoadManager.summonerCharacterList;
            var summonerStateList = summonerLoadManager.summonerCharacterList.summonerCharacterState;
            // for(int i =0; i < summonerLoadManager.summonerCharacter.summonerCharacterState.Length; i++)

            if (summonerStateList.Length > 0)//저장된 세이브 파일이 있다면
            {
                summonerCharacter.summonerCharacterState = new SummonerCharacterState[summonerStateList.Length];
                for (int i = 0; i < summonerStateList.Length; i++)
                {
                    summonerCharacter.summonerCharacterState[i] = new SummonerCharacterState();
                    summonerCharacter.summonerCharacterState[i] = summonerStateList[i];
                }
            }
            else//조건: 선수단의 정보(세이브Json파일이)가 없으면 선수단 3명 랜덤 생성 랜덤 아직 미구현
            {
                //소환사 랜덤 생성
                summonerCharacter.summonerCharacterState = new SummonerCharacterState[3];
                summonerCharacter.summonerCharacterState[0] = new SummonerCharacterState();
                summonerCharacter.summonerCharacterState[1] = new SummonerCharacterState();
                summonerCharacter.summonerCharacterState[2] = new SummonerCharacterState();

                summonerCharacter.summonerCharacterState[0].name = "육종택";
                summonerCharacter.summonerCharacterState[0].atttack = 9;
                summonerCharacter.summonerCharacterState[0].defend = 6;

                summonerCharacter.summonerCharacterState[1].name = "칠종택";
                summonerCharacter.summonerCharacterState[1].atttack = 8;
                summonerCharacter.summonerCharacterState[1].defend = 7;

                summonerCharacter.summonerCharacterState[2].name = "팔팔종택";
                summonerCharacter.summonerCharacterState[2].atttack = 7;
                summonerCharacter.summonerCharacterState[2].defend = 8;

                 summonerStateList = summonerCharacter.summonerCharacterState;
            }

        }

        public SummonerCharacter GetSummonerCharacter()
        {
            return summonerCharacter;
        }
    }
}
