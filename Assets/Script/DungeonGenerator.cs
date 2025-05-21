using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class DungeonGenerator : MonoBehaviour
{
    private GameObject dungeonContainer;
    public GameObject floor;
    public GameObject wall;
    public int dungeonSizeX = 52;
    public int dungeonSizeY = 28;
    public int roomNumX = 3;
    public int roomNumY = 2;
    public int minRoomX = 5;
    public int minRoomY = 4;

    private int[] origninalMin ;
    private int[] origninalRoomNum ;
    private int[] originalDungeonSize ;
    [SerializeField] private int roomPadding = 1;       
    [SerializeField] private int dungeonBorder = 1;

    [Range(0, 1)] public float straightCorridoorChance;

    private int GenType;
    public bool genTest;
    private float roomChance = 0.7f;
    private List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) centre)> roomCorners;
    private List<(int x, int y)> addedCentres = new List<(int x, int y)>();  

    public enum DungeonType
    {
        SingleRoom,
        fullRoom,
        halfRoom,
        threeQuartRoom,
    }
    public enum TileType
    {
        Wall,
        Floor
    }

    private void Start()
    {
        origninalMin = new int[2] { minRoomX,minRoomY};
        origninalRoomNum = new int[2] {roomNumX,roomNumY};
        originalDungeonSize = new int[4] {dungeonSizeX,dungeonSizeY, dungeonSizeX, dungeonSizeY };
        roomCorners = new List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) centre)>();
        GenerateDungeon();
    }


    public void Update()
    {
        if(genTest)
        {
            float tempTime = Time.time;
            GenerateDungeon();
            genTest = false;

            //Debug.Log($"time taken to gen {(Time.time - tempTime)*1000} ms");
        }
    }
    public void GenerateDungeon()
    {
        addedCentres.Clear();
        DecideDungeonType();





        if (dungeonContainer != null)
            Destroy(dungeonContainer);//destroy the previous dungeon generated

        dungeonContainer = new GameObject("DungeonContainer");
        dungeonContainer.transform.parent = this.transform;
        AdjustDungeonSize(originalDungeonSize[0],originalDungeonSize[1]);
        roomCorners.Clear();
        TileType[,] dungeon = new TileType[dungeonSizeX, dungeonSizeY];

        dungeon = ResetDungeon(dungeon);//fills dungeon with wall blocks 

        roomCorners = SplitDungeon(dungeon, roomNumX, roomNumY);//splits dungeon into sections 

        for (int i = 0; i < roomCorners.Count; i++)
        {

            float rand = UnityEngine.Random.value;

            Debug.LogFormat($"Rand = {rand}, chance = {roomChance}");
            if(rand  <= roomChance)
            {
                dungeon = GenerateRoom(dungeon, i);

                //TODO Log centres
            }
            
        }

        dungeon = ConnectRooms(dungeon);

        SpawnDungeon(dungeon);
    }

    private List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) center)>
    SplitDungeon(TileType[,] grid, int partsX, int partsY)
    {
        int totalWidth = grid.GetLength(0) - 2 * dungeonBorder;
        int totalHeight = grid.GetLength(1) - 2 * dungeonBorder;

        int partWidth = totalWidth / partsX;
        int partHeight = totalHeight / partsY;

        var result = new List<((int, int), (int, int), (int, int), (int, int), (int, int))>();

        for (int py = 0; py < partsY; py++)
        {
            for (int px = 0; px < partsX; px++)
            {


                int startX = px * partWidth + dungeonBorder;
                int startY = py * partHeight + dungeonBorder;

                int endX = startX + partWidth - 1;
                int endY = startY + partHeight - 1;

                var topLeft = (startX, startY);
                var topRight = (endX, startY);
                var bottomLeft = (startX, endY);
                var bottomRight = (endX, endY);
                var centre = ((startX + endX) / 2, (startY + endY) / 2);

                result.Add((topLeft, topRight, bottomLeft, bottomRight, centre));
            }
        }

        return result;
    }



    private TileType[,] GenerateRoom(TileType[,] tile, int index)
    {
        // Shrink the usable area by 1 on each side (padding)
        int sectionMinX = roomCorners[index].topLeft.x + roomPadding;
        int sectionMaxX = roomCorners[index].topRight.x - roomPadding;

        int sectionMinY = roomCorners[index].topLeft.y + roomPadding;
        int sectionMaxY = roomCorners[index].bottomLeft.y - roomPadding;


        int usableWidth = sectionMaxX - sectionMinX + 1;
        int usableHeight = sectionMaxY - sectionMinY + 1;

        // Ensure there's enough room to fit the minimum room + wall padding
        if (usableWidth < minRoomX || usableHeight < minRoomY)
        {
            Debug.LogWarning($"Section too small for room at index {index}: {usableWidth}x{usableHeight}");
            return tile;
        }

        // Choose random room size within usable space
        int roomWidth = UnityEngine.Random.Range(minRoomX, usableWidth + 1);
        int roomHeight = UnityEngine.Random.Range(minRoomY, usableHeight + 1);

        // Random offset within the usable area (to pad on all sides)
        int offsetX = UnityEngine.Random.Range(0, usableWidth - roomWidth + 1);
        int offsetY = UnityEngine.Random.Range(0, usableHeight - roomHeight + 1);

        int startX = sectionMinX + offsetX;
        int startY = sectionMinY + offsetY;

        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                tile[x, y] = TileType.Floor;
            }
        }

        int centreX = startX+roomWidth/2;
        int centreY = startY+roomHeight/2;
        addedCentres.Add((centreX, centreY));

        return tile;
    }





    private TileType[,] ResetDungeon(TileType[,] tilemap)
    {
        int width = tilemap.GetLength(0);
        int height = tilemap.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap[x, y] = TileType.Wall;
            }
        }
        return tilemap;
        
    }


    public void SpawnDungeon(TileType[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                GameObject prefabToSpawn = grid[x, y] switch
                {
                    TileType.Floor => floor,
                    TileType.Wall => wall,
                    _ => null
                };

                if (prefabToSpawn != null)
                {
                    Instantiate(prefabToSpawn, position, Quaternion.identity, dungeonContainer.transform);
                }
            }
        }
    }

    private void DecideDungeonType()
    {
        DungeonType dungeonType;

        dungeonType = RandomEnumValue<DungeonType>();
        if (dungeonType == DungeonType.SingleRoom)//Sets room size and room numbers so single room can be 1 large room
        {            


            minRoomX = Convert.ToInt32(Math.Ceiling(.9 * originalDungeonSize[0]));
            minRoomY = Convert.ToInt32(Math.Ceiling(.9 * originalDungeonSize[1]));
            roomNumX = 1;
            roomNumY = 1;
            roomChance = 1.1f;

        }
        else
        {
            minRoomX = origninalMin[0];
            minRoomY = origninalMin[1];
            roomNumX = origninalRoomNum[0];
            roomNumY = origninalRoomNum[1];
        }

        switch(dungeonType)//set probablility to spawn room based on 
        {
            case DungeonType.threeQuartRoom:
            roomChance = .75f;
            break;
            case DungeonType.fullRoom:
            roomChance = 1f;
            break;
            case DungeonType.halfRoom:
            roomChance = .5f;
            break;
        }
        Debug.Log("Dungeon Type : " + dungeonType);

    }


    static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(UnityEngine.Random.Range(0,v.Length));
    }


    private void AdjustDungeonSize(int originalX,int originalY)//changes dungeon size based on padding and border 
    {
        originalDungeonSize[2] = originalX;
        originalDungeonSize[3] = originalY;

        dungeonSizeX = originalDungeonSize[2] + (dungeonBorder * 2) + (roomPadding * 2 * roomNumX);
        dungeonSizeY = originalDungeonSize[3] + (dungeonBorder * 2) + (roomPadding * 2 * roomNumY);




    }


    public TileType[,] ConnectRooms(TileType[,] tileType)
    {
        int numberOfRooms = addedCentres.Count;
        if (numberOfRooms < 2)
            return tileType;

        for (int i = 0; i < numberOfRooms; i++)
        {
            (int x, int y) start = addedCentres[i];
            (int x, int y) end = addedCentres[(i + 1) % numberOfRooms]; // loop to first at the end

            int currentX = start.x;
            int currentY = start.y;

            // Randomize first direction
            bool preferX = UnityEngine.Random.value < 0.5f;
            string lastDirection = preferX ? "x" : "y";

            while (currentX != end.x || currentY != end.y)
            {
                bool stepX = false;

                if (currentX != end.x && currentY != end.y)
                {
                    if (UnityEngine.Random.value < straightCorridoorChance)
                    {
                        stepX = lastDirection == "x";
                    }
                    else
                    {
                        stepX = UnityEngine.Random.value < 0.5f;
                    }
                }
                else if (currentX != end.x)
                {
                    stepX = true;
                }
                else if (currentY != end.y)
                {
                    stepX = false;
                }

                if (stepX)
                {
                    currentX += Math.Sign(end.x - currentX);
                    lastDirection = "x";
                }
                else
                {
                    currentY += Math.Sign(end.y - currentY);
                    lastDirection = "y";
                }

                // Mark the tile as floor
                if (currentX >= 0 && currentX < dungeonSizeX && currentY >= 0 && currentY < dungeonSizeY)
                {


                    // Ensure not out of bounds
                    // The dungeon map should be stored to be modified here
                    // You can make dungeon a class member if needed
                    // Example:
                   tileType[currentX, currentY] = TileType.Floor;

                }
            }
        }

        return tileType;
    }



}
