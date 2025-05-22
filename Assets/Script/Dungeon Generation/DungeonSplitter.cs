using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class DungeonSplitter
{
    private int partsX, partsY, border;

    public DungeonSplitter(int partsX, int partsY, int border)
    {
        this.partsX = partsX;
        this.partsY = partsY;
        this.border = border;
    }

    public List<RoomBounds> Split(TileType[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        int partWidth = (width - 2 * border) / partsX;
        int partHeight = (height - 2 * border) / partsY;

        var result = new List<RoomBounds>();

        for (int y = 0; y < partsY; y++)
        {
            for (int x = 0; x < partsX; x++)
            {
                int startX = x * partWidth + border;
                int startY = y * partHeight + border;
                int endX = startX + partWidth - 1;
                int endY = startY + partHeight - 1;

                result.Add(new RoomBounds(
                    new(startX, startY),
                    new(endX, startY),
                    new(startX, endY),
                    new(endX, endY),
                    new((startX + endX) / 2, (startY + endY) / 2)
                ));
            }
        }

        return result;
    }
}
