using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemySystem
{
    public class AttackState : EnemyState
    {
        private Vector2Int AtkTarget;
        public AttackState(EnemyCharacter enemyScript, Vector2Int i_Atktarget) : base(enemyScript)
        {
            this.AtkTarget = i_Atktarget;
        }

        public override void StartFunction()
        {
            bool GetSuccess = false;
            var TargetPos = enemyScript.GetTileManager().GetTileData(AtkTarget, out GetSuccess);
            TargetPos.CharacterOnTile.GetDamage(1);
            enemyScript.SetState(new ActionOverState(enemyScript));
        }
    }
}


