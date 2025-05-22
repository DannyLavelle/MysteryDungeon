using UnityEngine;

public struct RoomBounds
{
    public Vector2Int topLeft, topRight, bottomLeft, bottomRight, center;

    public RoomBounds(Vector2Int tl, Vector2Int tr, Vector2Int bl, Vector2Int br, Vector2Int c)
    {
        topLeft = tl; topRight = tr;
        bottomLeft = bl; bottomRight = br;
        center = c;
    }
}
