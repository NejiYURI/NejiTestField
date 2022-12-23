using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public TileManager_TileMap tileManager;

    public GameObject PlayerObject;

    public GameObject PlayerCharacter;

    private TileGridData PlayerTile;

    private Vector3Int prevPos = new Vector3Int();
    private void Start()
    {
        if (tileManager != null)
        {
            TileGridData gridData = new TileGridData();
            if (tileManager.GetTileData(3, out gridData))
            {
                PlayerCharacter=Instantiate(PlayerObject, gridData.WorldLocation + gridData.TileOffset, Quaternion.identity);
                PlayerTile = gridData;
            }
        }
    }

    private void Update()
    {
        if (tileManager != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int gridPos = tileManager.GetCellPos(mousePosition);
            if (tileManager.CheckHasTile(gridPos) && prevPos != gridPos)
            {
                tileManager.ResetTile(prevPos);
                tileManager.ActiveTile(gridPos);
                prevPos = gridPos;
            }
            else if (!tileManager.CheckHasTile(gridPos) && tileManager.CheckHasTile(prevPos))
            {
                tileManager.ResetTile(prevPos);
                prevPos = new Vector3Int();
            }

            if (Input.GetMouseButtonDown(0) && tileManager.CheckHasTile(gridPos))
            {
                bool GetSuccess = false;
                TileGridData TargetPos = tileManager.GetTileData(gridPos, out GetSuccess);
                if (GetSuccess)
                {
                    List<TileGridData> pathList = new List<TileGridData>();
                    pathList = tileManager.FindPath(PlayerTile, TargetPos);
                    StartCoroutine(CharacterMove(pathList));
                   
                }
            }
        }
    }

    IEnumerator CharacterMove(List<TileGridData> pathList)
    {
        while (pathList.Count > 0)
        {
           
            PlayerCharacter.LeanMove(pathList[0].WorldLocation + pathList[0].TileOffset,0.1f);
            yield return new WaitForSeconds(0.1f);
            PlayerTile = pathList[0];
            pathList.RemoveAt(0);
        }
       
    }
}
