using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject target;
    public Stats stats;
    public float delay;
    public bool debug;
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
        if(debug)
        {
            yield return AttackPlayer();
        }
        else
        {
            yield return MoveStep();

        }
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


    private IEnumerator MoveStep()
    {
        if (target == null) yield break;

        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);

        List<Vector2Int> path = PathfindingUtility.GetPath(myPos, playerPos);

        if (path != null && path.Count > 0)
        {
            Vector2Int nextStep = path[0];

            // 1) Don't step onto the player
            if (nextStep == playerPos)
            {
                yield return new WaitForSeconds(delay);
                yield break;
            }

            // 2) Don't step onto another enemy
            Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var other in allEnemies)
            {
                if (other == this) continue;
                Vector2Int otherPos = GridUtility.WorldToGridPosition(other.transform.position);
                if (nextStep == otherPos)
                {
                    // blocked by another enemy
                    yield return new WaitForSeconds(delay);
                    yield break;
                }
            }

            // 3) If all clear, move
            yield return StepTo(nextStep);
        }
        else
        {
            // no path—just wait
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator AttackPlayer()
    {
       if(CheckInRange(stats.attackRange))
        {

            IEnemyAttackBase enemyAttack = GetComponent<IEnemyAttackBase>();

            Stats playerStats = target.GetComponent<Stats>();

            enemyAttack.Attack(playerStats, stats);
            yield return new WaitForSeconds(delay);
        }
        else
        {
            yield return MoveStep();
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
        CheckForMine();
    }

    private void CheckForMine()
    {
        // 1) Get the enemy's current grid tile
        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);

        // 2) Find all Mine instances in the scene
        GameObject[] allMines = GameObject.FindGameObjectsWithTag("Mine");
        foreach (var mine in allMines)
        {
            
            Vector2Int minePos = GridUtility.WorldToGridPosition(mine.transform.position);

            if (minePos == myPos)
            {
                
                Stats myStats = GetComponent<Stats>();
                Stats mineStats = mine.GetComponent<Stats>();
                if (myStats != null)
                    myStats.TakeDamage(mineStats.damage);

               
                Destroy(mine.gameObject);

               
                break;
            }
        }
    }

    public bool CheckInRange(int range)
    {
        if (target == null) return false;

        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);

        int distance = PathfindingUtility.GetPathLength(myPos, playerPos);
        return distance <= range;
    }

}
