using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class DungeonGenerator : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public int dungeonSizeX = 52;
    public int dungeonSizeY = 28;
    public int roomNumX = 3;
    public int roomNumY = 2;
    public int minRoomX = 5;
    public int minRoomY = 4;

    private int GenType;


    private List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) centre)> roomCorners;
        
    public enum TileType
    {
        Wall,
        Floor
    }

    private void Start()
    {
        roomCorners = new List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) centre)>();
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        TileType[,] dungeon = new TileType[dungeonSizeX,dungeonSizeY];

        dungeon = ResetDungeon(dungeon);



        int roomsX = dungeonSizeX / roomNumX;
        int roomsY = dungeonSizeY / roomNumY;

        roomCorners = SplitDungeon(dungeon,roomNumX,roomNumY);

        //foreach(var room in roomCorners)
        //{
        //    Debug.Log(room);
        //}

        for (int i = 0; i < roomCorners.Count; i++)

        {

            //dungeon[roomCorners[i].topLeft.x, roomCorners[i].topLeft.y] = TileType.Floor;
            //dungeon[roomCorners[i].topRight.x, roomCorners[i].topRight.y] = TileType.Floor;
            //dungeon[roomCorners[i].bottomLeft.x, roomCorners[i].bottomLeft.y] = TileType.Floor;
            //dungeon[roomCorners[i].bottomRight.x, roomCorners[i].bottomRight.y] = TileType.Floor;
            //dungeon[roomCorners[i].centre.x, roomCorners[i].centre.y] = TileType.Floor;

            dungeon = GenerateRoom(dungeon, i);
        }

        SpawnDungeon(dungeon);

    }

    private List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) center)>
    SplitDungeon(TileType[,] grid, int partsX, int partsY)
    {
        int totalWidth = grid.GetLength(0);   // X-axis
        int totalHeight = grid.GetLength(1);  // Y-axis

        int partWidth = totalWidth / partsX;
        int partHeight = totalHeight / partsY;

        var result = new List<((int, int), (int, int), (int, int), (int, int), (int, int))>();

        for (int py = 0; py < partsY; py++)
        {
            for (int px = 0; px < partsX; px++)
            {
                int startX = px * partWidth;
                int startY = py * partHeight;

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
        // Extract section bounds
        int minX = roomCorners[index].topLeft.x;
        int maxX = roomCorners[index].topRight.x;

        int minY = roomCorners[index].topLeft.y;
        int maxY = roomCorners[index].bottomLeft.y;

        int maxRoomWidth = maxX - minX + 1;
        int maxRoomHeight = maxY - minY + 1;

        // Ensure we have space for at least minRoomX/Y
        if (maxRoomWidth < minRoomX || maxRoomHeight < minRoomY)
        {
            Debug.LogWarning($"Room section too small at index {index}: {maxRoomWidth}x{maxRoomHeight}");
            return tile;
        }

        // Determine actual room size
        int roomWidth = UnityEngine.Random.Range(minRoomX, maxRoomWidth + 1);
        int roomHeight = UnityEngine.Random.Range(minRoomY, maxRoomHeight + 1);

        // Random offset within the section
        int offsetX = UnityEngine.Random.Range(0, maxRoomWidth - roomWidth + 1);
        int offsetY = UnityEngine.Random.Range(0, maxRoomHeight - roomHeight + 1);

        int startX = minX + offsetX;
        int startY = minY + offsetY;

        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                tile[x, y] = TileType.Floor;
            }
        }

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
                Vector3 position = new Vector3(x, 0, y); // 3D space, Y is height (change to 2D if needed)

                GameObject prefabToSpawn = null;

                switch (grid[x, y])
                {
                    case TileType.Floor:
                    prefabToSpawn = floor;
                    break;
                    case TileType.Wall:
                    prefabToSpawn = wall;
                    break;
                }

                if (prefabToSpawn != null)
                {
                    Instantiate(prefabToSpawn, position, Quaternion.identity, this.transform);
                }
            }
        }
    }


}
