using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterSystem
{
    public class MovingState : CharacterState
    {
        private List<Vector2Int> PathList;
        public MovingState(MainCharacterScript characterScript, List<Vector2Int> i_PathList) : base(characterScript)
        {
            this.PathList = i_PathList;
        }

        public override void StartFunction()
        {
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("EndTurnBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", false);
            }
            characterScript.StartCoroutine(CharacterMove(this.PathList));
        }

        IEnumerator CharacterMove(List<Vector2Int> pathList)
        {
            while (pathList.Count > 0)
            {
                bool IsSuccess = false;
                characterScript.transform.LeanMove(characterScript.GetTileManager().GetTileWorldPosition(pathList[0], out IsSuccess), 0.1f);
                yield return new WaitForSeconds(0.1f);
                characterScript.PlayerTileVector = pathList[0];
                pathList.RemoveAt(0);
            }
            if (characterScript.GetTileManager() != null)
            {
                characterScript.GetTileManager().CharacterInTile(characterScript.PlayerTileVector, characterScript);
            }
            if (characterScript.GetMainGameManager() != null)
            {
                characterScript.GetMainGameManager().SetPlayerPos(characterScript.PlayerTileVector);
            }
            characterScript.SetState(new SelectState(characterScript));
        }
    }
}
