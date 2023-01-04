using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomTileSystem
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager tileManager;

        public GameObject basicTile;

        [Header("Tile Setting")]
        public TileData tile_Normal;
        public TileData tile_SelectedByUser;
        public TileData tile_SelectedBySystem;

        public Transform SpawnTarget;

        public int Row = 5;
        public int Col = 5;
        public float TileSize = 1;

        [SerializeField]
        private Dictionary<Vector2Int, TileGridData> GridMap;

        private Isometric_Matrix GirdMatrix = new Isometric_Matrix();

        private float Timer;

        private void Awake()
        {
            tileManager = this;
            GridMap = new Dictionary<Vector2Int, TileGridData>();
            GenerateBaseTile();
        }
        void Start()
        {
            Timer = 0;
        }
        private void FixedUpdate()
        {
            #region-Fancy Move
            //Timer += Time.deltaTime;
            //for (int i = 0; i < Row; i++)
            //{
            //    for (int j = 0; j < Col; j++)
            //    {
            //        if (HasTile(new Vector2(i, j)))
            //        {
            //            GameObject obj = GridMap[new Vector2(i, j)];
            //            float ypos = Mathf.Sin(Timer * 10f + i + j) * 0.02f;
            //            obj.transform.position += new Vector3(0, ypos, 0);
            //        }
            //    }

            //}
            #endregion
        }

        #region-API
        public TileGridData GetTileData(Vector2Int gridPos, out bool IsSuccess)
        {
            IsSuccess = false;
            if (HasTile(gridPos))
            {
                IsSuccess = true;
                return GridMap[gridPos];
            }
            return new TileGridData();
        }

        public Vector2 GetTileWorldPosition(Vector2Int gridPos, out bool IsSuccess)
        {
            IsSuccess = false;
            if (HasTile(gridPos))
            {
                IsSuccess = true;
                return GridMap[gridPos].WorldLocation;
            }
            return new Vector2();
        }

        public bool HasTile(Vector2Int i_pos)
        {
            return GridMap.ContainsKey(i_pos);
        }

        public void CharacterInTile(Vector2Int gridPos, IF_GameCharacter if_Character)
        {
            if (HasTile(gridPos))
            {
                SetTileBlock(gridPos, true);
                GridMap[gridPos].CharacterOnTile = if_Character;
            }
        }


        public void CharacterLeaveTile(Vector2Int gridPos)
        {
            if (HasTile(gridPos))
            {
                SetTileBlock(gridPos, false);
                GridMap[gridPos].CharacterOnTile = null;
            }
        }

        public void ClearAllCharacterOnTile()
        {
            foreach (var tile in GridMap)
            {
                SetTileBlock(tile.Value.GridPosition, false);
            }
        }
        private void SetTileBlock(Vector2Int gridPos, bool _IsBlock)
        {
            GridMap[gridPos].IsBlocked = _IsBlock;
        }

        public void SetSelectTileStyle(TileData _tileBase)
        {
            tile_SelectedBySystem = _tileBase;
        }

        public bool GetTileIsBlock(Vector2Int gridPos)
        {
            if (HasTile(gridPos))
            {
                return GridMap[gridPos].IsBlocked;
            }
            return false;
        }

        public bool CheckHasCharacter(Vector2Int i_pos)
        {
            if (!HasTile(i_pos)) return false;
            bool GetSuccess = false;
            TileGridData gridData = GetTileData(i_pos, out GetSuccess);
            if (!GetSuccess) return false;
            return gridData.CharacterOnTile != null;
        }

        public void ActiveTile(Vector2Int i_pos)
        {
            if (HasTile(i_pos))
                GridMap[i_pos].SetTile(tile_SelectedByUser);
        }

        public void SelectTile(Vector2Int i_pos)
        {
            if (HasTile(i_pos))
            {
                GridMap[i_pos].IsSelected = true;
                GridMap[i_pos].SetTile(tile_SelectedBySystem);
            }

        }

        public void CancelSelectTile(Vector2Int i_pos)
        {
            if (HasTile(i_pos))
            {
                GridMap[i_pos].IsSelected = false;
                GridMap[i_pos].SetTile(tile_Normal);
            }

        }

        public void ResetTile(Vector2Int i_pos)
        {
            if (HasTile(i_pos))
            {
                if (GridMap[i_pos].IsSelected)
                {
                    GridMap[i_pos].SetTile(tile_SelectedBySystem);
                }
                else
                {
                    GridMap[i_pos].SetTile(tile_Normal);
                }

            }

        }
        #endregion
        #region-TileGenerateMethods
        public void GenerateBaseTile()
        {
            ClearBaseTile();

            for (int x = 0; x < Row; x++)
            {
                for (int y = 0; y < Col; y++)
                {
                    Transform SpawnBase = this.transform;
                    if (SpawnTarget != null)
                        SpawnBase = SpawnTarget;
                    Vector2Int GridPos = new Vector2Int(x, y);
                    Vector2 pos = ToScreenVector(GridPos, TileSize);
                    pos += (Vector2)SpawnBase.position;
                    GameObject newTile = Instantiate(basicTile, pos, Quaternion.identity);
                    TileGridData NewGridData = new TileGridData();
                    if (newTile.GetComponent<TileObject>() != null)
                    {
                        newTile.GetComponent<TileObject>().TileSet(tile_Normal);
                        newTile.name = "Tile[" + x + "," + y + "] of pos [" + pos.x + "," + pos.y + "]";
                        newTile.transform.localScale = new Vector2(TileSize, TileSize);
                        NewGridData.SetTileObject(newTile);
                        NewGridData.GridPosition = GridPos;
                        NewGridData.WorldLocation = pos;
                        if (SpawnTarget != null)
                            newTile.transform.SetParent(SpawnTarget);
                        else
                            newTile.transform.SetParent(this.transform);
                        if (!GridMap.ContainsKey(GridPos))
                        {
                            //Debug.Log("Add Key " + GridPos);
                            GridMap.Add(GridPos, NewGridData);
                        }

                    }
                }
            }
        }

        public Dictionary<int, List<Vector2Int>> GetListOfRange(Vector2Int i_CenterPos)
        {
            if (!HasTile(i_CenterPos)) return new Dictionary<int, List<Vector2Int>>();
            Dictionary<int, List<Vector2Int>> Rslt = new Dictionary<int, List<Vector2Int>>();
            foreach (var grid in GridMap)
            {
                int Distance = GetDistance(grid.Value, GridMap[i_CenterPos]);
                if (!Rslt.ContainsKey(Distance))
                {
                    Rslt.Add(Distance, new List<Vector2Int>());

                }
                Rslt[Distance].Add(grid.Value.GridPosition);
            }
            return Rslt;
        }

        public void ClearBaseTile()
        {
            GridMap = new Dictionary<Vector2Int, TileGridData>();
            if (SpawnTarget != null)
            {
                while (SpawnTarget.childCount > 0)
                {
                    GameObject child = SpawnTarget.GetChild(0).gameObject;
                    child.transform.SetParent(null);
#if UNITY_EDITOR
                    if (Application.isPlaying)
                    {

                        Destroy(child);
                    }
                    else
                        DestroyImmediate(child);
#else
                    Destroy(transform.GetChild(0).gameObject);
#endif
                }

            }
            else
                while (transform.childCount > 0)
                {
                    GameObject child = transform.GetChild(0).gameObject;
                    child.transform.SetParent(null);
#if UNITY_EDITOR
                    if (Application.isPlaying)
                    {
                        Destroy(child);
                    }
                    else
                        DestroyImmediate(child);
#else
                    Destroy(transform.GetChild(0).gameObject);
#endif
                }
        }

        private Vector2 ToScreenVector(Vector2 i_vector, float i_tilesize = 1)
        {
            return (new Vector2(i_vector.x * GirdMatrix.i_hat.x + i_vector.y * GirdMatrix.j_hat.x, i_vector.x * GirdMatrix.i_hat.y + i_vector.y * GirdMatrix.j_hat.y)) * i_tilesize;
        }

        public Vector2Int ToGridVector(Vector2 i_vector, float i_tilesize = 1)
        {
            Transform SpawnBase = this.transform;
            if (SpawnTarget != null)
                SpawnBase = SpawnTarget;
            i_vector -= (Vector2)SpawnBase.position;
            float a = GirdMatrix.i_hat.x;
            float b = GirdMatrix.j_hat.x;
            float c = GirdMatrix.i_hat.y;
            float d = GirdMatrix.j_hat.y;
            Isometric_Matrix inv = invert_matrix(a, b, c, d);
            return (new Vector2Int((int)Mathf.Round((i_vector.x * inv.i_hat.x + i_vector.y * inv.j_hat.x) / TileSize), (int)Mathf.Round((i_vector.x * inv.i_hat.y + i_vector.y * inv.j_hat.y) / TileSize)));
        }

        private Isometric_Matrix invert_matrix(float a, float b, float c, float d)
        {
            float det = (1 / (a * d - b * c));
            return new Isometric_Matrix(new Vector2(det * d, det * -1f * c), new Vector2(det * -1f * b, det * a));
        }
        #endregion
        #region-PathFinding
        public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos, bool ToClosest = false)
        {
            bool getSuccess = false;
            TileGridData startTile = GetTileData(startPos, out getSuccess);
            if (!getSuccess) return new List<Vector2Int>();
            TileGridData EndTile = GetTileData(endPos, out getSuccess);
            if (!getSuccess) return new List<Vector2Int>();

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
                    if ((neighbor.IsBlocked || closedList.Contains(neighbor)) && EndTile != neighbor)
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

            return new List<Vector2Int>();
        }

        private List<Vector2Int> GetFinishList(TileGridData startTile, TileGridData EndTile)
        {
            List<Vector2Int> finishList = new List<Vector2Int>();

            TileGridData currentTile = EndTile;

            while (currentTile != startTile)
            {
                finishList.Add(currentTile.GridPosition);
                currentTile = currentTile.previousTile;
            }
            finishList.Reverse();
            return finishList;
        }

        private int GetDistance(TileGridData tile_a, TileGridData tile_b)
        {
            return Mathf.Abs(tile_a.GridPosition.x - tile_b.GridPosition.x) + Mathf.Abs(tile_a.GridPosition.y - tile_b.GridPosition.y);
        }

        public int GetDistance(Vector2Int tile_a, Vector2Int tile_b)
        {
            return Mathf.Abs(tile_a.x - tile_b.x) + Mathf.Abs(tile_a.y - tile_b.y);
        }

        private List<TileGridData> GetNeighborTiles(TileGridData currentTile)
        {
            List<TileGridData> neighbors = new List<TileGridData>();

            //Top
            Vector2Int CheckLocation = new Vector2Int(
                currentTile.GridPosition.x,
                currentTile.GridPosition.y + 1
                );
            if (HasTile(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Bottom
            CheckLocation = new Vector2Int(
                currentTile.GridPosition.x,
                currentTile.GridPosition.y - 1
                );
            if (HasTile(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Right
            CheckLocation = new Vector2Int(
                currentTile.GridPosition.x + 1,
                currentTile.GridPosition.y
                );
            if (HasTile(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }

            //Left
            CheckLocation = new Vector2Int(
                currentTile.GridPosition.x - 1,
                currentTile.GridPosition.y
                );
            if (HasTile(CheckLocation))
            {
                neighbors.Add(GridMap[CheckLocation]);
            }
            return neighbors;
        }
        #endregion

    }
}

