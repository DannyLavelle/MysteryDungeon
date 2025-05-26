using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public bool debug;

    DungeonContainer dungeonContainer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dungeonContainer = GetComponent<DungeonContainer>();
    }

    private void Update()
    {
        if (debug)
        {
            Vector2Int spawnPos = GetValidSpawnPosition();
            Transform spawnTransform = dungeonContainer.dungeonObjects[spawnPos.x, spawnPos.y].transform;
            Instantiate(enemy, spawnTransform.position, Quaternion.identity);
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
}
