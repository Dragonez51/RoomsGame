using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class TreeBasedSpawner : MonoBehaviour 
{

    [SerializeField] int roomAmount;

    [SerializeField] private GameObject room0;
    [SerializeField] private GameObject roomL;
    [SerializeField] private GameObject roomF;
    [SerializeField] private GameObject roomR;
    [SerializeField] private GameObject roomLF;
    [SerializeField] private GameObject roomFR;
    [SerializeField] private GameObject roomLR;
    [SerializeField] private GameObject roomLFR;

    private Room Room0, RoomL, RoomF, RoomR, RoomLF, RoomFR, RoomLR, RoomLFR;
    private Room[] rooms;

    private float roomOffset = 7.9f;

    Tree map;

    void Start()
    {
        Room0 = new Room(room0, false, false, false);

        RoomL = new Room(roomL, true, false, false);
        RoomF = new Room(roomF, false, true, false);
        RoomR = new Room(roomR, false, false, true);
        RoomLF = new Room(roomLF, true, true, false);
        RoomFR = new Room(roomFR, false, true, true);
        RoomLR = new Room(roomLR, true, false, true);
        RoomLFR = new Room(roomLFR, true, true, true);

        rooms = new Room[] { Room0, RoomL, RoomF, RoomR, RoomLF, RoomFR, RoomLR, RoomLFR };

        map = new Tree();

        generateMap();

        map.spawnAll();
    }

    private void generateMap()
    {
        int roomCount = 2;

        int forward = 0;
        int right = 0;

        int nextRotation = 0;

        TreeNode firstRoom = new TreeNode(Room0, new Vector3(transform.position.x, transform.position.y, transform.position.z), 2);
        map.add(firstRoom);
        //map.setFirstRoom(firstRoom);

        TreeNode generatedRoom = new TreeNode(rooms[Random.Range(1, 8)], new Vector3(firstRoom.position.x, firstRoom.position.y, firstRoom.position.z + roomOffset), 0);
        map.add(generatedRoom);

        Room currentRoom;

        while (roomCount < roomAmount)
        {
            currentRoom = generatedRoom.room;
            int currentRotation = generatedRoom.rotation;

            if (currentRoom == Room0)
            {
                break;
            }
            else if (currentRoom == RoomL)
            {
                if (currentRotation == 0) { right = -1; }
                else if (currentRotation == -1) { forward = -1; }
                else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                else if (currentRotation == 1) { forward = 1; }

                if (currentRotation <= -2) { nextRotation = 1; }
                else { nextRotation = currentRotation - 1; }
            }
            else if (currentRoom == RoomF)
            {
                if (currentRotation == 0) { forward = 1; }
                else if (currentRotation == -1) { right = -1; }
                else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                else if (currentRotation == 1) { right = 1; }

                nextRotation = currentRotation;
            }
            else if (currentRoom == RoomR)
            {
                if (currentRotation == 0) { right = 1; }
                else if (currentRotation == -1) { forward = 1; }
                else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                else if (currentRotation == 1) { forward = -1; }

                if (currentRotation >= 2) { nextRotation = -1; }
                else { nextRotation = currentRotation + 1; }
            }
            else if (currentRoom == RoomLF)
            {
                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }
                }
                else if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;
                }
            }
            else if (currentRoom == RoomFR)
            {
                if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;
                }
                else if(generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }
                }
            }
            else if (currentRoom == RoomLR) 
            {
                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }
                }
                else if(generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }
                }
            }
            else
            {
                if (generatedRoom.doorL)
                {
                    if (currentRotation == 0) { right = -1; }
                    else if (currentRotation == -1) { forward = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = 1; }
                    else if (currentRotation == 1) { forward = 1; }

                    if (currentRotation <= -2) { nextRotation = 1; }
                    else { nextRotation = currentRotation - 1; }
                }
                else if (generatedRoom.doorF)
                {
                    if (currentRotation == 0) { forward = 1; }
                    else if (currentRotation == -1) { right = -1; }
                    else if (currentRotation == -2 || currentRotation == 2) { forward = -1; }
                    else if (currentRotation == 1) { right = 1; }

                    nextRotation = currentRotation;
                }
                else if(generatedRoom.doorR)
                {
                    if (currentRotation == 0) { right = 1; }
                    else if (currentRotation == -1) { forward = 1; }
                    else if (currentRotation == -2 || currentRotation == 2) { right = -1; }
                    else if (currentRotation == 1) { forward = -1; }

                    if (currentRotation >= 2) { nextRotation = -1; }
                    else { nextRotation = currentRotation + 1; }
                }
            }

            Vector3 lastPosition = generatedRoom.position;

            Vector3 newPosition = new Vector3(lastPosition.x + (roomOffset * right), lastPosition.y, lastPosition.z + (roomOffset * forward));
            
            forward = 0;
            right = 0;

            bool comparison = map.comparePositionsRec(newPosition, 0.1f);
            if (comparison) 
            {
                Debug.Log("There's already a room here at: "+newPosition+"!");
                if (generatedRoom.doorL)
                {
                    generatedRoom.doorL = false;

                    continue;
                }
                else if (generatedRoom.doorF)
                {
                    generatedRoom.doorF = false;

                    nextRotation = 0;

                    continue;
                }
                else if (generatedRoom.doorR)
                {
                    generatedRoom.doorR = false;

                    continue;
                }
                else 
                {
                    generatedRoom = generatedRoom.parent;

                    if (generatedRoom == firstRoom) { return; }

                    continue;
                }
                
            }

            generatedRoom = new TreeNode(rooms[Random.Range(1, 8)], newPosition, nextRotation);

            map.add(generatedRoom);

            nextRotation = 0;

            roomCount++;
            
        }
    }
}
