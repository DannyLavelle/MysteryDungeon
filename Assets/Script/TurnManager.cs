using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    public abstract IEnumerator Execute();
}

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
            // Move one tile, then let enemies take their step
            yield return player.StepTo(step);
            yield return TurnManager.Instance.EnemyTurn();
        }
    }
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private Queue<GameAction> actionQueue = new Queue<GameAction>();
    private bool processing = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EnqueueAction(GameAction action)
    {
        actionQueue.Enqueue(action);
        if (!processing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        processing = true;
        while (actionQueue.Count > 0)
        {
            yield return actionQueue.Dequeue().Execute();
        }
        processing = false;
    }

    /// <summary>
    /// Called between each player tile?step: each enemy takes its turn in sequence.
    /// </summary>
    public IEnumerator EnemyTurn()
    {
        // Find all active Enemy components in the scene
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var e in enemies)
        {
            yield return e.TakeTurn();
        }
    }
}
