using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTileSystem;

public class StartState : StateData
{
    public StartState(MainGameManager gameManager) : base(gameManager)
    {

    }

    public override void TurnStartFunction()
    {
        if (gameManager.PlayerUI != null)
        {
            gameManager.PlayerUI.SetActive(false);
        }
        if (gameManager.LevelSetup.Count <= 0)
        {
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.SetUIVisibility.Invoke("ClearPanel",true);
            }
        }
        else
        {
            gameManager.StartCoroutine(SpawnCharacter());
        }
       
    }



    IEnumerator SpawnCharacter()
    {
       
        if (TileManager.tileManager != null && gameManager.LevelSetup.Count > 0)
        {
            LevelData cur_Level = gameManager.LevelSetup[0];
            //gameManager.SpawnCharacter(gameManager.LevelSetup.PlayerSpawnPos, gameManager.PlayerObject);
            if (gameManager.PlayerObject != null)
            {
                gameManager.SpawnCharacter(cur_Level.PlayerSpawnPos, gameManager.PlayerObject, false);
                if (TileManager.tileManager.HasTile(cur_Level.PlayerSpawnPos))
                {
                    gameManager.SetPlayerPos(cur_Level.PlayerSpawnPos);
                }
            }
            yield return new WaitForSeconds(0.2f);
            gameManager.EnemyList = new List<IF_EnemyFunc>();
            foreach (var EnemyData in cur_Level.enemyList)
            {

                GameObject tmp_enemy = gameManager.SpawnCharacter(EnemyData.Pos, EnemyData.obj);
                if (tmp_enemy != null && TileInteractScript.tileInteract!=null)
                {
                    TileInteractScript.tileInteract.StartWave(EnemyData.Pos);
                    yield return new WaitForSeconds(0.5f);
                }
               
                if (TileManager.tileManager.HasTile(EnemyData.Pos) && EnemyData.obj.GetComponent<IF_EnemyFunc>() != null && tmp_enemy != null) gameManager.EnemyList.Add(tmp_enemy.GetComponent<IF_EnemyFunc>());
                
            }
            gameManager.LevelSetup.RemoveAt(0);
        }
        yield return new WaitForSeconds(1.5f);
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
}
