using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (TileManager_TileMap.TileManager != null)
        {
            gameManager.SpawnCharacter(gameManager.Player_StartPos, gameManager.PlayerObject);
            foreach (var EnePos in gameManager.Enemy_StartPos)
            {
                gameManager.SpawnCharacter(EnePos, gameManager.EnemyObject);
            }
        }
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
}
