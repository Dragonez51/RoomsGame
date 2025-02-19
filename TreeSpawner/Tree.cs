using UnityEngine;

public class Tree : MonoBehaviour
{
    private TreeNode root;

    private bool hit = false;

    public Tree() { root = null; }

    public void addStartingRooms(TreeNode firstRoom, TreeNode secondRoom) 
    {
        root = firstRoom;
        root.front = secondRoom;
    }

    public void add(TreeNode next)
    {

        if (root == null) 
        {
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
    }

    public void addAfter(TreeNode parent, TreeNode child, char door) 
    {
        if (door == 'L' || door == 'l')
        {
            parent.left = child;
            parent.doorL = false;
        }
        else if (door == 'F' || door == 'f')
        {
            parent.front = child;
            parent.doorF = false;
        }
        else if (door == 'R' || door == 'r')
        {
            parent.right = child;
            parent.doorR = false;
        }
        else 
        {
            Debug.Log("Wrong door as parameter!");
        }

        child.parent = parent;
    }

    public void spawnAll(float scale) { spawnAll(this.root, scale); }
    private void spawnAll(TreeNode node, float scale)
    {
        if (node == null) { return; }

        GameObject clone = Instantiate(node.room.roomRef, node.position, new Quaternion(0, 0, 0, 1));
        clone.transform.Rotate(Vector3.down, -90 * node.rotation);
        clone.transform.localScale = new Vector3(scale, scale, scale);

        spawnAll(node.left, scale);
        spawnAll(node.front, scale);
        spawnAll(node.right, scale);

    }

    private TreeNode comparePositionsRec(TreeNode node, Vector3 position, float offset)
    {
        if (node == null) { return null; }

        comparePositionsRec(node.left, position, offset);
        comparePositionsRec(node.front, position, offset);
        comparePositionsRec(node.right, position, offset);

        Vector3 temp = node.position;
        if ((position.x >= temp.x - offset && position.x <= temp.x + offset)
                                           &&
            (position.z >= temp.z - offset && position.z <= temp.z + offset))
        {
            hit = true;
            return null;
        }

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
            return false;
        }
    }

    public void backtrack(Room room0, float roomOffset)
    {
        //Debug.Log("=========== Starting backtrack ===========");
        backtrack(root.front, roomOffset, room0);
    }
    private void backtrack(TreeNode node, float roomOffset, Room room0) 
    {
        if (node == null) { return; }

        backtrack(node.left, roomOffset, room0);
        backtrack(node.front, roomOffset, room0);
        backtrack(node.right, roomOffset, room0);

        //Debug.Log(node);
        if (node.doorL)
        {
            int rotation = node.rotation;
            Vector3 position = new Vector3();
            if (rotation == 0)
            {
                position = new Vector3(node.position.x - roomOffset, node.position.y, node.position.z);
            }
            else if (rotation == 1)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z + roomOffset);
            }
            else if (rotation == 2 || rotation == -2)
            {
                position = new Vector3(node.position.x + roomOffset, node.position.y, node.position.z);
            }
            else if (rotation == -1)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z - roomOffset);
            }
            else { Debug.Log("rotation out of bounds!"); }

            bool compare = comparePositionsRec(position, 0.01f);
            //Debug.Log("doorL comparing: " + position + "; result: "+compare);
            if (!compare)
            {
                TreeNode endRoom = new TreeNode(room0, position, rotation - 1);
                node.left = endRoom;
                node.doorL = false;
            }
        }

        if (node.doorF)
        {
            int rotation = node.rotation;
            Vector3 position = new Vector3();
            if (rotation == 0)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z + roomOffset);
            }
            else if (rotation == 1)
            {
                position = new Vector3(node.position.x + roomOffset, node.position.y, node.position.z);
            }
            else if (rotation == 2 || rotation == -2)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z - roomOffset);
            }
            else if (rotation == -1)
            {
                position = new Vector3(node.position.x - roomOffset, node.position.y, node.position.z);
            }
            else { Debug.Log("rotation out of bounds!"); }

            bool compare = comparePositionsRec(position, 0.01f);
            //Debug.Log("doorF comparing: " + position + "; result: "+compare);
            if (!compare)
            {
                TreeNode endRoom = new TreeNode(room0, position, rotation);
                node.front = endRoom;
                node.doorF = false;
            }
        }

        if (node.doorR) 
        {
            int rotation = node.rotation;
            Vector3 position = new Vector3();
            if (rotation == 0)
            {
                position = new Vector3(node.position.x + roomOffset, node.position.y, node.position.z);
            }
            else if (rotation == 1)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z - roomOffset);
            }
            else if (rotation == 2 || rotation == -2)
            {
                position = new Vector3(node.position.x - roomOffset, node.position.y, node.position.z);
            }
            else if (rotation == -1)
            {
                position = new Vector3(node.position.x, node.position.y, node.position.z + roomOffset);
            }
            else { Debug.Log("rotation out of bounds!"); }

            bool compare = comparePositionsRec(position, 0.01f);
            //Debug.Log("doorR comparing: " + position + "; result: "+compare);
            if (!compare)
            {
                TreeNode endRoom = new TreeNode(room0, position, rotation + 1);
                node.right = endRoom;
                node.doorR = false;
            }
        }
    }

    public void printAll()
    {
        printAll(root);
    }
    private void printAll(TreeNode node) 
    {
        if (node == null) { return; }

        printAll(node.left);
        printAll(node.front);
        printAll(node.right);

        Debug.Log(node);
    }

    public TreeNode getRoot() { return root; }
}
