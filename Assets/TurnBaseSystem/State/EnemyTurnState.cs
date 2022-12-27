using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyTurnState : StateData
{
    public EnemyTurnState(MainGameManager gameManager) : base(gameManager)
    {

    }

    public override void TurnStartFunction()
    {
        gameManager.StartCoroutine(WaitTimer());
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(3f);
        TurnEndFunction();
    }

    public override void TurnEndFunction()
    {
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
}

