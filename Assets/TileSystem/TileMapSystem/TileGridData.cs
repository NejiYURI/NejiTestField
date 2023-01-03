using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMapSystem
{
    public class TileGridData
    {
        public Vector2Int ArrayIndex;

        public Vector3Int TileLocation;

        public Vector3 WorldLocation;

        public Vector3 TileOffset;

        public int G;
        public int H;

        public int F { get { return G + H; } }

        public bool IsBlocked;

        public TileGridData previousTile;

        public bool IsSelected;

        public IF_GameCharacter CharacterOnTile;

        public Vector3 GetPosOnTile()
        {
            return WorldLocation + TileOffset;
        }
    }
}

