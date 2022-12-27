using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectState : CharacterState
{
    public SelectState(MainCharacterScript characterScript) : base(characterScript)
    {

    }

    public override void StartFunction()
    {
        if (characterScript.CheckIsActionOver() && MainGameManager.mainGameManager != null)
        {
            Debug.Log("Turn Over!");
            MainGameManager.mainGameManager.TurnEnd();
            characterScript.SetState(new WaitTurnState(characterScript));
        }
        else
        {
            if (GameEventManager.gameEvent != null) GameEventManager.gameEvent.ActionSelect.AddListener(ButtonAction);
        }
    }

    public override void ButtonAction(string _action)
    {
        switch (_action)
        {
            case "Move":
                if (characterScript.IsMoved()) break;
                EndFunction();
                characterScript.SetState(new MoveSelectState(characterScript));
                break;
            case "Attack":
                if (characterScript.IsActioned()) break;
                EndFunction();
                characterScript.SetState(new AttackSelectState(characterScript));
                break;
        }
    }

    public override void EndFunction()
    {
        if (GameEventManager.gameEvent != null) GameEventManager.gameEvent.ActionSelect.RemoveListener(ButtonAction);
    }
}

