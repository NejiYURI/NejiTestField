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
        if (TileManager.tileManager != null)
        {
            gameManager.SpawnCharacter(gameManager.Player_StartPos, gameManager.PlayerObject);
            gameManager.EnemyList = new List<IF_EnemyFunc>();
            foreach (var EnePos in gameManager.Enemy_StartPos)
            {
               
                GameObject tmp_enemy = gameManager.SpawnCharacter(EnePos, gameManager.EnemyObject);
                if (TileManager.tileManager.HasTile(EnePos) && gameManager.EnemyObject.GetComponent<IF_EnemyFunc>() != null && tmp_enemy!=null) gameManager.EnemyList.Add(tmp_enemy.GetComponent<IF_EnemyFunc>());
            }
        }
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
}
