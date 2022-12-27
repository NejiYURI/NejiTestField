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
            TileInteractScript.tileInteract.SelectedRange(characterScript.PlayerTileVector, characterScript.MoveRange, true);
        }

        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("EndRoundBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", true);
        }
    }

    public override void UpdateFunction()
    {

    }

    public override void MouseClick(Vector2 i_MPos)
    {
        if (TileManager_TileMap.TileManager != null && TileInteractScript.tileInteract != null)
        {
            Vector3Int gridPos = TileManager_TileMap.TileManager.GetCellPos(Camera.main.ScreenToWorldPoint(i_MPos));

            if (Input.GetMouseButtonDown(0) && TileManager_TileMap.TileManager.CheckHasTile(gridPos) && TileInteractScript.tileInteract.CanSelect(gridPos))
            {
                bool GetSuccess = false;
                TileGridData TargetPos = TileManager_TileMap.TileManager.GetTileData(gridPos, out GetSuccess);
                if (GetSuccess)
                {
                    List<TileGridData> pathList = new List<TileGridData>();
                    pathList = TileManager_TileMap.TileManager.FindPath(characterScript.PlayerTileVector, TargetPos.TileLocation);
                    TileManager_TileMap.TileManager.CharacterLeaveTile(characterScript.PlayerTileVector);
                    if (TileInteractScript.tileInteract != null) TileInteractScript.tileInteract.CancelSelectRange();
                    characterScript.PlayerMoved();
                    characterScript.SetState(new MovingState(characterScript, pathList));
                }
            }
        }
    }

    public override void MouseRClick()
    {
        if (TileInteractScript.tileInteract != null) TileInteractScript.tileInteract.CancelSelectRange();
        characterScript.SetState(new SelectState(characterScript));
    }
}

