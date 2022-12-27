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
            GameEventManager.gameEvent.PlayerTurn.Invoke();
            if(gameManager.PlayerUI!=null)
            {
                gameManager.PlayerUI.SetActive(true);
            }
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
