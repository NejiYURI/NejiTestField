using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateMachine : MonoBehaviour
{
    protected CharacterState state;

    public void SetState(CharacterState _state)
    {
        this.state = _state;
        this.state.StartFunction();
    }
}
