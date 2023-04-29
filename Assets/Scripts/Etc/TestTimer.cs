using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TestTimer : MonoBehaviour
    {
        
        public string asdasd;

        public List<string> aaa = new List<string>();

        public List<string> bbb;


        private void Start()
        {            
            string[] aa = {"zero","one","two","three","four","five","six","seven","eight","nine"};
            string[] bb = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for(int i = 0; i< aa.Length; i++)
            {             
               asdasd = asdasd.Replace(aa[i], bb[i]);
               
            }

          

        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        Timer.Instance.ExecuteTimer(2f, OnEndWait);
        //    }
        //}

        //void OnEndWait()
        //{
        //    Debug.Log("OnEndWait");
        //}
    }
}