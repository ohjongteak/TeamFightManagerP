using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public int data;

    public TreeNode Left;
    public TreeNode Right;

}

public class Tree: MonoBehaviour
{
  

    public TreeNode[] Node =new TreeNode[15];

   

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Node.Length);
       
       for (int i = 0; i<Node.Length; i++)
       {
            Node[i] = new TreeNode();
       }

        for(int i = 1; i < Node.Length; i++)
        {
            Node[i].data = i;
            Node[i].Left = null;
            Node[i].Right = null;
            
        }

        for (int i = 1; i < Node.Length; i++)
        {
            if (i % 2 == 0)
            {
                Node[i].Left = Node[i];
            }
            else
            {   
                Node[i].Right = Node[i];
            }

        }

        MidFirst(Node[1]);
    }

    public void LeftFirst(TreeNode Node)
    {
        
            LeftFirst(Node.Left);
            Debug.Log(Node.data);
            LeftFirst(Node.Right);
        
    }

    public void MidLast(TreeNode Node)
    {
        
            MidLast(Node.Left);
            MidLast(Node.Right);
            Debug.Log(Node.data);
        
        
    }

    public void MidFirst(TreeNode Node)
    {
       
           // Debug.Log(Node.data);
            MidFirst(Node.Left);
            MidFirst(Node.Right);
        
    }
}
