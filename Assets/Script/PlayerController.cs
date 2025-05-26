using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int GridPosition;
    public Vector2Int LastMoveDirection { get; private set; } = Vector2Int.zero;
    public DungeonContainer dungeonGridContainer;


    private void Start()
    {
        dungeonGridContainer = FindAnyObjectByType<DungeonContainer>();
    }
    public IEnumerator StepTo(Vector2Int target)
    {
        
        LastMoveDirection = target - GridPosition;

      
        GridPosition = target;
        Vector3 worldPos = new Vector3(target.x, 0, target.y);
        transform.position = worldPos;

        yield return new WaitForSeconds(0.1f);
    }

}
