using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    protected EnemyState state;

    public void SetState(EnemyState _state)
    {
        this.state = _state;
        this.state.StartFunction();
    }
}
