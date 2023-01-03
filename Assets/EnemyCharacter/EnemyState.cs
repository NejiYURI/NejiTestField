using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class EnemyState
{
    protected EnemyCharacter enemyScript;

    public EnemyState(EnemyCharacter _enemyScript)
    {
        this.enemyScript = _enemyScript;
    }
    public virtual void StartFunction()
    {

    }

    public virtual void UpdateFunction()
    {

    }
    public virtual void EndFunction()
    {

    }
}

