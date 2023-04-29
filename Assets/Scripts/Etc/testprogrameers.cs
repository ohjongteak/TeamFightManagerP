using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testprogrameers : MonoBehaviour
{
    public string[] report;
    public string[] id_list;

    public int[] reportCountList;
    public int[] answer;


    private void Start()
    {
        string[] ssss = new string[report.Length*2];
        answer = new int[id_list.Length];
        reportCountList = new int[id_list.Length];

        Debug.Log(ssss.Length);

        for (int i = 0; i < report.Length; i++)
        {
            string[] kk = new string[2];
            kk = report[i].Split(' ');
            
            ssss[i+i] = kk[0];
            //for(int z = 0; z < report[i].Length; z++)
            //{
            //    report[]
            //}
            ssss[i+i+1] = kk[1];  
        }

        

        for (int i = 0; i<id_list.Length;i++)
        {
            for (int z = 1; z < ssss.Length; z+=2)
            {
                if(id_list[i] == ssss[z])
                {
                    reportCountList[i]++;
                }
            }
        }

       
        for (int i = 0;i<reportCountList.Length; i++)
        {
           if(reportCountList[i] >= 2)
           {
              for (int z = 1; z < ssss.Length; z += 2)
              {
                if(ssss[z] == ssss[i+1])
                { 
                   answer[(z - 1) / 2] += 1;    
                }

              }  
           }
        }

     
    }   

    // Update is called once per frame

}
