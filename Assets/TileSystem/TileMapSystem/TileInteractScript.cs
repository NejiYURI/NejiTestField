using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
namespace TileMapSystem
{
    public class TileInteractScript : MonoBehaviour
    {
        public static TileInteractScript tileInteract;

        private List<Vector3Int> SelectList;

        public TileBase tile_MoveSelect;

        public TileBase tile_AtkSelect;


        private void Awake()
        {
            tileInteract = this;
        }
        private Vector3Int prevPos = new Vector3Int();
        // Update is called once per frame
        void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int gridPos = TileManager.tileManager.GetCellPos(mousePosition);
            if (TileManager.tileManager.CheckHasTile(gridPos) && prevPos != gridPos)
            {
                TileManager.tileManager.ResetTile(prevPos);
                TileManager.tileManager.ActiveTile(gridPos);
                prevPos = gridPos;
            }
            else if (!TileManager.tileManager.CheckHasTile(gridPos) && TileManager.tileManager.CheckHasTile(prevPos))
            {
                TileManager.tileManager.ResetTile(prevPos);
                prevPos = new Vector3Int();
            }
        }

        public void SelectedRange(Vector3Int CenterPos, int Range, bool IsMove)
        {
            if (TileManager.tileManager == null) return;
            SelectList = new List<Vector3Int>();
            int CurRange = Range;
            for (int x = 0; x <= Range; x++)
            {
                for (int y = CurRange - 1; y > -CurRange; y--)
                {
                    if (CenterPos + new Vector3Int(x, y) == CenterPos) continue;
                    if (TileManager.tileManager.CheckHasTile(CenterPos + new Vector3Int(x, y)) && (!IsMove || !TileManager.tileManager.GetTileIsBlock(CenterPos + new Vector3Int(x, y))))
                    {
                        TileManager.tileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                        TileManager.tileManager.SelectTile(CenterPos + new Vector3Int(x, y));
                        SelectList.Add(CenterPos + new Vector3Int(x, y));
                    }
                    if (x != 0)
                        if (TileManager.tileManager.CheckHasTile(CenterPos - new Vector3Int(x, y)) && (!IsMove || !TileManager.tileManager.GetTileIsBlock(CenterPos - new Vector3Int(x, y))))
                        {
                            TileManager.tileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                            TileManager.tileManager.SelectTile(CenterPos - new Vector3Int(x, y));
                            SelectList.Add(CenterPos - new Vector3Int(x, y));
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
            SelectList = new List<Vector3Int>();
        }

        public bool CanSelect(Vector3Int i_targetVector)
        {
            return SelectList.Contains(i_targetVector);
        }
    }
}
