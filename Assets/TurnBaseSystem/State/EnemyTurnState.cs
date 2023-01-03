using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyTurnState : StateData
{
    public List<IF_EnemyFunc> tmp_EnemyList;
    public EnemyTurnState(MainGameManager gameManager) : base(gameManager)
    {
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.EnemyActionOver.AddListener(EnemyActionOver);
        }
        tmp_EnemyList=new List<IF_EnemyFunc>();
        tmp_EnemyList.AddRange(gameManager.EnemyList);
    }

    public override void TurnStartFunction()
    {
        gameManager.StartCoroutine(TriggerEnemy());
    }

    IEnumerator TriggerEnemy()
    {
        if (tmp_EnemyList.Count > 0)
        {
            yield return new WaitForSeconds(0.15f);
            tmp_EnemyList[0].StartAction();
        }
        else
        {
            gameManager.StartCoroutine(WaitTimer());
        }
    }

    public override void TurnEndFunction()
    {
        Debug.Log("Enemy turn end");
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.EnemyActionOver.RemoveListener(EnemyActionOver);
        }
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(0.5f);
        TurnEndFunction();
    }
    void EnemyActionOver()
    {
        tmp_EnemyList.RemoveAt(0);
        gameManager.StartCoroutine(TriggerEnemy());

    }
}

