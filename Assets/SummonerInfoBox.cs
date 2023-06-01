using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{ 
    public class SummonerInfoBox : MonoBehaviour
    {

        public TrainingSummonerSort trainingSummonerSort;
        [SerializeField]
        public TrainingManager trainingManager;
       

        // Start is called before the first frame update
        private void OnMouseEnter() //마우스 오버 시 아웃라인 켜기
        {
            this.GetComponent<Outline>().enabled = true;
        }

        private void OnMouseExit() //마우스 나갈 시 아웃라인 끄기
        {
            this.GetComponent<Outline>().enabled = false;
        }

        public void ChangeColor() 
        {
            Transform[] temp = new Transform[this.transform.parent.childCount];

            Color Green;
            ColorUtility.TryParseHtmlString("#68ff01", out Green);

            Color Black;
            ColorUtility.TryParseHtmlString("#121319", out Black);

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = this.transform.parent.GetChild(i);
                temp[i].GetComponent<Image>().color = Black;
            }


            this.GetComponent<Image>().color = Green;

        }

        public string GetThisName()//인덱스 넘겨주기 위한 함수
        {
            return this.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;  
        }

        public void PassThisName()
        {
            trainingSummonerSort.SummonerBox(GetThisName());
            trainingManager.SummonerBox(GetThisName());
        }

    }
}
