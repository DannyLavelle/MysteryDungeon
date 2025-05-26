using UnityEngine;
using System.Collections;

public interface IMovable
{
    Vector2Int gridPosition { get; }
    DungeonContainer dungeonGridContainer { get; }
    IEnumerator StepTo(Vector2Int position);
}
