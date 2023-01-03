using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TileMapSystem;
using CustomTileSystem;
public class MainGameManager : TurnBaseStateMachine
{

    public static MainGameManager mainGameManager;
    public GameObject PlayerObject;

    public Vector2Int Player_StartPos;

    private Vector2Int Player_Position;

    public GameObject EnemyObject;

    public List<Vector2Int> Enemy_StartPos;
    public List<IF_EnemyFunc> EnemyList;

    public GameObject PlayerUI;

    private void Awake()
    {
        mainGameManager = this;
    }
    private void Start()
    {
        Player_Position = Player_StartPos;
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

    public GameObject SpawnCharacter(Vector2Int i_pos, GameObject i_Character)
    {
        bool IsSucces = false;
        #region-TileMap Ver
        //TileGridData gridData = TileManager.tileManager.GetTileData(i_pos, out IsSucces);
        //if (IsSucces)
        //{

        //    GameObject EnemyObj = SpawnObj(i_Character, gridData.GetPosOnTile());
        //    if (EnemyObj.GetComponent<IF_GameCharacter>() != null)
        //    {
        //        TileManager.tileManager.CharacterInTile(gridData.TileLocation, EnemyObj.GetComponent<IF_GameCharacter>());
        //        EnemyObj.GetComponent<IF_GameCharacter>().TileVector = gridData.TileLocation;
        //    }
        //}
        #endregion
        #region-Custom Tile Ver
        TileGridData gridData = TileManager.tileManager.GetTileData(i_pos, out IsSucces);
        if (IsSucces)
        {
            //Debug.Log("Spawn " + i_Character.name);
            GameObject SpawnCharacter = SpawnObj(i_Character, gridData.WorldLocation);
            if (SpawnCharacter.GetComponent<IF_GameCharacter>() != null)
            {
                TileManager.tileManager.CharacterInTile(gridData.GridPosition, SpawnCharacter.GetComponent<IF_GameCharacter>());
                SpawnCharacter.GetComponent<IF_GameCharacter>().TileVector = gridData.GridPosition;
            }
            return SpawnCharacter;
        }
        #endregion
        return null;
    }
    public int GetEnemyAmount()
    {
        return Enemy_StartPos.Count;
    }

    public Vector2Int GetPlayerPos()
    {
        return Player_Position;
    }

    public void SetPlayerPos(Vector2Int i_NewPos)
    {
        Player_Position = i_NewPos;
    }

    public void RemoveEnemy(IF_EnemyFunc i_target)
    {
        if (EnemyList.Contains(i_target)) EnemyList.Remove(i_target);
    }
}

