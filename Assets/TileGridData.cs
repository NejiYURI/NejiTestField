using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridData
{
    public TileBase tileBase;

    public Vector3Int GridLocation;

    public Vector3 WorldLocation;

    public Vector3 TileOffset;

    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool IsBlocked;

    public TileGridData previousTile;
}
