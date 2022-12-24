using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectState : CharacterState
{
    public MoveSelectState(MainCharacterScript characterScript) : base(characterScript)
    {

    }

    public override void StartFunction()
    {
        if (TileInteractScript.tileInteract != null)
        {
            TileInteractScript.tileInteract.SelectedRange(characterScript.PlayerTile.GridLocation, characterScript.MoveRange);
        }
    }

    public override void UpdateFunction()
    {

    }

    public override void MouseClick(Vector2 i_MPos)
    {
        if (TileManager_TileMap.TileManager != null)
        {
            Vector3Int gridPos = TileManager_TileMap.TileManager.GetCellPos(Camera.main.ScreenToWorldPoint(i_MPos));

            if (Input.GetMouseButtonDown(0) && TileManager_TileMap.TileManager.CheckHasTile(gridPos) && IsInRange(gridPos) && gridPos != characterScript.PlayerTile.GridLocation)
            {
                bool GetSuccess = false;
                TileGridData TargetPos = TileManager_TileMap.TileManager.GetTileData(gridPos, out GetSuccess);
                if (GetSuccess)
                {
                    List<TileGridData> pathList = new List<TileGridData>();
                    pathList = TileManager_TileMap.TileManager.FindPath(characterScript.PlayerTile, TargetPos);
                    if (TileInteractScript.tileInteract != null) TileInteractScript.tileInteract.CancelSelectRange();
                    characterScript.SetState(new MovingState(characterScript, pathList));
                }
            }
        }
    }

    private bool IsInRange(Vector3Int i_pos)
    {
        Vector3Int rslt = new Vector3Int(Mathf.Abs(i_pos.x - characterScript.PlayerTile.GridLocation.x), Mathf.Abs(i_pos.y - characterScript.PlayerTile.GridLocation.y));
        return (rslt.x + rslt.y) < characterScript.MoveRange;
    }
}
