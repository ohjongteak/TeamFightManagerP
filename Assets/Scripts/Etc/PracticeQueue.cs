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

        // �� Grape ��带 Banana ��� �ڿ� �߰�
        //list.AddAfter(node, newNode);

        // ����Ʈ ���
        //list.ToList<string>().ForEach(p =>Debug.Log(p));

        // Enumerator ����Ʈ ��� 

        //if (aaaa[14] == throw new IndexOutOfRangeException())
        //{

        //}


        //foreach (var aa in aaaa)
        //{
        //    Debug.Log(aa);
        //}

      

        

    

    }

   
   
}
