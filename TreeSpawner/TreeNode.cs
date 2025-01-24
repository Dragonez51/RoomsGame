using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public Room room;
    public Vector3 position;
    public int rotation;

    public bool doorL, doorF, doorR;

    public TreeNode parent;

    public TreeNode left;
    public TreeNode front;
    public TreeNode right;

    public TreeNode(Room room, Vector3 position, int rotation) 
    {
        this.room = room;
        this.position = position;
        this.rotation = rotation;

        doorL = room.doorL;
        doorF = room.doorF;
        doorR = room.doorR;
    }
}
