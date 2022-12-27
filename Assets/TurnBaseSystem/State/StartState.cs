using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartState : StateData
{
    public StartState(MainGameManager gameManager) : base(gameManager)
    {

    }

    public override void TurnStartFunction()
    {
        if (TileManager_TileMap.TileManager != null)
        {
            TileGridData gridData = new TileGridData();
            if (TileManager_TileMap.TileManager.GetTileData(gameManager.Player_StartPos, out gridData))
            {
                GameObject playerObj = gameManager.SpawnObj(gameManager.PlayerObject, gridData.GetPosOnTile());

                if (playerObj.GetComponent<MainCharacterScript>() != null)
                {
                    TileManager_TileMap.TileManager.CharacterInTile(gridData.TileLocation, playerObj.GetComponent<MainCharacterScript>());
                    playerObj.GetComponent<MainCharacterScript>().SetTileVector(gridData.TileLocation);
                }
            }
            foreach (var EnePos in gameManager.Enemy_StartPos)
            {
                if (TileManager_TileMap.TileManager.GetTileData(EnePos, out gridData))
                {
                    GameObject EnemyObj = gameManager.SpawnObj(gameManager.EnemyObject, gridData.GetPosOnTile());
                    if (EnemyObj.GetComponent<EnemyCharacter>() != null)
                    {
                        TileManager_TileMap.TileManager.CharacterInTile(gridData.TileLocation, EnemyObj.GetComponent<EnemyCharacter>());
                        EnemyObj.GetComponent<EnemyCharacter>().TileVector = gridData.TileLocation;
                    }

                }
            }

        }
        gameManager.SetState(new PlayerTurnState(gameManager));
    }
}
