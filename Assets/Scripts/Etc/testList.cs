using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testList : MonoBehaviour
{
    public int[] arraytoList;

    public const int MaxSize = 100;
    // Start is called before the first frame update

    public List<int> asdasd = new List<int>(); 
    void settingSize()
    {
        
        arraytoList = new int[MaxSize];
    }

    void AddData(int data)
    {
        int[] temparray = arraytoList;
        
        arraytoList = new int[arraytoList.Length + 1];

        for(int i = 0; i < temparray.Length; i++)
        {
            arraytoList[i+1] = temparray[i];
        }

        arraytoList[0] = data;

    }

    void ListCapacity()
    {
        for (int i = 0; i < float.PositiveInfinity; i++)
        {
            try
            {
                int temp = arraytoList[i];
            }
            catch
            {
                Debug.Log( i + "어레이투리스트의사이즈");
                break;

            }
        }
    }

    void RemoveAtIndex(int _index)
    {
        if (_index >= arraytoList.Length)
        {
            Debug.LogError("인덱스 초과");
        }
        else if(_index == arraytoList.Length -1)
        {
            int[] temp = new int[arraytoList.Length -1];

            for(int i = 0;i < temp.Length; i++)
            {
                temp[i] = arraytoList[i];
            }

            arraytoList = temp;
        }

        else
        {
            int[] temp = new int[arraytoList.Length - 1];

            for (int i = _index; i<arraytoList.Length -1; i++)
            {
                arraytoList[i] = arraytoList[i + 1];
            }

            for(int i = 0; i<temp.Length; i++)
            {
                temp[i] = arraytoList[i];
            }
            arraytoList = temp;
        }

    }

    void ReMoveData(int data)
    {
        for(int i = 0; i <arraytoList.Length; i++)
        {    
            if(arraytoList[i] == data)
            {
                int tempIndex = i;

                int[] temparray = new int[arraytoList.Length - 1];

                for (int z = tempIndex; z < arraytoList.Length - 1; z++)
                {
                    arraytoList[z] = arraytoList[z + 1];
                }

                for (int z = 0; z < temparray.Length; z++)
                {
                    temparray[z] = arraytoList[z];
                }
                arraytoList = temparray;

                break;
            }

        }
    }

    void InsertData(int _index, int _data)
    {
        if(_index >= arraytoList.Length)
        {
            Debug.LogError("인덱스 초과");
        }
        else
        {
            int[] temparray = new int[arraytoList.Length + 1];

            for (int z = 0; z < _index; z++)
            {
                temparray[z] = arraytoList[z];
            }

            for (int z = _index; z < temparray.Length - 1; z++)
            {
                temparray[z + 1] = arraytoList[z];
            }

            temparray[_index] = _data;

            arraytoList = temparray;

        }

    }

    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= MaxSize)
            {
                return -1;
            }
            else
            {
                // 정수배열로부터 값 리턴
                return arraytoList[index];
            }
        }
        set
        {
            if (!(index < 0 || index >= MaxSize))
            {
                // 정수배열에 값 저장
                arraytoList[index] = value;
            }
        }
    }


    void Clear()
    {
        arraytoList = new int[0];
    }

    private void Start()
    {
       
        InsertData(0, 100);

        //Debug.Log(arraytoList.Length);

      

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            asdasd.RemoveAt(1);
            Debug.Log(asdasd.Capacity);
            Debug.Log(asdasd.Count);

        }
    }

}
