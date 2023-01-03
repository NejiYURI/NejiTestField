using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CustomTileSystem
{
    public class TileInteractScript : MonoBehaviour
    {
        public static TileInteractScript tileInteract;

        private TileManager tileManager;

        private List<Vector2Int> SelectList;

        public TileData tile_MoveSelect;

        public TileData tile_AtkSelect;

        private Vector2Int? PrevGridPos = null;


        private void Awake()
        {
            tileInteract = this;
        }

        private void Start()
        {
            if (TileManager.tileManager != null)
                tileManager = TileManager.tileManager;
            PrevGridPos = null;
        }
        // Update is called once per frame
        void Update()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int GridPos = tileManager.ToGridVector(mousePosition);
            if (tileManager.HasTile(GridPos) && mousePosition != PrevGridPos)
            {
                if (PrevGridPos != null)
                    TileManager.tileManager.ResetTile((Vector2Int)PrevGridPos);
                TileManager.tileManager.ActiveTile(GridPos);
                PrevGridPos = GridPos;
            }
            else if (GridPos != PrevGridPos && PrevGridPos != null)
            {
                tileManager.ResetTile((Vector2Int)PrevGridPos);
                PrevGridPos = null;
            }
        }

        public void SelectedRange(Vector2Int CenterPos, int Range, bool IsMove)
        {
            if (TileManager.tileManager == null) return;
            SelectList = new List<Vector2Int>();
            int CurRange = Range;
            for (int x = 0; x <= Range; x++)
            {
                for (int y = CurRange - 1; y > -CurRange; y--)
                {
                    if (CenterPos + new Vector2Int(x, y) == CenterPos) continue;
                    if (TileManager.tileManager.HasTile(CenterPos + new Vector2Int(x, y)) && (!IsMove || !TileManager.tileManager.GetTileIsBlock(CenterPos + new Vector2Int(x, y))))
                    {
                        TileManager.tileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                        TileManager.tileManager.SelectTile(CenterPos + new Vector2Int(x, y));
                        SelectList.Add(CenterPos + new Vector2Int(x, y));
                    }
                    if (x != 0)
                        if (TileManager.tileManager.HasTile(CenterPos - new Vector2Int(x, y)) && (!IsMove || !TileManager.tileManager.GetTileIsBlock(CenterPos - new Vector2Int(x, y))))
                        {
                            TileManager.tileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                            TileManager.tileManager.SelectTile(CenterPos - new Vector2Int(x, y));
                            SelectList.Add(CenterPos - new Vector2Int(x, y));
                        }
                }
                CurRange--;
            }


        }

        public void CancelSelectRange()
        {
            if (TileManager.tileManager == null) return;
            foreach (var item in SelectList)
            {
                TileManager.tileManager.CancelSelectTile(item);
            }
            SelectList = new List<Vector2Int>();
        }

        public bool CanSelect(Vector2Int i_targetVector)
        {
            return SelectList.Contains(i_targetVector);
        }

        public void StartWave(Vector2Int i_Center)
        {
            StartCoroutine(TileWave(i_Center));
        }

        IEnumerator TileWave(Vector2Int i_Center)
        {
            if (TileManager.tileManager != null)
            {
                Dictionary<int, List<Vector2Int>> Tiles= TileManager.tileManager.GetListOfRange(i_Center);
                int Index = 0;
                while (Tiles.Count > 0)
                {
                    if (Tiles.ContainsKey(Index))
                    {
                        foreach (Vector2Int grid in Tiles[Index])
                        {
                            bool isSuccess = false;
                            StartCoroutine(SingleTileWave(TileManager.tileManager.GetTileData(grid, out isSuccess).GetTileGameObject().transform));
                        }
                        Tiles.Remove(Index);
                        yield return new WaitForSeconds(0.1f);
                        Index++;
                    }
                }
               
            }
            
        }
        IEnumerator SingleTileWave(Transform targetObj)
        {
            Vector2 pos = (Vector2)targetObj.position;
            targetObj.LeanMove(pos + new Vector2(0, 0.08f), 0.02f);
            yield return new WaitForSeconds(0.08f);
            targetObj.LeanMove(pos - new Vector2(0, 0.04f), 0.03f);
            yield return new WaitForSeconds(0.05f);
            targetObj.LeanMove(pos, 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
