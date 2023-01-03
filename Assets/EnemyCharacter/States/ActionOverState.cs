using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemySystem
{
    public class ActionOverState : EnemyState
    {
        public ActionOverState(EnemyCharacter enemyScript) : base(enemyScript)
        {

        }
        public override void StartFunction()
        {
            if (GameEventManager.gameEvent != null)
            {
                GameEventManager.gameEvent.EnemyActionOver.Invoke();
            }
            enemyScript.IsMove = false;
            enemyScript.IsAction = false;
            enemyScript.SetState(new WaitState(enemyScript));
        }
    }
}


