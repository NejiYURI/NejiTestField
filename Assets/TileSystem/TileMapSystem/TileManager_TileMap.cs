using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMapSystem
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager tileManager;

        public Tilemap map;

        public Dictionary<Vector2Int, TileGridData> GridMap;

        public Vector3 TileOffset;

        public TileBase tile_Normal;
        public TileBase tile_SelectedByUser;
        public TileBase tile_SelectedBySystem;

        private void Awake()
        {
            tileManager = this;
            int xmax = map.cellBounds.max.x;
            int ymax = map.cellBounds.max.y;
            int zmax = map.cellBounds.max.z;
            GridMap = new Dictionary<Vector2Int, TileGridData>();
            for (int z = map.cellBounds.min.z; z < zmax; z++)
            {
                for (int y = map.cellBounds.min.y, gy = 0; y < ymax; y++, gy++)
                {
                    for (int x = map.cellBounds.min.x, gx = 0; x < xmax; x++, gx++)
                    {
                        Vector3Int GridLocation = new Vector3Int(x, y, z);
                        Vector2Int getPos = new Vector2Int(x, y);

                        if (map.HasTile(GridLocation) && !GridMap.ContainsKey(getPos))
                        {
                            TileGridData newtile = new TileGridData();
                            newtile.ArrayIndex = new Vector2Int(gy, gx);
                            newtile.TileLocation = GridLocation;
                            newtile.WorldLocation = map.CellToWorld(GridLocation);
                            newtile.TileOffset = this.TileOffset;
                            //Debug.Log("[" + x + "," + y + "," + z +"]");
                            //Debug.Log("[" + gx + "," + gy + "]");
                            GridMap.Add(getPos, newtile);
                        }

                    }
                }
            }
        }

        public TileGridData GetTileData(Vector3Int gridPos, out bool IsSuccess)
        {
            IsSuccess = false;
            if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
            {
                IsSuccess = true;
                return GridMap[new Vector2Int(gridPos.x, gridPos.y)];
            }
            return new TileGridData();
        }

        public TileGridData GetTileData(Vector2Int gridPos, out bool IsSuccess)
        {
            IsSuccess = false;
            if (GridMap.ContainsKey(gridPos))
            {
                IsSuccess = true;
                return GridMap[gridPos];
            }
            return new TileGridData();
        }

        public void CharacterInTile(Vector3Int gridPos, IF_GameCharacter if_Character)
        {
            if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
            {
                SetTileBlock(gridPos, true);
                GridMap[new Vector2Int(gridPos.x, gridPos.y)].CharacterOnTile = if_Character;
            }
        }


        public void CharacterLeaveTile(Vector3Int gridPos)
        {
            if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
            {
                SetTileBlock(gridPos, false);
                GridMap[new Vector2Int(gridPos.x, gridPos.y)].CharacterOnTile = null;
            }
        }
        public void SetTileBlock(Vector3Int gridPos, bool _IsBlock)
        {
            if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
            {
                GridMap[new Vector2Int(gridPos.x, gridPos.y)].IsBlocked = _IsBlock;
            }
        }

        public void SetSelectTileStyle(TileBase _tileBase)
        {
            tile_SelectedBySystem = _tileBase;
        }

        public bool GetTileIsBlock(Vector3Int gridPos)
        {
            if (GridMap.ContainsKey(new Vector2Int(gridPos.x, gridPos.y)))
            {
                return GridMap[new Vector2Int(gridPos.x, gridPos.y)].IsBlocked;
            }
            return false;
        }

        public bool CheckHasTile(Vector3Int i_pos)
        {
            return map.HasTile(i_pos) && GridMap.ContainsKey(new Vector2Int(i_pos.x, i_pos.y));
        }

        public bool CheckHasCharacter(Vector3Int i_pos)
        {
            if (!CheckHasTile(i_pos)) return false;
            bool GetSuccess = false;
            TileGridData gridData = GetTileData(i_pos, out GetSuccess);
            if (!GetSuccess) return false;
            return gridData.CharacterOnTile != null;
        }

        public Vector3Int GetCellPos(Vector2 i_pos)
        {
            return map.WorldToCell(i_pos);
        }

        public void ActiveTile(Vector3Int i_pos)
        {
            if (map.HasTile(i_pos))
                map.SetTile(i_pos, tile_SelectedByUser);
        }

        public void SelectTile(Vector3Int i_pos)
        {
            if (map.HasTile(i_pos) && GridMap.ContainsKey(new Vector2Int(i_pos.x, i_pos.y)))
            {
                GridMap[new Vector2Int(i_pos.x, i_pos.y)].IsSelected = true;
                map.SetTile(i_pos, tile_SelectedBySystem);
            }

        }

        public void CancelSelectTile(Vector3Int i_pos)
        {
            if (map.HasTile(i_pos) && GridMap.ContainsKey(new Vector2Int(i_pos.x, i_pos.y)))
            {
                GridMap[new Vector2Int(i_pos.x, i_pos.y)].IsSelected = false;
                map.SetTile(i_pos, tile_Normal);
            }

        }

        public void ResetTile(Vector3Int i_pos)
        {
            if (map.HasTile(i_pos) && GridMap.ContainsKey(new Vector2Int(i_pos.x, i_pos.y)))
            {
                if (GridMap[new Vector2Int(i_pos.x, i_pos.y)].IsSelected)
                {
                    map.SetTile(i_pos, tile_SelectedBySystem);
                }
                else
                {
                    map.SetTile(i_pos, tile_Normal);
                }

            }

        }


        public List<TileGridData> FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            bool getSuccess = false;
            TileGridData startTile = GetTileData(startPos, out getSuccess);
            if (!getSuccess) return new List<TileGridData>();
            TileGridData EndTile = GetTileData(endPos, out getSuccess);
            if (!getSuccess) return new List<TileGridData>();

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
                    if (neighbor.IsBlocked || closedList.Contains(neighbor) || Mathf.Abs(currentTile.TileLocation.z - neighbor.TileLocation.z) > 1)
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
            return Mathf.Abs(tile_a.TileLocation.x - tile_b.TileLocation.x) + Mathf.Abs(tile_a.TileLocation.y - tile_b.TileLocation.y);
        }

        private List<TileGridData> GetNeighborTiles(TileGridData currentTile)
        {
            List<TileGridData> neighbors = new List<TileGridData>();

            //Top
            Vector2Int CheckLocation = new Vector2Int(
                currentTile.TileLocation.x,
                currentTile.TileLocation.y + 1
                );
            if (GridMap.ContainsKey(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Bottom
            CheckLocation = new Vector2Int(
                currentTile.TileLocation.x,
                currentTile.TileLocation.y - 1
                );
            if (GridMap.ContainsKey(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Right
            CheckLocation = new Vector2Int(
                currentTile.TileLocation.x + 1,
                currentTile.TileLocation.y
                );
            if (GridMap.ContainsKey(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Left
            CheckLocation = new Vector2Int(
                currentTile.TileLocation.x - 1,
                currentTile.TileLocation.y
                );
            if (GridMap.ContainsKey(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }
            return neighbors;
        }
    }
}

