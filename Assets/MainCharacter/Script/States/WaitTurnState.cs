using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterSystem
{
    public class WaitTurnState : CharacterState
    {
        public WaitTurnState(MainCharacterScript characterScript) : base(characterScript)
        {

        }

        public override void StartFunction()
        {
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.SetUIVisibility.Invoke("MoveBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("AtkBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("EndRoundBtn", false);
                GameEventManager.gameEvent.SetUIVisibility.Invoke("CancelBtn", false);
            }
        }
    }

}
