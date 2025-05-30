
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveAction : GameAction
{
    private PlayerController player;
    private Vector2Int destination;

    public MoveAction(PlayerController player , Vector2Int dest)
    {
        this.player = player;
        destination = dest;
    }

    //public override IEnumerator Execute()
    //{
    //    var path = PathfindingUtility.GetPath(player.GridPosition, destination);

    //    foreach (var step in path)
    //    {
    //        // Check if tile is walkable
    //        if (IsWalkable(step))
    //        {
    //            yield return player.StepTo(step);
    //            yield return TurnManager.Instance.EnemyTurn();
    //        }
    //        else
    //        {
    //            Debug.Log("Blocked by wall at: " + step);
    //            break; // Stop moving if next tile is a wall
    //        }
    //    }
    //}

    //public override IEnumerator Execute()
    //{
    //    // 1) Get the full path, where path[0] == your current tile
    //    var path = PathfindingUtility.GetPath(player.GridPosition, destination);

    //    // 2) Start the loop at i=1 so you ONLY iterate real moves
    //    for (int i = 1; i < path.Count; i++)
    //    {
    //        Vector2Int step = path[i];

    //        // 3) Wall?check
    //        if (!IsWalkable(step))
    //        {
    //            Debug.Log("Blocked by wall at: " + step);
    //            break;
    //        }

    //        // 4) Move the player one tile
    //        yield return player.StepTo(step);

    //        // 5) Now let the enemy take exactly one turn
    //        yield return TurnManager.Instance.EnemyTurn();
    //    }
    //}

    public override IEnumerator Execute()
    {
        
        var path = PathfindingUtility.GetPath(player.GridPosition, destination);

        foreach (var step in path)
        {
            if (!IsWalkable(step))
            {
                Debug.LogWarning($"Blocked by wall at: {step}");
                break;
            }

            // Player moves exactly one tile
            yield return player.StepTo(step);

            // Then all enemies take exactly one tile-step
            yield return TurnManager.Instance.EnemyTurn();
        }
    }

    private bool IsWalkable(Vector2Int pos)
    {
        TileType[,] grid = player.dungeonGridContainer.dungeon;
        return grid[pos.x, pos.y] != TileType.Wall;
    }
}
