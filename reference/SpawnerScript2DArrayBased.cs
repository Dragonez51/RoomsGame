using UnityEngine;

//                  TODO:
//      1. Make more use of Room.cs file as class. When a Room object is created spawn it in the world with coords that are set in constructor. - this will skip the spawnAll method and could skip making the 3D map. Instead we could make a 2D map of Room objects.
//              The map wouldn't be so huge with such great amounts of 0's in it. It would also remove the problem with going out of bounds because there are no bounds.
//      2. Backtracking system. (If the rooms are the same sizes [which they need to be in order to fit the doors perfectly] then we could compare transforms when trying to spawn [|?|only in backtracking or in the algorithm also|?|])
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
    private long mapSize;

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

        mapSize = (RoomAmount + 1) * 2;

        //hardCodeMap();
        generateMap();
        spawnAll();
        writeMap();
    }

    void hardCodeMap()
                            // This function was made for testing spawnAll function.
                            // If you want you can hardCode a map yourself.
                            // In the future I could add making your own custom maps.
    {
        mapSize = 5;
        map = new int[,,] 
        { 
            { {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} }, 
            { {0, 0}, {0, 0}, {7, 0}, {6, 1}, {2, 1} }, 
            { {0, 0}, {2, -1}, {5, 0}, {0, 0}, {0, 0} }, 
            { {8, -1}, {4, -2}, {1, 2}, {0, 0}, {0, 0} }, 
            { {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} }, 
        };
    }

    void writeMap()
    // Debug puprouse
    {
        string result = "";
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int k = 0; k < map.GetLength(2); k++)
                {
                    result += map[i, j, k];
                }
                result += ", ";
            }
            result += "\n";
        }
        Debug.Log(result);
    }

    void spawnAll()
    {
        Room spawnRoom = rooms[Random.Range(0, 6)];

        int skipX = 1; // this determines how far to move the transform to the right

        var leftSide = transform.position.x; // Keep the transform position to the left so that every other row starts from the same point.

        for (int i = 0; i < mapSize; i++)
        {          // Rows 

            transform.position = new Vector3(leftSide, transform.position.y, transform.position.z - roomOffset); // move transform to the leftSide (first collumn in a row) and move it backwards by roomOffset distance

            skipX = 1; // reset the counter how far to spawn the room on every row change

            for (int j = 0; j < mapSize; j++)
            {      // Collumns
                if (map[i, j, 0] == 0)
                {
                    skipX++; // skip this place in the world
                    continue;
                }
                else 
                {
                    Spawn(rooms[map[i, j, 0] - 1], skipX, map[i, j, 1]);
                }
                skipX++;
            }
        }
    }

    void Spawn(Room roomToSpawn, int skipX, int rotation) 
                        // Spawn the room and move the transform;
                        // Positives - rotate to right || Negatives rotate to left || 0 - don't rotate
    {
        GameObject clone = Instantiate(roomToSpawn.roomRef, new Vector3(transform.position.x + (roomOffset * skipX), transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 0));
        clone.transform.Rotate(Vector3.down, -90 * rotation);
    }

    void generateMap()
    {
        map = new int[mapSize, mapSize, 5]; // Rows are Z (--------), Collumns are X (|||||), third dimension is representation of a room having:
                                                                      //                                                    { roomNumber, rotation, doorL, doorF, doorR }
                                                                      //            doorL, doorF, doorR determines whether it was used or not (used only in rooms with more than 1 exit)
        long Z, X;
        int rotation = 0;
        int roomCount = 0;

        if (RoomAmount % 2 == 0)
        {
            Z = mapSize / 2;
            X = mapSize / 2;
        }
        else
        {
            Z = (mapSize + 1) / 2;
            X = (mapSize + 1) / 2;
        }

        map[Z, X, 0] = 1; // set the center room to room0
        map[Z, X, 1] = 2; // rotate the center room 180 degrees

        Z--; // move the selected space 1 forward
        map[Z, X, 0] = Random.Range(2, 8); // generate a random room that is not room0

            //int tempZ = Z; // save coords of this room to know
            //int tempX = X; // where to come back in case of room0 

        while (map[Z, X, 0] != 1) // spawn next rooms until a room0 spawns
        {
            int currentRoom = map[Z, X, 0];

            if (currentRoom == 2) // if roomL
            {
                Debug.Log("current room: roomL - rotating left");

                if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                if (map[Z, X, 0] != 0) 
                {
                    Debug.Log("next room is already set! Breaking the loop..."); 
                    break; 
                }

                if (rotation <= -2) // if the rotation is equal to -2 (2 times left)
                {
                    rotation = 1; // set the rotation from 2 left to 1 right
                }
                else
                {
                    rotation--;
                }

                map[Z, X, 1] = rotation; // set room rotation
            }
            else if (currentRoom == 3) // if roomF
            {
                Debug.Log("current room: roomF - not rotating");

                if (rotation == 0) { Z -= 1; } // no rotation - go forward
                else if (rotation == -1) { X -= 1; } // rotated left - go left
                else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                else if (rotation == 1) { X += 1; } // rotated right - go right

                if (map[Z, X, 0] != 0)
                {
                    Debug.Log("next room is already set! Breaking the loop...");
                    break;
                }

                map[Z, X, 1] = rotation;
            }
            else if (currentRoom == 4) // if roomR
            {
                Debug.Log("current room: roomR - rotating right");

                if (rotation == 0) { X += 1; } // no rotation - go right
                else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                else if (rotation == 1) { Z += 1; } // rotated right - go backward

                if (map[Z, X, 0] != 0)
                {
                    Debug.Log("next room is already set! Breaking the loop...");
                    break;
                }

                if (rotation >= 2)
                {
                    rotation = -1;
                }
                else
                {
                    rotation++;
                }

                map[Z, X, 1] = rotation;
            }
            else if (currentRoom == 5) // if roomLF                        |!| be aware - this algorithm is taking only the most left exit (to be updgraded) |!| 
            {

                Debug.Log("current room: roomLF:");

                if (map[Z, X, 2] == 0) // if doorL is open
                {
                    Debug.Log("LF - doorL open");

                    map[Z, X, 2] = 1; // set doorL to closed

                    if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                    else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                    else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                    else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation <= -2) // if the rotation is equal to -2 (2 times left)
                    {
                        rotation = 1; // set the rotation from 2 left to 1 right
                    }
                    else
                    {
                        rotation--;
                    }

                    map[Z, X, 1] = rotation; // set room rotation

                    //tempZ = Z;
                    //tempX = X;
                }
                else if (map[Z, X, 3] == 0) // if doorL is closed and doorF is open
                {
                    Debug.Log("LF - doorF open");

                    map[Z, X, 3] = 1; // set doorF to closed
                        
                    if (rotation == 0) { Z -= 1; } // no rotation - go forward
                    else if (rotation == -1) { X -= 1; } // rotated left - go left
                    else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                    else if (rotation == 1) { X += 1; } // rotated right - go right

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    map[Z, X, 1] = rotation; // set room rotation
                }
                else // go back to previous room            |?|How to save more than one last room?|?|
                {
                    Debug.Log("going back to previous room from roomLF");

                    //Z = tempZ;
                    //X = tempX;

                    //break;
                }

            }
            else if (currentRoom == 6) // if roomFR
            {
                Debug.Log("current room: roomFR:");

                if (map[Z, X, 3] == 0) // if doorF is open
                {
                    Debug.Log("FR - doorF open");

                    map[Z, X, 3] = 1;

                    if (rotation == 0) { Z -= 1; } // no rotation - go forward
                    else if (rotation == -1) { X -= 1; } // rotated left - go left
                    else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                    else if (rotation == 1) { X += 1; } // rotated right - go right

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    map[Z, X, 1] = rotation;
                }
                else if (map[Z, X, 4] == 0) // if doorR is open
                {
                    Debug.Log("FR - doorR open");
                    
                    map[Z, X, 4] = 1;

                    if (rotation == 0) { X += 1; } // no rotation - go right
                    else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                    else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                    else if (rotation == 1) { Z += 1; } // rotated right - go backward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation >= 2)
                    {
                        rotation = -1;
                    }
                    else
                    {
                        rotation++;
                    }

                    map[Z, X, 1] = rotation;
                }
                else // if all doors are closed - go back to previous room with opened doors
                {
                    Debug.Log("going back to previous room from room FR");

                    //Z = tempZ;
                    //X = tempX;

                    //break;
                }
            }
            else if (currentRoom == 7) // if roomLR
            {
                Debug.Log("current room: roomLR:");

                if (map[Z, X, 2] == 0)
                {
                    Debug.Log("LR - doorL is open");

                    map[Z, X, 2] = 1;

                    if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                    else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                    else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                    else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation <= -2) // if the rotation is equal to -2 (2 times left)
                    {
                        rotation = 1; // set the rotation from 2 left to 1 right
                    }
                    else
                    {
                        rotation--;
                    }

                    map[Z, X, 1] = rotation; // set room rotation

                    //tempZ = Z;
                    //tempX = X;
                }
                else if (map[Z, X, 4] == 0)
                {
                    Debug.Log("LR - doorR is open");
                    
                    map[Z, X, 4] = 1;

                    if (rotation == 0) { X += 1; } // no rotation - go right
                    else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                    else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                    else if (rotation == 1) { Z += 1; } // rotated right - go backward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation >= 2)
                    {
                        rotation = -1;
                    }
                    else
                    {
                        rotation++;
                    }

                    map[Z, X, 1] = rotation;
                }
                else 
                {
                    Debug.Log("going back to previous room from roomLR");

                    //tempZ = Z;
                    //tempX = X;

                    //break;
                }
            }
            else if (currentRoom == 8) // if roomLFR
            {
                Debug.Log("current room: roomLFR:");

                if (map[Z, X, 2] == 0)
                {
                    map[Z, X, 2] = 1;

                    if (rotation == 0) { X -= 1; } // if room didn't rotate - go left
                    else if (rotation == -1) { Z += 1; } // if room rotated left - go backward
                    else if (rotation == -2 || rotation == 2) { X += 1; } // if room rotated twice - go right
                    else if (rotation == 1) { Z -= 1; } // if room rotated right - go forward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation <= -2) // if the rotation is equal to -2 (2 times left)
                    {
                        rotation = 1; // set the rotation from 2 left to 1 right
                    }
                    else
                    {
                        rotation--;
                    }

                    map[Z, X, 1] = rotation; // set room rotation

                    //tempZ = Z;
                    //tempX = X;
                }
                else if (map[Z, X, 3] == 0)
                {
                    map[Z, X, 3] = 1;

                    if (rotation == 0) { Z -= 1; } // no rotation - go forward
                    else if (rotation == -1) { X -= 1; } // rotated left - go left
                    else if (rotation == -2 || rotation == 2) { Z += 1; } // rotated 180 - go backward
                    else if (rotation == 1) { X += 1; } // rotated right - go right

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    map[Z, X, 1] = rotation;

                    //tempZ = Z;
                    //tempX = X;
                }
                else if (map[Z, X, 4] == 0)
                {
                    map[Z, X, 4] = 1;
                    
                    if (rotation == 0) { X += 1; } // no rotation - go right
                    else if (rotation == -1) { Z -= 1; } // rotated left - go forward
                    else if (rotation == -2 || rotation == 2) { X -= 1; } // rotated 180 - go left
                    else if (rotation == 1) { Z += 1; } // rotated right - go backward

                    if (map[Z, X, 0] != 0)
                    {
                        Debug.Log("next room is already set! Breaking the loop...");
                        break;
                    }

                    if (rotation >= 2)
                    {
                        rotation = -1;
                    }
                    else 
                    {
                        rotation++;
                    }

                    map[Z, X, 1] = rotation;
                }
                else 
                {
                    Debug.Log("going back to previous room from roomLFR");

                    //Z = tempZ;
                    //X = tempX;

                    //break;
                }
            }

            map[Z, X, 0] = Random.Range(2, 9);
            roomCount++;

            Debug.Log("roomCount: "+roomCount);

            if (roomCount >= RoomAmount) { Debug.Log("roomCount higher than RoomAmount"); break; }
        }

    }
}