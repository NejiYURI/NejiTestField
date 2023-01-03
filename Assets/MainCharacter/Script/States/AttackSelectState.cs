using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterSystem
{
    public class AttackSelectState : CharacterState
    {
        public AttackSelectState(MainCharacterScript characterScript) : base(characterScript)
        {

        }

        public override void StartFunction()
        {
            if (characterScript.GetIneractScript() != null)
            {
                characterScript.GetIneractScript().SelectedRange(characterScript.PlayerTileVector, characterScript.AtkRange, false);
            }
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("EndTurnBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", true);
            }
        }

        public override void MouseClick(Vector2 i_MPos)
        {
            if (characterScript.GetTileManager() != null && characterScript.GetIneractScript() != null)
            {
                //Vector3Int gridPos = characterScript.GetTileManager().GetCellPos(Camera.main.ScreenToWorldPoint(i_MPos));
                Vector2Int gridPos = characterScript.GetTileManager().ToGridVector(i_MPos);
                if (characterScript.GetTileManager().CheckHasCharacter(gridPos) && characterScript.GetIneractScript().CanSelect(gridPos))
                {
                    Debug.Log("Attack Click!");
                    bool GetSuccess = false;
                    var TargetPos = characterScript.GetTileManager().GetTileData(gridPos, out GetSuccess);
                    if (GetSuccess)
                    {
                        characterScript.GetIneractScript().CancelSelectRange();
                        characterScript.PlayerActioned();
                        characterScript.GetIneractScript().StartWave(gridPos);
                        characterScript.SetState(new AttackState(characterScript, TargetPos.CharacterOnTile));
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
