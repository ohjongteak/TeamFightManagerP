using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private static GameObject objParent;

    [SerializeField] private GameObject poolingObjectPrefab;

    Queue<Bullet> quPoolingObject = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        objParent = new GameObject(poolingObjectPrefab.name);
        Initialize(5);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            quPoolingObject.Enqueue(CreateNewObject());
        }
    }

    // Queue�� ������Ʈ �߰�
    private Bullet CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(objParent.transform);
        return newObj;
    }

    // Queue�� ���� ������Ʈ ���
    public Bullet GetObject()
    {
        if (Instance.quPoolingObject.Count > 0)
        {
            var obj = Instance.quPoolingObject.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    // ����� ������Ʈ �ٽ� Queue�� �߰�
    public void ReturnObject(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(objParent.transform);
        Instance.quPoolingObject.Enqueue(obj);
    }
}
