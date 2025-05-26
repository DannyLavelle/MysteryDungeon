using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject target;
    public Stats stats;

    Dictionary<string, int> probabilities = new Dictionary<string, int>
        {
            { "Move", 20 },
            { "Attack", 30 },
            { "Retreat", 50 }
        };

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<Stats>();
    }
    public IEnumerator TakeTurn()
    {
        if (target == null) yield break;

        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);

        List<Vector2Int> path = PathfindingUtility.GetPath(myPos, playerPos);

        // Move one step toward the player (if possible)
        if (path != null && path.Count > 0)
        {
            Vector2Int nextStep = path[0];

            // <-- NEW: only move if nextStep is NOT the player's position
            if (nextStep != playerPos)
            {
                yield return StepTo(nextStep);
            }
            else
            {
                // Optionally, you could attack here instead:
                // yield return AttackPlayer();
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    private void DecidePlan()//decides high level gameplan
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

    private IEnumerator StepTo(Vector2Int position)
    {
        Vector3 targetPos = GridUtility.GridToWorldPosition(position);
        float duration = 0.1f;
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPos;
    }

}
