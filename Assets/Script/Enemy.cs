using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject target;
    public Stats stats;
    public float delay;
    public bool debug;
    private int closeAlly;
    DungeonContainer dungeonContainer;
    Dictionary<string, float> probabilities = new Dictionary<string, float>
        {
            { "Move", 20f },
            { "Attack", 30f },
            { "Retreat", 50f }
        };
    Dictionary<string, bool> locks  = new Dictionary<string, bool>
{
    { "Move",    false },
    { "Attack",  false },
    { "Retreat", false  }  
};

    //private bool attackLock, moveLock, retreatLock;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<Stats>();
        dungeonContainer = FindFirstObjectByType<DungeonContainer>();
    }

    public IEnumerator TakeTurn()
    {
        int rand = UnityEngine.Random.Range(0, 100 + (100 -(10*stats.intelligence)));

       if(rand < probabilities["Move"])
        {
            yield return RandomMoveStep();
        }
       else if (rand < (probabilities["Move"] + probabilities["Attack"]))
        {
            yield return AttackPlayer();
        }
        else if (rand < (probabilities["Move"] + probabilities["Attack"] + probabilities["Retreat"]))
        {
            yield return  RetreatStep();
        }

            if (debug)
        {
            yield return AttackPlayer();
        }
        else
        {
            yield return MoveStep();

        }


        foreach(KeyValuePair<string,bool> kvs in locks)//unlocks for next turn
        {
            locks[kvs.Key] = false;
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
        

        if(distanceToPlayer > stats.detectionRange)
        {
           HandlePercentages(probabilities, "Move", 100f, isAbsolute: true, locks);
            locks["Move"] = true;
            Debug.Log("Not in range");

            return;
        }//if not in detection just move about

        if(stats.intelligence ==0)
        {
            HandlePercentages(probabilities, "Attack", 100f, isAbsolute: true, locks);
            locks["Attack"] = true;
        }



        //int closestEnemyDist = int.MaxValue;
        foreach (Enemy other in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (other == this) continue;

            Vector2Int otherPos = GridUtility.WorldToGridPosition(other.transform.position);
            int dist = PathfindingUtility.GetPathLength(myPos, otherPos);
            if (dist < stats.allyThreshold) closeAlly ++;
        }

        switch(stats.personality)
        {
            case PersonalityType.Shy:
            HandlePercentages(probabilities, "Attack", -5*closeAlly, isAbsolute: false, locks);
            break;
            case PersonalityType.Aggressive:
            HandlePercentages(probabilities, "Attack", 10 * closeAlly, isAbsolute: false, locks);
            break;
        }//emboldens based on personality type and number of close allies

     
        if (stats.currentHealth < 5)//retreat if low health
        {
            if (stats.personality == PersonalityType.Shy)
            {
                HandlePercentages(probabilities, "Retreat", 80 , isAbsolute: false, locks);
            }
            else
            {
                HandlePercentages(probabilities, "Retreat", -20 , isAbsolute: false, locks);
            }
        }

        if (playerStats.currentHealth < stats.damage)//Embolden if can kill
        {
            if (stats.personality == PersonalityType.Shy)
            {
                HandlePercentages(probabilities, "Attack", 50, isAbsolute: false, locks);
            }
            else
            {
                HandlePercentages(probabilities, "Attack", 80, isAbsolute: false, locks);
            }
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



    //public void handlePercentage(string type, float amount, int mult, bool lockProb)
    //{
    //    switch(type)
    //    {
    //        case "Move":
    //        if (moveLock) return;
    //        break;
    //        case "Attack":
    //        if (attackLock) return;
    //        break;
    //        case "Retreat":
    //        if (retreatLock) return;
    //        break;
    //    }
    //    if (probabilities[type] + amount < 0 || probabilities[type] + amount > 100) amount = probabilities[type];
       

    //    probabilities[type] += amount;
    //    if(lockProb)
    //    {
    //        switch (type)
    //        {
    //            case "Move":
    //            moveLock = true;
    //            break;
    //            case "Attack":
    //            attackLock = true;
    //            break;
    //            case "Retreat":
    //            retreatLock = true;
    //            break;
    //        }

    //        lockNumber++;

    //    }

    //    float remainingCatagories = probabilities.Count - lockNumber;

    //    float toAllocate = amount/remainingCatagories;

    //    foreach(KeyValuePair<string, float> kvp in probabilities)
    //    {
    //        if (type == kvp.Key) continue;
    //        if (CheckLock(kvp.Key)) continue;
    //        probabilities[kvp.Key] += (toAllocate * mult *-1);

    //    }

        

    //}

    public static void HandlePercentages(
    Dictionary<string, float> probs,
    string key,
    float amount,
    bool isAbsolute,
    Dictionary<string, bool> locks
)
    {
        if (!probs.ContainsKey(key))
            throw new ArgumentException($"Key '{key}' not found in probabilities.");

       
        if (locks.TryGetValue(key, out bool locked) && locked)
            return;

        float oldValue = probs[key];
        float newValue = isAbsolute ? amount : oldValue + amount;
        newValue = Mathf.Clamp(newValue, 0f, 100f);
        probs[key] = newValue;


        float sumLocked = 0f;
        float sumUnlockedOthers = 0f;
        foreach (var kv in probs)
        {
            if (kv.Key == key) continue;
            if (locks.TryGetValue(kv.Key, out bool isLocked) && isLocked)
                sumLocked += kv.Value;
            else
                sumUnlockedOthers += kv.Value;
        }

      
        float remainder = 100f - newValue - sumLocked;


        if (sumUnlockedOthers <= 0f)
            return;

       
        foreach (var k in new List<string>(probs.Keys))
        {
            if (k == key)
                continue;

            if (locks.TryGetValue(k, out bool isLocked2) && isLocked2)
                continue;

            float oldOther = probs[k];
            float share = oldOther / sumUnlockedOthers;
            probs[k] = Mathf.Clamp(remainder * share, 0f, 100f);
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

    //bool CheckLock(string type)
    //{
    //    switch (type)
    //    {
    //        case "Move":
    //        if (moveLock) return true;
    //        break;
    //        case "Attack":
    //        if (attackLock) return true;
    //        break;
    //        case "Retreat":
    //        if (retreatLock) return true;
    //        break;
    //    }
    //    return false;
    //}


    public IEnumerator RetreatStep()
    {
        if (target == null)
            yield break;

        // Current grid positions
        Vector2Int myPos = GridUtility.WorldToGridPosition(transform.position);
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);

        // 8?way direction from player to me:
        int dx = myPos.x - playerPos.x;
        int dy = myPos.y - playerPos.y;
        Vector2Int dir = new Vector2Int(Mathf.Clamp(dx, -1, 1),
                                        Mathf.Clamp(dy, -1, 1));

        if (dir == Vector2Int.zero)
        {
            // On same tile? just wait
            yield return new WaitForSeconds(delay);
            yield break;
        }

        Vector2Int dest = myPos + dir;

        // Check bounds and walkability
        // (assumes dungeonContainer.dungeon is your TileType[,] grid)
        var grid = dungeonContainer.dungeon;
        int w = grid.GetLength(0), h = grid.GetLength(1);
        if (dest.x < 0 || dest.x >= w || dest.y < 0 || dest.y >= h
            || grid[dest.x, dest.y] == TileType.Wall)
        {
            yield return new WaitForSeconds(delay);
            yield break;
        }

        // Avoid other enemies
        foreach (var other in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (other == this) continue;
            Vector2Int otherPos = GridUtility.WorldToGridPosition(other.transform.position);
            if (dest == otherPos)
            {
                yield return new WaitForSeconds(delay);
                yield break;
            }
        }

        // Step away
        yield return StepTo(dest);
    }

    public IEnumerator RandomMoveStep()
    {
        if (dungeonContainer == null || dungeonContainer.floorTiles.Count == 0)
            yield break;

 
        var randomIndex = UnityEngine.Random.Range(0, dungeonContainer.floorTiles.Count);
        Vector2Int dest = new Vector2Int(dungeonContainer.floorTiles[randomIndex].x, dungeonContainer.floorTiles[randomIndex].y);

        
        Vector2Int playerPos = GridUtility.WorldToGridPosition(target.transform.position);
        if (dest == playerPos)
        {
            yield return new WaitForSeconds(delay);
            yield break;
        }

       
        foreach (var other in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if (other == this) continue;
            Vector2Int otherPos = GridUtility.WorldToGridPosition(other.transform.position);
            if (dest == otherPos)
            {
                yield return new WaitForSeconds(delay);
                yield break;
            }
        }

       
        yield return StepTo(dest);
    }
}
