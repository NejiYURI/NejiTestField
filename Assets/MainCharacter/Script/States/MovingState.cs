using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : CharacterState
{
    private List<TileGridData> PathList;
    public MovingState(MainCharacterScript characterScript, List<TileGridData> i_PathList) : base(characterScript)
    {
        this.PathList= i_PathList;
    }

    public override void StartFunction()
    {
        characterScript.StartCoroutine(CharacterMove(this.PathList));
    }

    IEnumerator CharacterMove(List<TileGridData> pathList)
    {
        while (pathList.Count > 0)
        {

            characterScript.transform.LeanMove(pathList[0].WorldLocation + pathList[0].TileOffset, 0.1f);
            yield return new WaitForSeconds(0.1f);
            characterScript.PlayerTile = pathList[0];
            pathList.RemoveAt(0);
        }
        characterScript.SetState(new SelectState(characterScript));
    }
}
