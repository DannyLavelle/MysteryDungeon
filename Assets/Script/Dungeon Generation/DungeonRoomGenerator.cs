using UnityEngine;
using System.Collections.Generic;
using static DungeonGenerator;

public class DungeonRoomGenerator
{
    private int minWidth, minHeight, padding;

    public DungeonRoomGenerator(int minW, int minH, int padding)
    {
        minWidth = minW;
        minHeight = minH;
        this.padding = padding;
    }

    public TileType[,] GenerateRoom(TileType[,] grid, RoomBounds bounds, List<Vector2Int> centers)
    {
        int minX = bounds.topLeft.x + padding;
        int maxX = bounds.topRight.x - padding;
        int minY = bounds.topLeft.y + padding;
        int maxY = bounds.bottomLeft.y - padding;

        int usableWidth = maxX - minX + 1;
        int usableHeight = maxY - minY + 1;

        if (usableWidth < minWidth || usableHeight < minHeight)
            return grid;

        int width = Random.Range(minWidth, usableWidth + 1);
        int height = Random.Range(minHeight, usableHeight + 1);
        int offsetX = Random.Range(0, usableWidth - width + 1);
        int offsetY = Random.Range(0, usableHeight - height + 1);

        int startX = minX + offsetX;
        int startY = minY + offsetY;

        for (int x = startX; x < startX + width; x++)
            for (int y = startY; y < startY + height; y++)
                grid[x, y] = TileType.Floor;

        centers.Add(new(startX + width / 2, startY + height / 2));
        return grid;
    }
}
