using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public int dungeonSizeX;
    public int dungeonSizeY;
    public int roomMaxSizeX;
    public int roomMaxSizeY;
    public int roomMinSizeX;
    public int roomMinSizeY;

    public enum TileType
    {
        Wall,
        Floor
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDungeoon();
    }

    public void GenerateDungeoon()
    {
        TileType[,] grid = GenerateDungeonLayout(dungeonSizeX, dungeonSizeY, roomMaxSizeX, roomMinSizeY, roomMinSizeX, roomMinSizeY);
        SpawnDungeon(grid);
    }


    public TileType[,] GenerateDungeonLayout(int dungeonSizeX, int dungeonSizeY, int roomMaxSizeX, int roomMaxSizeY, int roomMinSizeX, int roomMinSizeY)
    {
        int cellsX = dungeonSizeX / roomMaxSizeX;
        int cellsY = dungeonSizeY / roomMaxSizeY;

        TileType[,] dungeonGrid = new TileType[dungeonSizeX, dungeonSizeY];


        // Initialize everything as walls
        for (int x = 0; x < dungeonSizeX; x++)
        {
            for (int y = 0; y < dungeonSizeY; y++)
            {
                dungeonGrid[x, y] = TileType.Wall;
            }
        }




        for (int cellX = 0; cellX < cellsX; cellX++)
        {
            for (int cellY = 0; cellY < cellsY; cellY++)
            {
                if (Random.value <= 0.4f)
                    continue;

                dungeonGrid = PlaceRoomInCell(dungeonGrid, cellX, cellY);
            }
        }




        return dungeonGrid;

    }


    private TileType[,] PlaceRoomInCell(TileType[,] dungeonGrid, int cellX, int cellY)
    {
        TileType[,] room = GenerateRoomTypes(roomMaxSizeX, roomMaxSizeY, roomMinSizeX, roomMinSizeY);

        int offsetX = cellX * roomMaxSizeX;
        int offsetY = cellY * roomMaxSizeY;

        for (int x = 0; x < roomMaxSizeX; x++)
        {
            for (int y = 0; y < roomMaxSizeY; y++)
            {
                int globalX = offsetX + x;
                int globalY = offsetY + y;

                if (globalX < dungeonGrid.GetLength(0) && globalY < dungeonGrid.GetLength(1))
                {
                    dungeonGrid[globalX, globalY] = room[x, y];
                }
            }
        }

        return dungeonGrid;
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



    public void SpawnDungeon(TileType[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1); // This is now your Z-axis in world space

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new Vector3(x, 0, z); // Y=0 is ground level

                GameObject prefabToSpawn = null;

                switch (grid[x, z])
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
