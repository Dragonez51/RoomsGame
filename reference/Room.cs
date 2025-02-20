using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public GameObject roomRef;
    public bool doorL;
    public bool doorF;
    public bool doorR;

    public bool DoneDoorL, DoneDoorF, DoneDoorR;

    public Room(GameObject roomRef, bool doorL, bool doorF, bool doorR) 
    {
        this.roomRef = roomRef;
        this.doorL = doorL;
        this.doorF = doorF;
        this.doorR = doorR;
        DoneDoorL = false;
        DoneDoorF = false;
        DoneDoorR = false;
    }

    public override string ToString() 
    {
        if (doorL && !doorF && !doorR)
        {
            return "RoomL";
        }
        if (!doorL && doorF && !doorR)
        {
            return "RoomF";
        }
        if (!doorL && !doorF && doorR)
        {
            return "RoomR";
        }
        if (doorL && doorF && !doorR)
        {
            return "RoomLF";
        }
        if (!doorL && doorF && doorR)
        {
            return "RoomFR";
        }
        if (doorL && !doorF && doorR)
        {
            return "RoomLR";
        }
        if (doorL && doorF && doorR)
        {
            return "RoomLFR";
        }
        return "Room0";
    }
}
