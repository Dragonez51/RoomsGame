using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    private NextRoom1 head;
    private int size;

    public Map(NextRoom1 head) { this.head = head; size = 1; }

    public void addAtFront(NextRoom1 add) 
    { 
        add.setNext(head); 
        head = add; 
        size++; 
    }

    public NextRoom1 getHead() { return head; }

    public void spawnAll() 
    {
        NextRoom1 temp = head;

        while (temp != null) 
        {
            GameObject clone = Instantiate(temp.getRoom().roomRef, temp.getPosition(), new Quaternion(0,0,0,1));
            clone.transform.Rotate(Vector3.down, -90 * temp.getRotation());
            temp = temp.getNext();
        }
    }

    public void writeAll() 
    {
        NextRoom1 temp = head;

        while (temp != null) 
        {
            Debug.Log(temp.getRoom().roomRef);
            temp = temp.getNext();
        }

        Debug.Log(size);
    }

    public bool compare(Vector3 position1, float offset) 
    {
        NextRoom1 temp = head;

        while (temp != null)
        {
            Debug.Log("Comparing: "+position1+" to: "+temp.getPosition());
            //if (position1.Equals(temp.getPosition())) { return true; } else { temp = temp.getNext(); }
            Vector3 position2 = temp.getPosition();

            bool x = false; 
            bool z = false;

            if (position1.x >= position2.x - offset && position1.x <= position2.x + offset) { x = true; }
            if (position1.z >= position2.z - offset && position1.z <= position2.z + offset) { z = true; }

            if (x && z) { return true; } else { temp = temp.getNext(); }
        }

        return false;
    }
}