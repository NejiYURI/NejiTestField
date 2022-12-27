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
        }
    }

    public override void TurnEndFunction()
    {
        gameManager.SetState(new EnemyTurnState(gameManager));
    }
}
