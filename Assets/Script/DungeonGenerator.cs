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
    public int roomNumX = 1;
    public int roomNumY = 1;
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



        
    }

    private List<((int x, int y) topLeft, (int x, int y) topRight, (int x, int y) bottomLeft, (int x, int y) bottomRight, (int x, int y) center)>
        SplitDungeon(TileType[,] grid, int partsX, int partsY)
    {
        int totalWidth = grid.GetLength(1);
        int totalHeight = grid.GetLength(0);

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


}
