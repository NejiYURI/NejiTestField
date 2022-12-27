using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

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
        Vector3Int gridPos = TileManager_TileMap.TileManager.GetCellPos(mousePosition);
        if (TileManager_TileMap.TileManager.CheckHasTile(gridPos) && prevPos != gridPos)
        {
            TileManager_TileMap.TileManager.ResetTile(prevPos);
            TileManager_TileMap.TileManager.ActiveTile(gridPos);
            prevPos = gridPos;
        }
        else if (!TileManager_TileMap.TileManager.CheckHasTile(gridPos) && TileManager_TileMap.TileManager.CheckHasTile(prevPos))
        {
            TileManager_TileMap.TileManager.ResetTile(prevPos);
            prevPos = new Vector3Int();
        }
    }

    public void SelectedRange(Vector3Int CenterPos, int Range, bool IsMove)
    {
        if (TileManager_TileMap.TileManager == null) return;
        SelectList = new List<Vector3Int>();
        int CurRange = Range;
        for (int x = 0; x <= Range; x++)
        {
            for (int y = CurRange - 1; y > -CurRange; y--)
            {
                if (CenterPos + new Vector3Int(x, y) == CenterPos) continue;
                if (TileManager_TileMap.TileManager.CheckHasTile(CenterPos + new Vector3Int(x, y)) && (!IsMove || !TileManager_TileMap.TileManager.GetTileIsBlock(CenterPos + new Vector3Int(x, y))))
                {
                    TileManager_TileMap.TileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                    TileManager_TileMap.TileManager.SelectTile(CenterPos + new Vector3Int(x, y));
                    SelectList.Add(CenterPos + new Vector3Int(x, y));
                }
                if (x != 0)
                    if (TileManager_TileMap.TileManager.CheckHasTile(CenterPos - new Vector3Int(x, y)) && (!IsMove || !TileManager_TileMap.TileManager.GetTileIsBlock(CenterPos - new Vector3Int(x, y))))
                    {
                        TileManager_TileMap.TileManager.SetSelectTileStyle(IsMove ? this.tile_MoveSelect : this.tile_AtkSelect);
                        TileManager_TileMap.TileManager.SelectTile(CenterPos - new Vector3Int(x, y));
                        SelectList.Add(CenterPos - new Vector3Int(x, y));
                    }
            }
            CurRange--;
        }


    }

    public void CancelSelectRange()
    {
        if (TileManager_TileMap.TileManager == null) return;
        foreach (var item in SelectList)
        {
            TileManager_TileMap.TileManager.CancelSelectTile(item);
        }
        SelectList = new List<Vector3Int>();
    }

    public bool CanSelect(Vector3Int i_targetVector)
    {
        return SelectList.Contains(i_targetVector);
    }
}
