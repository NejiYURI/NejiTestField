using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainGameManager : TurnBaseStateMachine
{

    public static MainGameManager mainGameManager;
    public GameObject PlayerObject;

    public Vector2Int Player_StartPos;

    public GameObject EnemyObject;

    public List<Vector2Int> Enemy_StartPos;

    public Button MoveButton;

    private void Awake()
    {
        mainGameManager = this;
    }
    private void Start()
    {
        SetState(new StartState(this));
    }

    public void ActionSelect(string ActionName)
    {
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.ActionSelect.Invoke(ActionName);
        }
    }

    public void TurnEnd()
    {
        state.TurnEndFunction();
    }

    public GameObject SpawnObj(GameObject i_obj, Vector3 i_pos)
    {
        return Instantiate(i_obj, i_pos, Quaternion.identity);
    }
}

