using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    public GameObject PlayerObject;

    public Button MoveButton;

    private void Start()
    {
        if (TileManager_TileMap.TileManager != null)
        {
            TileGridData gridData = new TileGridData();
            if (TileManager_TileMap.TileManager.GetTileData(3, out gridData))
            {
                GameObject playerObj = Instantiate(PlayerObject, gridData.WorldLocation + gridData.TileOffset, Quaternion.identity);
                if (playerObj.GetComponent<MainCharacterScript>() != null)
                {
                    playerObj.GetComponent<MainCharacterScript>().SetTile(gridData);
                }
            }
        }
    }

    public void ActionSelect(string ActionName)
    {
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.ActionSelect.Invoke(ActionName);
        }
    }
}
