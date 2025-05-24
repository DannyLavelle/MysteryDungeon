using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Prefabs & Settings")]
    public GameObject floor;
    public GameObject wall;
    public int dungeonSizeX = 52;
    public int dungeonSizeY = 28;
    public int roomNumX = 3;
    public int roomNumY = 2;
    public int minRoomX = 5;
    public int minRoomY = 4;

    [SerializeField] private int roomPadding = 1;
    [SerializeField] private int dungeonBorder = 1;
    [Range(0, 1)] public float straightCorridorChance = 0.5f;
    public bool regenerateOnFlag;
    DungeonContainer dungeonGridContainer; 
    private GameObject dungeonContainer;
    private float roomChance = 0.7f;
    private List<RoomBounds> roomBoundsList;
    private List<Vector2Int> roomCenters = new();

    private DungeonSettings defaultSettings;
    public TileType[,] dungeonGrid;



    private void Start()
    {
        dungeonGridContainer = GetComponent<DungeonContainer>();
        defaultSettings = new DungeonSettings(minRoomX, minRoomY, roomNumX, roomNumY, dungeonSizeX, dungeonSizeY);
        GenerateDungeon();
    }

    private void Update()
    {
        if (regenerateOnFlag)
        {
            GenerateDungeon();
            regenerateOnFlag = false;
        }
    }

    public void GenerateDungeon()
    {
        roomCenters.Clear();
        roomBoundsList = new();

        // Clean old dungeon
        if (dungeonContainer != null)
            Destroy(dungeonContainer);

        dungeonContainer = new GameObject("DungeonContainer");
        dungeonContainer.transform.parent = this.transform;

        // Adjust dimensions
        Vector2Int dungeonSize = DungeonUtility.CalculateDungeonSize(
            defaultSettings.width, defaultSettings.height, roomNumX, roomNumY, dungeonBorder, roomPadding);
        dungeonSizeX = dungeonSize.x;
        dungeonSizeY = dungeonSize.y;

        dungeonGrid = dungeonGridContainer.CreateDungeonGrid(dungeonSizeX, dungeonSizeY);
        DungeonUtility.FillWithWalls(dungeonGrid);

        DecideDungeonType();

        // Room splitting
        var splitter = new DungeonSplitter(roomNumX, roomNumY, dungeonBorder);
        roomBoundsList = splitter.Split(dungeonGrid);

        // Room placement
        var roomGen = new DungeonRoomGenerator(minRoomX, minRoomY, roomPadding);
        foreach (var room in roomBoundsList)
        {
            if (Random.value <= roomChance)
            {
                dungeonGrid = roomGen.GenerateRoom(dungeonGrid, room, roomCenters,dungeonGridContainer);
            }
        }

        // Connect rooms
        var connector = new DungeonConnector(straightCorridorChance, dungeonSizeX, dungeonSizeY);
        dungeonGrid = connector.ConnectRooms(dungeonGrid, roomCenters,dungeonGridContainer);


        dungeonGridContainer.dungeon = dungeonGrid;

        // Spawn
        DungeonUtility.SpawnTiles(dungeonGrid, floor, wall, dungeonContainer.transform,dungeonGridContainer);
        
    }

    private void DecideDungeonType()
    {
        var type = DungeonTypeUtility.GetRandom();
        switch (type)
        {
            case DungeonType.SingleRoom:
            minRoomX = Mathf.CeilToInt(defaultSettings.width * 0.9f);
            minRoomY = Mathf.CeilToInt(defaultSettings.height * 0.9f);
            roomNumX = roomNumY = 1;
            roomChance = 1.1f;
            break;
            case DungeonType.fullRoom:
            ResetToDefaultSettings();
            roomChance = 1f;
            break;
            case DungeonType.halfRoom:
            ResetToDefaultSettings();
            roomChance = 0.5f;
            break;
            case DungeonType.threeQuartRoom:
            ResetToDefaultSettings();
            roomChance = 0.75f;
            break;
        }

        //Debug.Log($"Dungeon Type: {type}");
    }

    private void ResetToDefaultSettings()
    {
        minRoomX = defaultSettings.minRoomX;
        minRoomY = defaultSettings.minRoomY;
        roomNumX = defaultSettings.roomNumX;
        roomNumY = defaultSettings.roomNumY;
    }
}
