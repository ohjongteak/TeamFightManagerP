using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
[Serializable]
public class pra
{

   public pppuuu aa;
}

[Serializable]

public class pppuuu
{
    public Dictionary<int, int> pp;
    public int nonono;
    public List<int> yesyes;
}

public class PracticeQueue : MonoBehaviour
{
    //public LinkedList<int> bb = new LinkedList<int>();

    //public List<int> aaaa = new List<int>();
    //// Start is called before the first frame update

    //public Queue<int> ququ = new Queue<int>();

    //[SerializeField]
    //public List<(int, int)> tttt = new List<(int, int)>();

    //public Dictionary<string, int> ll;

    //[SerializeField]
    public pra pra;
    void Awake()
    {
        //tttt.Add((-1,01));
        //tttt.Add((02, 12));
        
        LinkedList<string> list = new LinkedList<string>();
        //list.AddLast("Apple");
        //list.AddLast("Banana");
        //list.AddLast("Lemon");
        //ququ.Enqueue(10);

        pra = new pra();
        pra.aa = new pppuuu();
        pra.aa.pp = new Dictionary<int, int>();
        // pra = JsonUtility.FromJson<pra>("ss");

        pra.aa.pp.Add(1, 100);
        pra.aa.pp.Add(5, 200);

        var aa = File.ReadAllText(Application.dataPath + "/Resources" + "/testdic.json");
        pra = JsonConvert.DeserializeObject<pra>(aa);

        //Debug.Log(pra.aa.pp[1]);
        //string json = JsonConvert.SerializeObject(pra);
        //File.WriteAllText(Application.dataPath + "/Resources" + "/testdic.json", json);




        //Debug.Log(pra.aa.pp["전사"]);


        //pra.aa.yesyes = new List<int>();
        //pra.aa.yesyes.Add(1);
        //pra.aa.yesyes.Add(2);

        //pra.aa.nonono = 1;
        //string json = JsonConvert.SerializeObject(pra);
        //File.WriteAllText(Application.dataPath + "/Resources" + "/testdic.json",json);

        //JsonConvert.SerializeObject(json);
        //Debug.Log(tttt[0]);
        //Debug.Log(tttt[0].Item1);
        //Debug.Log(tttt[0].Item2);

        //Debug.Log(tttt[1]);
        //Debug.Log(tttt[1].Item1);
        //Debug.Log(tttt[1].Item2);



        //Debug.Log(aa);

        //ququ.Enqueue(20);
        //ququ.Enqueue(30);

        //Debug.Log(ququ.Peek());


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
