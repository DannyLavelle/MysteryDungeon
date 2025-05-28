using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] toSpawn;
    public GameObject player;
    public bool debug;
    private int turnCounter;
    public int turnsToSpawn;
    public DungeonContainer dungeonContainer;
    public bool spawnTurn;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonContainer = FindAnyObjectByType<DungeonContainer>();
        if (dungeonContainer == null)
            Debug.LogError("No DungeonContainer found in scene!");
    }

    private void Update()
    {
        if (debug)
        {
            SpawnEnemy();
            debug = false;
        }
    }

    private Vector2Int GetValidSpawnPosition()
    {
        Vector2Int playerPos = GridUtility.WorldToGridPosition(player.transform.position);

        Vector2Int spawnPos;
        int attempts = 0;
        do
        {
            int rand = Random.Range(0, dungeonContainer.floorTiles.Count);
            spawnPos = new Vector2Int(dungeonContainer.floorTiles[rand].x, dungeonContainer.floorTiles[rand].y);

            attempts++;
            if (attempts > 100) // failsafe to avoid infinite loop
            {
                Debug.LogWarning("Could not find a valid enemy spawn location.");
                break;
            }

        } while (spawnPos == playerPos);

        return spawnPos;
    }

    private void SpawnEnemy()
    {
        Vector2Int spawnPos = GetValidSpawnPosition();
        Transform spawnTransform = dungeonContainer.dungeonObjects[spawnPos.x, spawnPos.y].transform;
        Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], spawnTransform.position, Quaternion.identity);//spawn random enemy
    }

    public void IncreaseTurnCounter()
    {

        if (spawnTurn) return;

        turnCounter++;
        if(turnCounter >= turnsToSpawn)
        {
            turnCounter = 0;
            SpawnEnemy();
        }
           
    }

    public void BatchSpawn(int numberToSpawn)
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            SpawnEnemy();

        }
        if (turnsToSpawn == -1) spawnTurn = true;
    }

}
