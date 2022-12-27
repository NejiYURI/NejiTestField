using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseStateMachine : MonoBehaviour
{
    protected StateData state;

    public void SetState(StateData _state)
    {
        this.state = _state;
        this.state.TurnStartFunction();
    }
}
