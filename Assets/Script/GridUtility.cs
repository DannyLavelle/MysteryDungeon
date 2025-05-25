using UnityEngine;

public static class GridUtility
{
    public static float tileSize = 1f; 

    
    public static Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / tileSize);
        int y = Mathf.RoundToInt(worldPos.z / tileSize); 
        return new Vector2Int(x, y);
    }

   
    public static Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        float x = gridPos.x * tileSize;
        float z = gridPos.y * tileSize;
        return new Vector3(x, 0, z); 
    }
}
