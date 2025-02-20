using UnityEngine;

public class NextRoom1
{
    private Room room;
    private Vector3 position;
    private NextRoom1 next;
    private int rotation;

    public bool doorL, doorF, doorR;

    public NextRoom1(Room room, Vector3 position, int rotation) 
    {
        this.room = room;
        this.position = position;
        this.rotation = rotation;

        //if (room.doorL) { doorL = true; } else { doorL = false; }
        //if (room.doorF) { doorF = true; } else { doorF = false; }
        //if (room.doorR) { doorR = true; } else { doorR = false; }

        doorL = room.doorL;
        doorF = room.doorF;
        doorR = room.doorR;
    }

    public NextRoom1 getNext() { return next; }

    public void setNext(NextRoom1 next) { this.next = next; }

    public Room getRoom() { return room; }

    public Vector3 getPosition() { return position; }

    public int getRotation() { return rotation; }

}