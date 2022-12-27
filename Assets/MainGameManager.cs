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

    public GameObject PlayerUI;

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

    public void SpawnCharacter(Vector2Int i_pos, GameObject i_Character)
    {
        TileGridData gridData = new TileGridData();
        if (TileManager_TileMap.TileManager.GetTileData(i_pos, out gridData))
        {
            GameObject EnemyObj = SpawnObj(i_Character, gridData.GetPosOnTile());
            if (EnemyObj.GetComponent<IF_GameCharacter>() != null)
            {
                TileManager_TileMap.TileManager.CharacterInTile(gridData.TileLocation, EnemyObj.GetComponent<IF_GameCharacter>());
                EnemyObj.GetComponent<IF_GameCharacter>().TileVector = gridData.TileLocation;
            }
        }
    }
}

