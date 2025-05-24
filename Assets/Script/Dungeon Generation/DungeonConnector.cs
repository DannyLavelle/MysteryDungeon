using UnityEngine;
using System.Collections.Generic;
using static DungeonGenerator;

public class DungeonConnector
{
    private float corridorStraightness;
    private int width, height;

    public DungeonConnector(float straightness, int width, int height)
    {
        corridorStraightness = straightness;
        this.width = width;
        this.height = height;
    }

    public TileType[,] ConnectRooms(TileType[,] grid, List<Vector2Int> centers,DungeonContainer dungeon)
    {
        if (centers.Count < 2) return grid;

        for (int i = 0; i < centers.Count; i++)
        {
            Vector2Int start = centers[i];
            Vector2Int end = centers[(i + 1) % centers.Count];

            int x = start.x, y = start.y;
            bool preferX = Random.value < 0.5f;
            string last = preferX ? "x" : "y";

            while (x != end.x || y != end.y)
            {
                bool stepX = x != end.x && (y == end.y || (Random.value < corridorStraightness ? last == "x" : Random.value < 0.5f));

                if (stepX)
                {
                    x += (int)Mathf.Sign(end.x - x);
                    last = "x";
                }
                else
                {
                    y += (int)Mathf.Sign(end.y - y);
                    last = "y";
                }

                if (x >= 0 && x < width && y >= 0 && y < height  )
                {
                    var pos = (x, y);


                    if (grid[x, y] == TileType.Wall || !dungeon.floorTiles.Contains(pos))
                    {
                        grid[x, y] = TileType.Floor;
                        dungeon.corridoorTiles.Add(pos);
                    }



                }
                    
            }
        }

        return grid;
    }
}
