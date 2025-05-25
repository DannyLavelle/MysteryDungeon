using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject target;
    public Stats stats;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<Stats>();
    }

    public IEnumerator TakeTurn()
    {
        


        yield return new WaitForSeconds(0.1f);
    }

    private void DecideAction()
    {
        if (target == null) return;

        Stats playerStats = target.GetComponent<Stats>();
        if (playerStats == null) return;

        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);

        int distanceToPlayer = PathfindingUtility.GetPathLength(myPos, playerPos);

        int closestEnemyDist = int.MaxValue;
        foreach (Enemy other in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (other == this) continue;

            Vector2Int otherPos = GridUtility.WorldToGridPosition(other.transform.position);
            int dist = PathfindingUtility.GetPathLength(myPos, otherPos);
            if (dist < closestEnemyDist) closestEnemyDist = dist;
        }

        Debug.Log($"Enemy: HP={stats.currentHealth}, DMG={stats.damage}, " +
                  $"DistToPlayer={distanceToPlayer}, PlayerHP={playerStats.currentHealth}, " +
                  $"DistToEnemy={closestEnemyDist}");

        // Example decision logic
        if (stats.currentHealth < 10)
        {
            Debug.Log("Retreating due to low health");
        }
        else if (distanceToPlayer <= 1)
        {
            Debug.Log("Attacking player!");
        }
        else
        {
            Debug.Log("Chasing player");
        }
    }

}
