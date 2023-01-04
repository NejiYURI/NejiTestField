using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemySystem
{
    public class MoveState : EnemyState
    {
        private Vector2Int MoveTarget;
        public MoveState(EnemyCharacter enemyScript, Vector2Int i_TargetPos) : base(enemyScript)
        {
            this.MoveTarget = i_TargetPos;

        }
        public override void StartFunction()
        {
            if (enemyScript.GetIneractScript() != null && enemyScript.GetTileManager() != null)
            {
                //Debug.Log(enemyScript.TileVector + " to " + MoveTarget);
                List<Vector2Int> pathList = new List<Vector2Int>();
                pathList = enemyScript.GetTileManager().FindPath(enemyScript.TileVector, MoveTarget, true);
                enemyScript.GetTileManager().CharacterLeaveTile(enemyScript.TileVector);
                enemyScript.StartCoroutine(CharacterMove(pathList));
            }
        }

        IEnumerator CharacterMove(List<Vector2Int> pathList)
        {
            if (enemyScript.GetMainGameManager() != null && enemyScript.GetTileManager() != null)
            {
                int MoveStep = enemyScript.MoveRange;
                Vector2Int PlayerPos = enemyScript.GetMainGameManager().GetPlayerPos();
                int dis = 0;
                while (pathList.Count > 0 && MoveStep > 0)
                {
                    bool IsSuccess = false;
                    dis = enemyScript.GetTileManager().GetDistance(PlayerPos, enemyScript.TileVector);
                    if (enemyScript.GetTileManager().GetTileIsBlock(pathList[0]) || dis <= enemyScript.AtkRange)
                    {
                        break;
                    }
                    enemyScript.transform.LeanMove(enemyScript.GetTileManager().GetTileWorldPosition(pathList[0], out IsSuccess), 0.1f);
                    yield return new WaitForSeconds(0.1f);

                    enemyScript.TileVector = pathList[0];
                    pathList.RemoveAt(0);
                    MoveStep--;
                }
                enemyScript.GetTileManager().CharacterInTile(enemyScript.TileVector, enemyScript);
            }
            enemyScript.IsMove = true;
            yield return new WaitForSeconds(0.5f);
            enemyScript.SetState(new TurnStart(enemyScript));
        }
    }
}


