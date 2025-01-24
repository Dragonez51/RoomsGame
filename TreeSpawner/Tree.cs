using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Tree : MonoBehaviour
{
    private TreeNode root;
    private TreeNode lastRoom;
    //private TreeNode firstRoom;

    private bool hit = false;

    public Tree() { this.root = null; }

    public void add(TreeNode next)
    {

        if (this.root == null) 
        {
            lastRoom = next;
            root = next;
            return;
        }

        TreeNode parent = null;
        TreeNode child = root;

        while (child!=null) 
        {
            parent = child;

            if (child.doorL)
            {
                child = child.left;
            }
            else if (child.doorF)
            {
                child = child.front;
            }
            else
            {
                child = child.right;
            }
        }

        if (parent.doorL)
        {
            parent.left = next;
            next.parent = parent;
            parent.doorL = false;
        }
        else if (parent.doorF)
        {
            parent.front = next;
            next.parent = parent;
            parent.doorF = false;
        }
        else
        {
            parent.right = next;
            next.parent = parent;
            parent.doorR = false;
        }

        lastRoom = next;
    }

    private void spawnAll(TreeNode node) 
    {
        if (node == null) { return; }

        GameObject clone = Instantiate(node.room.roomRef, node.position, new Quaternion(0, 0, 0, 1));
        clone.transform.Rotate(Vector3.down, -90 * node.rotation);

        spawnAll(node.left);
        spawnAll(node.front);
        spawnAll(node.right);

    }

    private TreeNode comparePositionsRec(TreeNode node, Vector3 position, float offset)
    {
        if (node == null) { return null; }

        Vector3 temp = node.position;
        if ((position.x >= temp.x - offset && position.x <= temp.x + offset)
                                           &&
            (position.z >= temp.z - offset && position.z <= temp.z + offset))
        {
            hit = true;
            return null;
        }

        comparePositionsRec(node.left, position, offset);
        comparePositionsRec(node.front, position, offset);
        comparePositionsRec(node.right, position, offset);

        return null;

    }
    public bool comparePositionsRec(Vector3 position, float offset)
    {
        comparePositionsRec(root, position, offset);

        if (hit)
        {
            hit = false;
            return true;
        }
        else 
        {
            hit = false;
            return false;
        }
    }

    private TreeNode getMostLeft(TreeNode node) 
    {
        if (node == null) 
        {
            return null;
        }

        while (node.left != null) 
        {
            node = node.left;
        }

        return node;
    }

    public void spawnAll() { spawnAll(this.root); }

    public TreeNode getLastRoom() { return lastRoom; }
    public TreeNode getRoot() { return root; }
    //public TreeNode getFirstRoom() { return firstRoom; }

    //public void setFirstRoom(TreeNode firstRoom) { this.firstRoom = firstRoom; }
}
