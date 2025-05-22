using UnityEngine;

[System.Serializable]
public class DungeonSettings
{
    public int minRoomX, minRoomY;
    public int roomNumX, roomNumY;
    public int width, height;

    public DungeonSettings(int minX, int minY, int numX, int numY, int width, int height)
    {
        minRoomX = minX;
        minRoomY = minY;
        roomNumX = numX;
        roomNumY = numY;
        this.width = width;
        this.height = height;
    }
}
