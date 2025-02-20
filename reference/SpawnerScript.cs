using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] public int RoomAmount;
    public float RoomOffset = 7.9f;
    private int roomsCount;
    private int availableRooms;
    private int rotation; // this is a variable that changes based on the amount of times the spawner has rotated (negatives to the left, positives to the right)

    private List<Transform> transforms = new List<Transform>();

    private int roomsBackward;

    [SerializeField] private GameObject room0;
    [SerializeField] private GameObject roomL;
    [SerializeField] private GameObject roomF;
    [SerializeField] private GameObject roomR;
    [SerializeField] private GameObject roomLF;
    [SerializeField] private GameObject roomFR;
    [SerializeField] private GameObject roomLR;
    [SerializeField] private GameObject roomLFR;

    private Room RRoom0, RRoomL, RRoomF, RRoomR, RRoomLF, RRoomFR, RRoomLR, RRoomLFR;

    private Room[] rooms;
    private Room[] spawnedRooms;

    private Room spawnedRoom;
    private Transform tempSpawnerTransform;

    void Start()
    {
        availableRooms = RoomAmount;
        spawnedRooms = new Room[RoomAmount];

        transform.Rotate(Vector3.down, 90);

        RRoom0 = new Room(room0, false, false, false);

        RRoomL = new Room(roomL, true, false, false);
        RRoomF = new Room(roomF, false, true, false);
        RRoomR = new Room(roomR, false, false, true);
        RRoomLF = new Room(roomLF, true, true, false);
        RRoomFR = new Room(roomFR, false, true, true);
        RRoomLR = new Room(roomLR, true, false, true);
        RRoomLFR = new Room(roomLFR, true, true, true);

        rooms = new Room[]{RRoom0, RRoomL, RRoomF, RRoomR, RRoomLF, RRoomFR, RRoomLR, RRoomLFR };

        Room startingRoom = RRoom0;

        GameObject clone = Instantiate(startingRoom.roomRef, transform.position, transform.rotation);
        clone.transform.Rotate(Vector3.up, 180);

        StartSpawning();
    }

    private void StartSpawning() 
    {
        transforms.Add(transform);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + RoomOffset);
        Room spawnRoom = rooms[Random.Range(1, 6)];
        spawnedRoom = spawnRoom;
        GameObject clone = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
        spawnedRooms[0] = spawnRoom;
        roomsCount = 1;
        SpawnSequence();
    }

    private void Spawn(int side, bool onlyRoomZero) // 0 for left, 1 for front, 2 for right 
    {
        switch (side) 
        {
            case 0: // doors on the left

                if (rotation == 0) // if room didn't rotate
                {
                    moveTransform(0); // go left
                    transform.Rotate(Vector3.down, 90);
                    rotation = -1;
                }
                else if (rotation == -1) // if room rotated left once
                {
                    moveTransform(3); // go backwards
                    transform.Rotate(Vector3.down, 90);
                    rotation = -2;
                }
                else if (rotation == -2 || rotation == 2) // if room rotated two times
                {
                    moveTransform(2); // go right
                    transform.Rotate(Vector3.down, 90);
                    rotation = 1;
                }
                else if (rotation == 1) // if room rotated right once
                {
                    moveTransform(1); // go forward
                    transform.Rotate(Vector3.down, 90);
                    rotation = 0;
                }

                if (roomsCount <= RoomAmount && !onlyRoomZero)
                {
                    roomsBackward = 0;

                    Room spawnRoom = rooms[Random.Range(0, 6)];
                    spawnedRoom = spawnRoom;

                    GameObject cloneL = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);

                    transforms.Add(cloneL.transform);

                    Debug.Log(cloneL.name);

                    spawnedRooms[roomsCount-1] = spawnRoom;
                    roomsCount++;
                }
                else
                {
                    Room spawnRoom = rooms[0];
                    GameObject clone = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
                    Debug.Log("Spawned room0 from doorL");
                }
                break;
            case 1: // doors at front
                if (rotation == 0) // if room didn't rotate
                {
                    moveTransform(1);
                }
                else if (rotation == -1) // if room rotated left once
                {
                    moveTransform(0);
                }
                else if (rotation == -2 || rotation == 2) // if room rotated two times
                {
                    moveTransform(3); ;
                }
                else if (rotation == 1) // if room rotated right once
                {
                    moveTransform(2);
                }

                if (roomsCount <= RoomAmount && !onlyRoomZero)
                {
                    roomsBackward = 0;

                    Room spawnRoom = rooms[Random.Range(0, 6)];
                    spawnedRoom = spawnRoom; // Probably to be deleted

                    GameObject cloneF = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
                    transforms.Add(cloneF.transform);

                    Debug.Log(cloneF.name);

                    spawnedRooms[roomsCount-1] = spawnRoom;
                    roomsCount++;
                }
                else
                {
                    Room spawnRoom = rooms[0];
                    GameObject clone = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
                    Debug.Log("Spawned room0 from doorF");
                }
                break;
            case 2: // doors on the right
                if (rotation == 0) // if room didn't rotate
                {
                    moveTransform(2);
                    transform.Rotate(Vector3.up, 90);
                    rotation = 1;
                }
                else if (rotation == -1) // if room rotated left once
                {
                    moveTransform(1);
                    transform.Rotate(Vector3.up, 90);
                    rotation = 0;
                }
                else if (rotation == -2 || rotation == 2) // if room rotated two times
                {
                    moveTransform(0);
                    transform.Rotate(Vector3.up, 90);
                    rotation = -1;
                }
                else if (rotation == 1) // if room rotated right once
                {
                    moveTransform(3);
                    transform.Rotate(Vector3.up, 90);
                    rotation = 2;
                }

                if (roomsCount <= RoomAmount && !onlyRoomZero)
                {
                    roomsBackward = 0;

                    Room spawnRoom = rooms[Random.Range(0, 6)];
                    spawnedRoom = spawnRoom; // probably usless || absolutely not 

                    GameObject cloneR = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
                    transforms.Add(cloneR.transform);

                    Debug.Log(cloneR.name);

                    spawnedRooms[roomsCount-1] = spawnRoom;
                    roomsCount++;
                }
                else
                {
                    Room spawnRoom = rooms[0];
                    GameObject clone = Instantiate(spawnRoom.roomRef, transform.position, transform.rotation);
                    Debug.Log("Spawned room0 from doorR");
                }
                break;
            default:
                Debug.Log("Podano niew³aœciwy numer drzwi");
                break;
        }
    }

    private void SpawnSequence() 
    {
        while(roomsCount < RoomAmount) 
        {
            tempSpawnerTransform = transform; // Save the current room coords to a temporary variable
            transforms.Add(tempSpawnerTransform);

            Debug.Log("roomsCount: "+roomsCount);

            if (spawnedRooms[roomsCount-1] == rooms[0]) 
            {
                roomsBackward++;
                Debug.Log("goBackToPreviousTransform method called...");
                goBackToPreviousTransform();
            }

            if (spawnedRoom.doorL && !spawnedRoom.DoneDoorL)
            {
                Spawn(0, false);
                //SpawnSequence();
                spawnedRoom.DoneDoorL = true;
            }


            if (spawnedRoom.doorF && !spawnedRoom.DoneDoorF)
            {
                Spawn(1, false);
                //SpawnSequence();
                spawnedRoom.DoneDoorF = true;
            }


            if (spawnedRoom.doorR && !spawnedRoom.DoneDoorR)
            {
                Spawn(2, false);
                //SpawnSequence();
                spawnedRoom.DoneDoorR = true;
            }

            
        }
        Debug.Log("roomsCount >= RoomAmount");

        cleaningSequence();
    }

    private void cleaningSequence() // spawn rooms 0 until no doors are left with empty doors
    {
        while (roomsBackward < RoomAmount) { 
        
            if (spawnedRoom.doorL && !spawnedRoom.DoneDoorL) { Spawn(0, true); }    
            if (spawnedRoom.doorF && !spawnedRoom.DoneDoorF) { Spawn(1, true); }
            if (spawnedRoom.doorR && !spawnedRoom.DoneDoorR) { Spawn(2, true); }

            roomsBackward++;

            Debug.Log("goBackToPreviousTransform method called...");
            goBackToPreviousTransform();
        }
    }

    private void moveTransform(int direction) // 0 left, 1 forward, 2 right, 3 backwards 
    {
        switch(direction)
        {
            case 0:
                transform.position = new Vector3(transform.position.x - RoomOffset, transform.position.y, transform.position.z);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + RoomOffset);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x + RoomOffset, transform.position.y, transform.position.z);
                break;
            case 3:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - RoomOffset);
                break;
            default:
                break;

        }
    }
    private void goBackToPreviousTransform()
    {
        Debug.Log("Rooms Backward: "+roomsBackward);
        Debug.Log("transforms.Count: "+transforms.Count);
        transform.position.Set(transforms[transforms.Count - roomsBackward - 1].position.x, tempSpawnerTransform.position.y, tempSpawnerTransform.position.z);
        transform.rotation.Set(transforms[transforms.Count - roomsBackward - 1].rotation.x, tempSpawnerTransform.rotation.y, tempSpawnerTransform.rotation.z, tempSpawnerTransform.rotation.w);
        spawnedRoom = spawnedRooms[roomsCount - roomsBackward - 1];
    }
}
