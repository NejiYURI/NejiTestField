using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class CharacterState
{
    protected MainCharacterScript characterScript;

    public CharacterState(MainCharacterScript _characterScript)
    {
        this.characterScript = _characterScript;
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

    public virtual void MouseClick(Vector2 i_MPos)
    {

    }

    public virtual void MouseRClick()
    {

    }

    public virtual void ButtonAction(string _action)
    {
        Debug.Log("Get State:" + _action);
    }
}

