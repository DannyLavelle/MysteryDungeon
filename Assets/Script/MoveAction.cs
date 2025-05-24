
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveAction : GameAction
{
    private PlayerController player;
    private Vector2Int destination;

    public MoveAction(PlayerController player, Vector2Int dest)
    {
        this.player = player;
        destination = dest;
    }

    public override IEnumerator Execute()
    {
        var path = PathfindingUtility.GetPath(player.GridPosition, destination);

        foreach (var step in path)
        {
            // Check if tile is walkable
            if (IsWalkable(step))
            {
                yield return player.StepTo(step);
                yield return TurnManager.Instance.EnemyTurn();
            }
            else
            {
                Debug.Log("Blocked by wall at: " + step);
                break; // Stop moving if next tile is a wall
            }
        }
    }

    private bool IsWalkable(Vector2Int pos)
    {
        TileType[,] grid = player.dungeonGridContainer.dungeon;
        return grid[pos.x, pos.y] != TileType.Wall;
    }
}
