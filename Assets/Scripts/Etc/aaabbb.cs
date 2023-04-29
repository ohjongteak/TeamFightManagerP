using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class aaabbb : MonoBehaviour
{
    public int[] aa = new int[2];

    public int[] bb;

    // Start is called before the first frame update

    public List<int> listcc = new List<int>();

    public Queue<int> aas = new Queue<int>();
    // Update is called once per frame

    Hashtable hash = new Hashtable();
    private void Start()
    {
        //bb = aa;
        //aa = new int[4];
        //aa = bb;
        //listcc.Remove(2);
        //listcc.Clear();

        hash.Add("d", "s");
        Debug.Log(hash.Count);

    }
}
