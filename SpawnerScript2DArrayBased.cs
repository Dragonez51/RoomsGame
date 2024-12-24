using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript2DArrayBased : MonoBehaviour
{

    [SerializeField] public int RoomAmount;

    private float roomOffset = 7.9f;

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

    private int startRoom;

    private int[,,] map;

    // Start is called before the first frame update
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

        rooms = new Room[]{RRoom0, RRoomL, RRoomF, RRoomR, RRoomLF, RRoomFR, RRoomLR, RRoomLFR};

        //hardCodeMap();
        generateMap();
        spawnAll();

    }

    void hardCodeMap()
                            // This function was made for testing spawnAll function.
                            // If you want you can hardCode a map yourself.
                            // In the future I could add making your own custom maps.
    {
        map = new int[,,] 
        { 
            { {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} }, 
            { {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} }, 
            { {0, 0}, {2, 1}, {2, 0}, {0, 0}, {0, 0} }, 
            { {8, 0}, {4, 2}, {1, 2}, {0, 0}, {0, 0} }, 
            { {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} }, 
        };
    }

    //void writeMap() 
    //                        // Debug puprouse
    //{
    //    for (int i = 0; i < map.GetLength(0); i++) 
    //    {
    //        for (int j = 0; j < map.GetLength(1); j++) 
    //        {
    //            for (int k = 0; k < map.GetLength(2); k++) 
    //            {
                    
    //            }
    //        }
    //    }
    //}

    void spawnAll() 
                    // on 0 do nothing || on 1 spawn room 0 || on 2 spawn roomL || on 3 spawn roomF || on 4 spawn roomR
                    // on 5 spawn roomLF || on 6 spawn room FR || on 7 spawn room LR || on 8 spawn roomLFR  
    {
        Room spawnRoom = rooms[Random.Range(0,6)];

        int skipX = 1; // this determines how far to move the transform to the right

        var leftSide = transform.position.x; // Keep the transform position to the left so that every other row starts from the same point.

        for (int i = 0; i < RoomAmount; i++) {          // Rows 

            transform.position = new Vector3(leftSide, transform.position.y, transform.position.z - roomOffset); // move transform to the leftSide (first collumn in a row) and move it backwards by roomOffset distance
            
            skipX = 1; // reset the counter how far to spawn the room on every row change

            for (int j = 0; j < RoomAmount; j++) {      // Collumns
                if (map[i, j, 0] == 0)
                {
                    skipX++; // skip this place in the world
                    continue;
                }
                else if (map[i, j, 0] == 1)
                {
                    Spawn(rooms[0], skipX,  map[i, j, 1]);
                }
                else if (map[i, j, 0] == 2)
                {
                    Spawn(rooms[1], skipX,  map[i, j, 1]);
                }
                else if (map[i, j, 0] == 3)
                {
                    Spawn(rooms[2], skipX,  map[i, j, 1]);
                }
                else if (map[i, j, 0] == 4)
                {
                    Spawn(rooms[3], skipX,  map[i, j, 1]);
                }
                else if (map[i, j, 0] == 5)
                {
                    Spawn(rooms[4], skipX, map[i, j, 1]);
                }
                else if (map[i, j, 0] == 6)
                {
                    Spawn(rooms[5], skipX, map[i, j, 1]);
                }
                else if (map[i, j, 0] == 7)
                {
                    Spawn(rooms[6], skipX, map[i, j, 1]);
                }
                else if (map[i, j, 0] == 8)
                {
                    Spawn(rooms[7], skipX, map[i, j, 1]);
                }
                skipX++;
            }
        }
    }

    void Spawn(Room roomToSpawn, int skipX, int rotation) 
                        // Spawn the room and move the transform;
                        // Positives - rotate to right || Negatives rotate to left || 0 - don't rotate
    {
        GameObject clone = Instantiate(roomToSpawn.roomRef, new Vector3(transform.position.x + (roomOffset * skipX), transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 1));
        clone.transform.Rotate(Vector3.down, 90 * rotation);
    }

    void generateMap()
    // generate a map 
    {
        map = new int[(RoomAmount * RoomAmount) + 1, (RoomAmount * RoomAmount) + 1, 5]; // Rows are Z (--------), Collumns are X (|||||), third dimension is representation of a room having:
                                                                      //                                                    { roomNumber, rotation, doorL, doorF, doorR }
                                                                      //            doorL, doorF, doorR determines whether it was used or not (used only in rooms with more than 1 exit)
        int Z, X;
        int rotation = 0;
        int roomCount = 0;

        if (RoomAmount % 2 == 0)
        {
            Z = RoomAmount / 2;
            X = RoomAmount / 2;
        }
        else
        {
            Z = RoomAmount / 2 + 1;
            X = RoomAmount / 2 + 1;
        }

        map[Z, X, 0] = 1; // set the center room to room0
        map[Z, X, 1] = 2; // rotate the center room 180 degrees

        Z--; // move the selected space 1 forward
        map[Z, X, 0] = Random.Range(2, 8); // generate a random room that is not room0

        while (roomCount < RoomAmount)
        {
            int tempZ = Z; // save coords of this room to know
            int tempX = X; // where to come back in case of room0 

            while (map[Z, X, 0] != 1 && roomCount < RoomAmount) // spawn next rooms until a room0 spawns
            {
                int currentRoom = map[Z, X, 0];

                if (currentRoom == 2) // if roomL
                {
                    if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                    else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                    else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                    else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                    rotation -= 1; // set the rotation to left
                    map[Z, X, 1] = rotation; // set room rotation
                }
                else if (currentRoom == 3) // if roomF
                {
                    if (rotation == 0) { Z -= 1; } // no rotation - go forward
                    else if (rotation == -1) { X -= 1; } // rotated left - go left
                    else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                    else if (rotation == 1) { X += 1; } // rotated right - go right

                    map[Z, X, 1] = rotation;
                }
                else if (currentRoom == 4) // if roomR
                {
                    if (rotation == 0) { X += 1; } // no rotation - go right
                    else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                    else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                    else if (rotation == 1) { Z += 1; } // rotated right - go backward

                    rotation += 1;

                    map[Z, X, 1] = rotation;
                }
                else if (currentRoom == 5) // if roomLF                        |!| be aware - this algorithm is taking only the most left exit (to be updgraded) |!| 
                {
                    if (map[Z, X, 2] == 0) // if doorL is open
                    {
                        if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                        else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                        else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                        else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                        rotation -= 1; // set the rotation to left

                        map[Z, X, 2] = 1; // set doorL to closed

                        map[Z, X, 1] = rotation; // set room rotation

                        tempZ = Z;
                        tempX = X;
                    }
                    else if (map[Z, X, 3] == 0) // if doorL is closed and doorF is open
                    {
                        if (rotation == 0) { Z -= 1; } // no rotation - go forward
                        else if (rotation == -1) { X -= 1; } // rotated left - go left
                        else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                        else if (rotation == 1) { X += 1; } // rotated right - go right

                        map[Z, X, 3] = 1; // set doorF to closed

                        map[Z, X, 1] = rotation; // set room rotation
                    }
                    else // go back to previous room            |?|How to save more than one last room?|?|
                    {
                        Debug.Log("going back to previous room from roomLF");

                        Z = tempZ;
                        X = tempX;
                    }

                }
                else if (currentRoom == 6) // if roomFR
                {
                    if (map[Z, X, 3] == 0) // if doorF is open
                    {
                        if (rotation == 0) { Z -= 1; } // no rotation - go forward
                        else if (rotation == -1) { X -= 1; } // rotated left - go left
                        else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                        else if (rotation == 1) { X += 1; } // rotated right - go right

                        map[Z, X, 3] = 1;

                        map[Z, X, 1] = rotation;
                    }
                    else if (map[Z, X, 4] == 0) // if doorR is open
                    {
                        if (rotation == 0) { X += 1; } // no rotation - go right
                        else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                        else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                        else if (rotation == 1) { Z += 1; } // rotated right - go backward

                        rotation += 1;

                        map[Z, X, 4] = 1;

                        map[Z, X, 1] = rotation;
                    }
                    else // if all doors are closed - go back to previous room with opened doors
                    {
                        Debug.Log("going back to previous room from room FR");

                        Z = tempZ;
                        X = tempX;
                    }
                }
                else if (currentRoom == 7) // if roomLR
                {
                    if (map[Z, X, 2] == 0)
                    {
                        if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                        else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                        else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                        else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                        rotation -= 1; // set the rotation to left

                        map[Z, X, 2] = 1;

                        map[Z, X, 1] = rotation; // set room rotation

                        tempZ = Z;
                        tempX = X;
                    }
                    else if (map[Z, X, 4] == 0)
                    {
                        if (rotation == 0) { X += 1; } // no rotation - go right
                        else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                        else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                        else if (rotation == 1) { Z += 1; } // rotated right - go backward

                        rotation += 1;

                        map[Z, X, 4] = 1;

                        map[Z, X, 1] = rotation;
                    }
                    else 
                    {
                        Debug.Log("going back to previous room from roomLR");

                        tempZ = Z;
                        tempX = X;
                    }
                }
                else if (currentRoom == 8) // if roomLFR
                {
                    if (map[Z, X, 2] == 0)
                    {
                        if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                        else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                        else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                        else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                        rotation -= 1; // set the rotation to left
                        map[Z, X, 1] = rotation; // set room rotation

                        tempZ = Z;
                        tempX = X;
                    }
                    else if (map[Z, X, 3] == 0)
                    {
                        if (rotation == 0) { Z -= 1; } // no rotation - go forward
                        else if (rotation == -1) { X -= 1; } // rotated left - go left
                        else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                        else if (rotation == 1) { X += 1; } // rotated right - go right

                        map[Z, X, 3] = 1;
                        map[Z, X, 1] = rotation;

                        tempZ = Z;
                        tempX = X;
                    }
                    else if (map[Z, X, 4] == 0)
                    {
                        if (rotation == 0) { X += 1; } // no rotation - go right
                        else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                        else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                        else if (rotation == 1) { Z += 1; } // rotated right - go backward

                        rotation += 1;

                        map[Z, X, 4] = 1;
                        map[Z, X, 1] = rotation;
                    }
                    else 
                    {
                        Debug.Log("going back to previous room from roomLFR");

                        Z = tempZ;
                        X = tempX;
                    }
                }

                Debug.Log(roomCount);

                if (map[Z, X, 0] < 1) // if the space for the next room is empty, generate next room
                {
                    map[Z, X, 0] = Random.Range(2, 8);
                    roomCount++;
                }
                else 
                {
                    break;
                }

                if (roomCount > 200) 
                {
                    break;
                }
            }

        }
    }
}
