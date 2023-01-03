using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerTurnState : StateData
{
    public PlayerTurnState(MainGameManager gameManager) : base(gameManager)
    {

    }

    public override void TurnStartFunction()
    {
        if (GameEventManager.gameEvent != null)
        {
            if (gameManager.PlayerUI != null)
            {
                gameManager.PlayerUI.SetActive(true);
            }
            GameEventManager.gameEvent.PlayerTurn.Invoke();
           
        }
    }

    public override void TurnEndFunction()
    {
        if (gameManager.PlayerUI != null)
        {
            gameManager.PlayerUI.SetActive(false);
        }
        gameManager.SetState(new EnemyTurnState(gameManager));
    }
}
