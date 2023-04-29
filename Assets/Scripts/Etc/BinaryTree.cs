using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeNode
{
    public int Data;
    public BinaryTreeNode Left;
    public BinaryTreeNode Right;
    public BinaryTreeNode(int data)
    {
        this.Data = data;
    }

}

// 이진 트리 클래스
public class BinaryTree :MonoBehaviour
{
    public BinaryTreeNode Root;
    // 트리 데이타 출력 예
    public void PreOrderTraversal(BinaryTreeNode node)
    {
        if (node == null) return;

        Debug.Log(node.Data);
        PreOrderTraversal(node.Left);
        PreOrderTraversal(node.Right);
    }

    public void PreOrderTraversal2(BinaryTreeNode node)
    {
        if (node == null) return;
     
        PreOrderTraversal(node.Left);
        Debug.Log(node.Data);
        PreOrderTraversal(node.Right);
    }

    public void PreOrderTraversal3(BinaryTreeNode node)
    {
        if (node == null) return;
        
        PreOrderTraversal(node.Left);
        PreOrderTraversal(node.Right);
        Debug.Log(node.Data);
    }

    private void Start()
    {
        BinaryTree btree = new BinaryTree();
        btree.Root = new BinaryTreeNode(1);
        btree.Root.Left = new BinaryTreeNode(2);
        btree.Root.Right = new BinaryTreeNode(3);
        btree.Root.Left.Left = new BinaryTreeNode(4);
        btree.Root.Left.Right = new BinaryTreeNode(7);
        btree.Root.Right.Right = new BinaryTreeNode(5);
        btree.Root.Left.Left.Left = new BinaryTreeNode(6);
        btree.PreOrderTraversal(btree.Root);
    }

}

