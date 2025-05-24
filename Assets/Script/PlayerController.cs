using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public DungeonContainer dungeonGridContainer;

    public IEnumerator StepTo(Vector2Int target)
    {
        GridPosition = target;
        Vector3 worldPos = new Vector3(target.x, 0, target.y);
        transform.position = worldPos;
        yield return new WaitForSeconds(0.1f);
    }
}
