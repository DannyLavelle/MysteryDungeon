using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction
{
    public abstract IEnumerator Execute();
}


public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private Queue<GameAction> actionQueue = new Queue<GameAction>();
    private bool processing = false;
   public EnemySpawner enemySpawner;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
       
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


    public IEnumerator EnemyTurn()
    {
        enemySpawner.IncreaseTurnCounter();
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var e in enemies)
        {
            if (e == null)
                continue; 
            yield return e.TakeTurn();
        }
    }

}
