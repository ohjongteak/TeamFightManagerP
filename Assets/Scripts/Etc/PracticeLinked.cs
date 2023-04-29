using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PracticeLinked<T> : MonoBehaviour
{
    public T Data;
    public PracticeLinked<T> Next { get; set; }
    public PracticeLinked<T> Prev { get; set; }

    public PracticeLinked<T> head;


  public PracticeLinked(T Data, PracticeLinked<T> prev, PracticeLinked<T>next)
  {
        this.Prev = prev;
        this.Next = next;
        this.Data = Data;
  }

    public PracticeLinked(T data)
        : this(data, null, null) { }

  public void Remove(PracticeLinked<T> RemoveNode)
  {
        if (RemoveNode == null || head == null)
            return;

        if(RemoveNode == head)
        {
            head = head.Next;
            if (head != null)
                head.Prev = null;
        }
        else
        {
            RemoveNode.Prev.Next = RemoveNode.Next;
            if (RemoveNode.Next != null)
                RemoveNode.Next.Prev = RemoveNode.Prev;
        }
        RemoveNode = null;
  }

  public void addFirst(PracticeLinked<T> NewNode)
  {
        if (NewNode == null)
            return;

        if (head == null)
            head = NewNode;
        else
        {
            var current = head;

            while (current != null && current.Next != null)
                current = current.Next;

            current.Next = NewNode;
            NewNode.Prev = current;
            NewNode.Next = null;

        }
  }

   public void AddInsert(PracticeLinked<T> NewNode, PracticeLinked<T> current)
   {
        if (NewNode == null || current == null || head == null)
            throw new InvalidOperationException();

        NewNode.Next = current.Next;
        current.Next.Prev = NewNode;
        current.Next = NewNode;
        NewNode.Prev = current;
        
   }

}
