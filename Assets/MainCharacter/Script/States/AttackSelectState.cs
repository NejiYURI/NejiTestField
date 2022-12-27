using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackSelectState : CharacterState
{
    public AttackSelectState(MainCharacterScript characterScript) : base(characterScript)
    {

    }

    public override void StartFunction()
    {
        if (TileInteractScript.tileInteract != null)
        {
            TileInteractScript.tileInteract.SelectedRange(characterScript.PlayerTileVector, characterScript.AtkRange, false);
        }
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("EndRoundBtn", false);
            GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", true);
        }
    }

    public override void MouseClick(Vector2 i_MPos)
    {
        if (TileManager_TileMap.TileManager != null && TileInteractScript.tileInteract != null)
        {

            Vector3Int gridPos = TileManager_TileMap.TileManager.GetCellPos(Camera.main.ScreenToWorldPoint(i_MPos));

            if (Input.GetMouseButtonDown(0) && TileManager_TileMap.TileManager.CheckHasCharacter(gridPos) && TileInteractScript.tileInteract.CanSelect(gridPos))
            {
                Debug.Log("Attack Click!");
                bool GetSuccess = false;
                TileGridData TargetPos = TileManager_TileMap.TileManager.GetTileData(gridPos, out GetSuccess);
                if (GetSuccess)
                {
                    if (TileInteractScript.tileInteract != null) TileInteractScript.tileInteract.CancelSelectRange();
                    characterScript.PlayerActioned();
                    characterScript.SetState(new AttackState(characterScript, TargetPos.CharacterOnTile));
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

