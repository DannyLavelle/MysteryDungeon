using UnityEngine;
using static DungeonGenerator;

public static class DungeonUtility
{
    public static void FillWithWalls(TileType[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = TileType.Wall;
    }

    public static void SpawnTiles(TileType[,] grid, GameObject floor, GameObject wall, Transform parent)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = grid[x, y] switch
                {
                    TileType.Floor => floor,
                    TileType.Wall => wall,
                    _ => null
                };

                if (prefab != null)
                    Object.Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, parent);
            }
    }

    public static Vector2Int CalculateDungeonSize(int baseX, int baseY, int roomsX, int roomsY, int border, int padding)
    {
        int sizeX = baseX + border * 2 + padding * 2 * roomsX;
        int sizeY = baseY + border * 2 + padding * 2 * roomsY;
        return new Vector2Int(sizeX, sizeY);
    }
}
