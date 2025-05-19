using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public float dungeonSizeX;
    public float dungeonSizeY;
    public int roomMaxSizeX;
    public int roomMaxSizeY;
    public float roomMinSizeX;
    public float roomMinSizeY;

    public enum TileType
    {
        Wall,
        Floor
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GenerateDungeoon()
    {

    }

    
    public TileType[,] GenerateRoomTypes(int maxX, int maxY, int minX, int minY)
    {

        //new grid
        TileType[,] roomGrid = new TileType[maxX, maxY];



        for (int x = 0; x < roomMaxSizeX; x++)// fill room with walls 
        {
            for (int y = 0; y < roomMaxSizeY; y++)
            {
                roomGrid[x, y] = TileType.Wall;
            }
        }

        int floorWidth = Random.Range(minX, maxX + 1);
        int floorHeight = Random.Range(minY, maxY + 1);


        // Choose a spot where the room will stil fit 
        int offsetX = Random.Range(0, roomMaxSizeX - floorWidth + 1);
        int offsetY = Random.Range(0, roomMaxSizeY - floorHeight + 1);



        //Add floors
        for (int x = 0; x < floorWidth; x++)
        {
            for (int y = 0; y < floorHeight; y++)
            {
                roomGrid[x + offsetX, y + offsetY] = TileType.Floor;
            }
        }

        return roomGrid;

    }
}
