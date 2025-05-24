using UnityEngine;
using System.Collections.Generic;
public class DungeonContainer : MonoBehaviour
{
    public  TileType[,] dungeon;
    public  List<(int x, int y)> floorTiles = new List<(int x, int y)>();
    public  List<(int x, int y)> corridoorTiles = new List<(int x, int y)>();
    public GameObject[,] dungeonObjects;
    public bool debug;
    public TileType[,] CreateDungeonGrid(int x, int y)
    {
        dungeon = new TileType[x, y];
        dungeonObjects = new GameObject[x, y];
        clearLists();
        return dungeon;
    }

    public void clearLists()
    {
        floorTiles.Clear();
        corridoorTiles.Clear();
    }

    private void Update()
    {
        //if(debug)
        //{
        //    foreach(var b in dungeonObjects)
        //    {
        //        Destroy(b);
        //    }
        //}
    }

}
