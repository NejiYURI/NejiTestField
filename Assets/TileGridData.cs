using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTile", menuName = "TileObj/NewTileData")]
public class TileGridData : ScriptableObject
{
    public Vector3Int GridLocation;

    public Vector3 WorldLocation;

    public Vector3 TileOffset;

    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool IsBlocked;

    public TileGridData previousTile;

    public bool IsSelected;
}
