using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TileMapSystem;
using CustomTileSystem;
using UnityEngine.SceneManagement;

public class MainGameManager : TurnBaseStateMachine
{

    public static MainGameManager mainGameManager;
    public GameObject PlayerObject;

    public List<LevelData> LevelSetup;
    private Vector2Int Player_Position;

    public List<IF_EnemyFunc> EnemyList;

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

    public GameObject SpawnCharacter(Vector2Int i_pos, GameObject i_Character, bool SpawnNew = true)
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
            GameObject SpawnCharacter = SpawnNew ? SpawnObj(i_Character, gridData.WorldLocation) : i_Character;
            if (!SpawnNew) i_Character.transform.position = gridData.WorldLocation;
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
        return EnemyList.Count;
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
        if (EnemyList.Count <= 0)
        {
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.PlayerTurnOver.Invoke();
            }
            if (TileManager.tileManager != null)
            {
                TileManager.tileManager.ClearAllCharacterOnTile();
            }
            SetState(new StartState(this));
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

