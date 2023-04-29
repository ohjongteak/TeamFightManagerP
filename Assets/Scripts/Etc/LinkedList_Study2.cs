using System;
using UnityEngine;

public class LinkedList_Study2<T> :MonoBehaviour //���� ��ũ ����Ʈ ����
{
    public T Data { get; set; }
    public LinkedList_Study2<T> Prev { get; set; }
    public LinkedList_Study2<T> Next { get; set; }

    public LinkedList_Study2(T data, LinkedList_Study2<T> prev, LinkedList_Study2<T> next)
    {
        this.Data = data;
        this.Prev = prev;
        this.Next = next;
    }

    public LinkedList_Study2(T data)
        : this(data, null, null) { }

    private LinkedList_Study2<T> head;

    public void Add(LinkedList_Study2<T> newNode) //�� ��� �߰�.
    {
        if (head == null) head = newNode; //����Ʈ�� ��������� head�� �� ��� �Ҵ�
        else //��� ���� ���� ��
        {
            var current = head;
            while (current != null && current.Next != null)
            current = current.Next;
            current.Next = newNode;
            newNode.Prev = current;
            newNode.Next = null;
        }
    }
    
    public void AddAfter(LinkedList_Study2<T> current, LinkedList_Study2<T> newNode) //�� ��带 �߰��� ����
    {
        if (head == null || current == null || newNode == null)
            throw new InvalidOperationException();
        newNode.Next = current.Next;
        current.Next.Prev = newNode;
        newNode.Prev = current;
        current.Next = newNode;
    }

    public void Remove(LinkedList_Study2<T> removeNode) //Ư�� ��� ����
    {
        if (head == null || removeNode == null) return;
        //���� ��尡 ù ����̸�
        if (removeNode == head)
        {
            head = head.Next;
            if (head != null) 
                head.Prev = null;
        }
        else//ù ��尡 �ƴϸ� prev���� Next��带 ����
        {
            removeNode.Prev.Next = removeNode.Next;
            if (removeNode.Next != null)
                removeNode.Next.Prev = removeNode.Prev;
        }
        removeNode = null;
    }

    public LinkedList_Study2<T> GetNode(int index) //������ ��ġ�� ��� ��ȯ
    {
        var current = head;
        for (int i = 0; i < index && current != null; i++)
        {
            current = current.Next;
        }
        return current;
    }

    public int Count()
    {
        int cnt = 0;
        var current = head;
        while (current != null)
        {
            cnt++;
            current = current.Next;
        }
        return cnt;
    }

   

}

