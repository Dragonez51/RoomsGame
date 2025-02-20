using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerScriptDynamicMap : MonoBehaviour
{
    private Map map;

    [SerializeField] private int mapSize;

    [SerializeField] private GameObject room0;
    [SerializeField] private GameObject roomL;
    [SerializeField] private GameObject roomF;
    [SerializeField] private GameObject roomR;
    [SerializeField] private GameObject roomLF;
    [SerializeField] private GameObject roomFR;
    [SerializeField] private GameObject roomLR;
    [SerializeField] private GameObject roomLFR;

    private float roomOffset = 7.9f;

    private Room RRoom0, RRoomL, RRoomF, RRoomR, RRoomLF, RRoomFR, RRoomLR, RRoomLFR;
    private Room[] rooms;

    void Start()
    {
        RRoom0 = new Room(room0, false, false, false);

        RRoomL = new Room(roomL, true, false, false);
        RRoomF = new Room(roomF, false, true, false);
        RRoomR = new Room(roomR, false, false, true);
        RRoomLF = new Room(roomLF, true, true, false);
        RRoomFR = new Room(roomFR, false, true, true);
        RRoomLR = new Room(roomLR, true, false, true);
        RRoomLFR = new Room(roomLFR, true, true, true);

        rooms = new Room[] { RRoom0, RRoomL, RRoomF, RRoomR, RRoomLF, RRoomFR, RRoomLR, RRoomLFR };

        generateMap();

        map.spawnAll();
    }

    void hardCodeMap() 
    {
        NextRoom1 firstRoom = new NextRoom1(new Room(room0, false, true, false), new Vector3(transform.position.x, transform.position.y, transform.position.z), 2);
        NextRoom1 secondRoom = new NextRoom1(new Room(roomL, false, true, false), new Vector3(firstRoom.getPosition().x, firstRoom.getPosition().y, firstRoom.getPosition().z + roomOffset), 0);
        NextRoom1 thirdRoom = new NextRoom1(new Room(roomF, false, true, false), new Vector3(secondRoom.getPosition().x - roomOffset, secondRoom.getPosition().y, secondRoom.getPosition().z), -1);
        map = new Map(firstRoom);

        map.addAtFront(secondRoom);

        map.addAtFront(thirdRoom);

        map.spawnAll();
    }

    void generateMap() 
    {
        NextRoom1 firstRoom = new NextRoom1(RRoom0, new Vector3(transform.position.x, transform.position.y, transform.position.z), 2);
        map = new Map(firstRoom);

        NextRoom1 generatedRoom = new NextRoom1(rooms[Random.Range(1, 8)], new Vector3(transform.position.x, transform.position.y, transform.position.z + roomOffset), 0);
        map.addAtFront(generatedRoom);

        int nextRotation = 0;

        int right = 0;
        int forward = 0;

        Room currentRoom = generatedRoom.getRoom();

        int roomCount = 2;

        bool backtracking = false;

        while (roomCount <= mapSize)
        {
            //Debug.Log(generatedRoom.getRoom().roomRef);
            if (generatedRoom == null) 
            {
                Debug.Log("generatedRoom == null");
                break;
            }
            currentRoom = generatedRoom.getRoom();

            if (currentRoom == RRoom0) 
            {
                generatedRoom = generatedRoom.getNext();
                if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                backtracking = true;
                continue;
            }
            if (currentRoom == RRoomL)                                                                           //      RoomL
            {
                if (!backtracking)
                {
                    int currentRotation = generatedRoom.getRotation();

                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }

                }
                else 
                {
                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

            }
            else if (currentRoom == RRoomF)                                                                      //      RoomF
            {
                if (!backtracking)
                {
                    int currentRotation = generatedRoom.getRotation();

                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;
                }
                else 
                {
                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

            }
            else if (currentRoom == RRoomR)                                                                       //      RoomR
            {
                if (!backtracking) 
                {
                    int currentRotation = generatedRoom.getRotation();

                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }
                }
                else 
                {
                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }
            }
            else if (currentRoom == RRoomLF)                                                                      //      RoomLF
            {
                int currentRotation = generatedRoom.getRotation();

                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }

                    generatedRoom.doorL = false;
                }
                else if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;

                    generatedRoom.doorF = false;
                }
                else
                {
                    //Debug.Log("RoomLF: Current room doesn't have any doors open!");
                    //break;

                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

                backtracking = false;
            }
            else if (currentRoom == RRoomFR)                                                                      //      RoomFR
            {
                int currentRotation = generatedRoom.getRotation();

                if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;

                    generatedRoom.doorF = false;
                }
                else if (generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }

                    generatedRoom.doorR = false;
                }
                else
                {
                    //Debug.Log("RoomFR: Current room doesn't have any doors open!");
                    //break;

                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

                backtracking = false;
            }
            else if (currentRoom == RRoomLR)                                                                      //      RoomLR
            {
                int currentRotation = generatedRoom.getRotation();

                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }

                    generatedRoom.doorL = false;
                }
                else if (generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }

                    generatedRoom.doorR = false;
                }
                else
                {
                    //Debug.Log("RoomLR: Current room doesn't have any doors open!");
                    //break;

                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

                backtracking = false;
            }
            else if (currentRoom == RRoomLFR)                                                                      //      RoomLFR
            {
                int currentRotation = generatedRoom.getRotation();

                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || generatedRoom.getRotation() == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }

                    generatedRoom.doorL = false;
                }
                else if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || generatedRoom.getRotation() == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;

                    generatedRoom.doorF = false;
                }
                else if (generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || generatedRoom.getRotation() == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }

                    generatedRoom.doorR = false;
                }
                else
                {
                    //Debug.Log("RoomLFR: Current room doesn't have any doors open!");
                    //break;

                    generatedRoom = generatedRoom.getNext();
                    if (currentRoom == null) { Debug.Log("No more rooms to clear!"); break; }
                    continue;
                }

                backtracking = false;
            }
            else
            {
                Debug.Log("Room definition not found!");
                break;
            }

            Vector3 lastPosition = generatedRoom.getPosition();

            generatedRoom = new NextRoom1(rooms[Random.Range(0, 8)], new Vector3(lastPosition.x + (roomOffset * right), lastPosition.y, lastPosition.z + (roomOffset * forward)), nextRotation);

            if (map.compare(generatedRoom.getPosition(), 0.0001f)) 
            { 
                Debug.Log("Room already spawned here!");

                //generatedRoom = generatedRoom.getNext();
                Debug.Log(generatedRoom.getNext());
                Debug.Log(generatedRoom.getRoom().roomRef);
                map.writeAll();
                backtracking = true;
                continue;
            } 
            else 
            { 
                Debug.Log(generatedRoom.getRoom().roomRef+" is alone"); 
            }

            map.addAtFront(generatedRoom);

            roomCount++;

            if (roomCount >= mapSize) 
            {
                Debug.Log("Ran out of rooms!");
                break;
            }

            currentRoom = generatedRoom.getRoom();

            nextRotation = 0;

            right = 0;
            forward = 0;

        }

        Debug.Log(roomCount);
    }
}