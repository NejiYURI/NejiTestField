using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectState : CharacterState
{
    public SelectState(MainCharacterScript characterScript) : base(characterScript)
    {
    }

    public override void ButtonAction(string _action)
    {
        switch (_action)
        {
            case "Move":
                characterScript.SetState(new MoveSelectState(characterScript));
                break;
        }
       
    }
}
