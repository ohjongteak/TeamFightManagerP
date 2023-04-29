using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PracticeQueue : MonoBehaviour
{
    public LinkedList<int> bb = new LinkedList<int>();

    public List<int> aaaa = new List<int>();
    // Start is called before the first frame update
    void Awake()
    {
        LinkedList<string> list = new LinkedList<string>();
        list.AddLast("Apple");
        list.AddLast("Banana");
        list.AddLast("Lemon");
        


        //LinkedListNode<string> node = list.Find("Banana");
        //LinkedListNode<string> newNode = new LinkedListNode<string>("Grape");

        // 새 Grape 노드를 Banana 노드 뒤에 추가
        //list.AddAfter(node, newNode);

        // 리스트 출력
        //list.ToList<string>().ForEach(p =>Debug.Log(p));

        // Enumerator 리스트 출력 

        //if (aaaa[14] == throw new IndexOutOfRangeException())
        //{

        //}


        //foreach (var aa in aaaa)
        //{
        //    Debug.Log(aa);
        //}

      

        

    

    }

   
   
}
