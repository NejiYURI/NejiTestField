using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager_TileMap : MonoBehaviour
{
    public Tilemap map;

    public Dictionary<Vector2Int, TileGridData> GridMap;

    public Vector3 TileOffset;

    public TileBase tile_Normal;
    public TileBase tile_Selected;

    private void Awake()
    {
        int xmax = map.cellBounds.max.x;
        int ymax = map.cellBounds.max.y;
        int zmax = map.cellBounds.max.z;
        GridMap = new Dictionary<Vector2Int, TileGridData>();
        for (int z = map.cellBounds.min.z; z < zmax; z++)
        {
            for (int y = map.cellBounds.min.y; y < ymax; y++)
            {
                for (int x = map.cellBounds.min.x; x < xmax; x++)
                {
                    Vector3Int GridLocation = new Vector3Int(x, y, z);
                    Vector2Int getPos = new Vector2Int(x, y);
                    if (map.HasTile(GridLocation) && !GridMap.ContainsKey(getPos))
                    {
                        TileGridData newtile = new TileGridData();
                        newtile.tileBase = map.GetTile(GridLocation);
                        newtile.GridLocation = GridLocation;
                        newtile.WorldLocation = map.CellToWorld(GridLocation);
                        newtile.TileOffset = this.TileOffset;
                        GridMap.Add(getPos, newtile);
                    }

                }
            }
        }
    }

    public bool GetTileData(int index, out TileGridData _Target)
    {
        int count = 0;
        _Target = new TileGridData();
        foreach (var item in GridMap)
        {
            if (count != index)
            {
                count++;
                continue;
            }
            _Target = item.Value;
            return true;
        }
        return false;
    }

    public TileGridData GetTileData(Vector3Int gridPos,out bool IsSuccess)
    {
        IsSuccess = false;
        if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
        {
            IsSuccess = true;
            return GridMap[new Vector2Int(gridPos.x, gridPos.y)];
        }
        return new TileGridData();
    }

    public bool CheckHasTile(Vector3Int i_pos)
    {
        return map.HasTile(i_pos) && GridMap.ContainsKey(new Vector2Int(i_pos.x, i_pos.y));
    }

    public Vector3Int GetCellPos(Vector2 i_pos)
    {
        return map.WorldToCell(i_pos);
    }

    public void ActiveTile(Vector3Int i_pos)
    {
        if (map.HasTile(i_pos))
            map.SetTile(i_pos, tile_Selected);
    }

    public void ResetTile(Vector3Int i_pos)
    {
        if (map.HasTile(i_pos))
            map.SetTile(i_pos, tile_Normal);
    }


    public List<TileGridData> FindPath(TileGridData startTile, TileGridData EndTile)
    {
        List<TileGridData> openList = new List<TileGridData>();
        List<TileGridData> closedList = new List<TileGridData>();

        openList.Add(startTile);
        while (openList.Count > 0)
        {
            TileGridData currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == EndTile)
            {
                //Path Found
                return GetFinishList(startTile, EndTile);
            }

            List<TileGridData> neighborTiles = GetNeighborTiles(currentTile);

            foreach (var neighbor in neighborTiles)
            {
                //1=jump height
                if (neighbor.IsBlocked || closedList.Contains(neighbor) || Mathf.Abs(currentTile.GridLocation.z - neighbor.GridLocation.z) > 1)
                {
                    continue;
                }
                neighbor.G = GetDistance(startTile, neighbor);
                neighbor.H = GetDistance(EndTile, neighbor);

                neighbor.previousTile = currentTile;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<TileGridData>();
    }

    private List<TileGridData> GetFinishList(TileGridData startTile, TileGridData EndTile)
    {
        List<TileGridData> finishList = new List<TileGridData>();

        TileGridData currentTile = EndTile;

        while (currentTile != startTile)
        {
            finishList.Add(currentTile);
            currentTile = currentTile.previousTile;
        }
        finishList.Reverse();
        return finishList;
    }

    private int GetDistance(TileGridData tile_a, TileGridData tile_b)
    {
        return Mathf.Abs(tile_a.GridLocation.x - tile_b.GridLocation.x) + Mathf.Abs(tile_a.GridLocation.y - tile_b.GridLocation.y);
    }

    private List<TileGridData> GetNeighborTiles(TileGridData currentTile)
    {
        List<TileGridData> neighbors = new List<TileGridData>();

        //Top
        Vector2Int CheckLocation = new Vector2Int(
            currentTile.GridLocation.x,
            currentTile.GridLocation.y + 1
            );
        if (GridMap.ContainsKey(CheckLocation))
        {
            neighbors.Add(GridMap[CheckLocation]);
        }

        //Bottom
        CheckLocation = new Vector2Int(
            currentTile.GridLocation.x,
            currentTile.GridLocation.y - 1
            );
        if (GridMap.ContainsKey(CheckLocation))
        {
            neighbors.Add(GridMap[CheckLocation]);
        }

        //Right
        CheckLocation = new Vector2Int(
            currentTile.GridLocation.x + 1,
            currentTile.GridLocation.y
            );
        if (GridMap.ContainsKey(CheckLocation))
        {
            neighbors.Add(GridMap[CheckLocation]);
        }

        //Left
        CheckLocation = new Vector2Int(
            currentTile.GridLocation.x - 1,
            currentTile.GridLocation.y
            );
        if (GridMap.ContainsKey(CheckLocation))
        {
            neighbors.Add(GridMap[CheckLocation]);
        }
        return neighbors;
    }
}
