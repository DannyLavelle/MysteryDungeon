using System.Collections.Generic;
using UnityEngine;
using System;

public static class PathfindingUtility
{
    
    public static List<Vector2Int> GetPath(Vector2Int from, Vector2Int to)
    {
        var path = new List<Vector2Int>();
        int x = from.x, y = from.y;
        while (x != to.x || y != to.y)
        {
            int dx = to.x - x;
            int dy = to.y - y;
            x += Math.Sign(dx);
            y += Math.Sign(dy);
            path.Add(new Vector2Int(x, y));
        }
        return path;
    }

    public static int GetPathLength(Vector2Int start, Vector2Int end)
    {
        var path = GetPath(start, end);
        if (path == null || path.Count == 0)
            return int.MaxValue; // unreachable

        return path.Count;
    }
}
