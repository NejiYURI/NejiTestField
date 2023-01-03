using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemySystem
{
    public class TurnStart : EnemyState
    {
        public TurnStart(EnemyCharacter enemyScript) : base(enemyScript)
        {

        }

        public override void StartFunction()
        {
            if (enemyScript.GetMainGameManager() != null && enemyScript.GetTileManager()!=null)
            {
                if (!enemyScript.IsAction)
                {
                    Vector2Int PlayerPos = enemyScript.GetMainGameManager().GetPlayerPos();
                    int dis = enemyScript.GetTileManager().GetDistance(PlayerPos, enemyScript.TileVector);
                    if (dis <= enemyScript.AtkRange)
                    {
                        enemyScript.IsAction = true;
                        enemyScript.SetState(new AttackState(enemyScript, PlayerPos));
                    }
                    else if (!enemyScript.IsMove)
                    {
                        enemyScript.SetState(new MoveState(enemyScript, PlayerPos));
                    }
                    else
                    {
                        enemyScript.SetState(new ActionOverState(enemyScript));
                    }
                }
               
            }
        }
    }
}


