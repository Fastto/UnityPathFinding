using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static CellType[,] _map { get; private set; }
    private static int _worldWidth;
    private static int _worldHeight;

    public static void Init(List<GameObject> items, int WorldWidth, int WorldHeight)
    {
        _worldWidth = WorldWidth;
        _worldHeight = WorldHeight;
        _map = new CellType[WorldWidth, WorldHeight];

        foreach (GameObject item in items)
        {
            _map[(int) item.transform.position.x, (int) item.transform.position.z] =
                (CellType) Enum.Parse(typeof(CellType), item.tag);
        }

        // for (int x = 0; x < WorldWidth; x++)
        // {
        //     string row = "";
        //     for (int y = 0; y < WorldHeight; y++)
        //     {
        //         row += _map[x, y] + " ";
        //     }
        //     Debug.Log(x + ": " + row);
        // }
    }

    public static bool Exist(Vector2Int position)
    {
        if (position.x >= 0 && position.x < _worldWidth && position.y >= 0 && position.y < _worldHeight) return true;

        return false;
    }
}