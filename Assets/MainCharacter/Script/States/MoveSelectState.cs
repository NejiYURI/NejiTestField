using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterSystem
{
    public class MoveSelectState : CharacterState
    {
        public MoveSelectState(MainCharacterScript characterScript) : base(characterScript)
        {

        }

        public override void StartFunction()
        {
            if (characterScript.GetIneractScript() != null)
            {
                characterScript.GetIneractScript().SelectedRange(characterScript.PlayerTileVector, characterScript.MoveRange, true);
            }

            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("EndTurnBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", true);
            }
        }

        public override void UpdateFunction()
        {

        }

        public override void MouseClick(Vector2 i_MPos)
        {
            if (characterScript.GetTileManager() != null && characterScript.GetIneractScript() != null)
            {
                //Vector3Int gridPos = characterScript.GetTileManager().GetCellPos(Camera.main.ScreenToWorldPoint(i_MPos));
                Vector2Int gridPos = characterScript.GetTileManager().ToGridVector(i_MPos);
                if (characterScript.GetIneractScript().CanSelect(gridPos))
                {
                    Debug.Log("SelectMove!!");
                    bool GetSuccess = false;
                    var TargetPos = characterScript.GetTileManager().GetTileData(gridPos, out GetSuccess);
                    if (GetSuccess)
                    {
                        List<Vector2Int> pathList = new List<Vector2Int>();
                        pathList = characterScript.GetTileManager().FindPath(characterScript.PlayerTileVector, TargetPos.GridPosition);
                        characterScript.GetTileManager().CharacterLeaveTile(characterScript.PlayerTileVector);
                        if (characterScript.GetIneractScript() != null) characterScript.GetIneractScript().CancelSelectRange();
                        characterScript.PlayerMoved();
                        characterScript.SetState(new MovingState(characterScript, pathList));
                    }
                }
            }
        }

        public override void MouseRClick()
        {
            if (characterScript.GetIneractScript() != null) characterScript.GetIneractScript().CancelSelectRange();
            characterScript.SetState(new SelectState(characterScript));
        }
    }

}