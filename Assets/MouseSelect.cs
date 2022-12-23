using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseSelect : MonoBehaviour
{
    public Tilemap map;

    public TileBase tile_Normal;
    public TileBase tile_Selected;

    [SerializeField]
    private Vector3Int prevPos = new Vector3Int();

    public Dictionary<Vector3Int, TileBase> GridMap;

    private void Start()
    {
        int xmax = map.cellBounds.max.x;
        int ymax = map.cellBounds.max.y;
        int zmax = map.cellBounds.max.z;
        GridMap = new Dictionary<Vector3Int, TileBase>();
        for (int y = map.cellBounds.min.y; y < ymax; y++)
        {
            for (int x = map.cellBounds.min.x; x < xmax; x++)
            {

                Vector3Int getPos = new Vector3Int(x, y, 0);
                if (map.HasTile(getPos) && !GridMap.ContainsKey(getPos))
                {
                    GridMap.Add(getPos, map.GetTile(getPos));
                }

            }
        }
        //StartCoroutine(ShowTileTimer());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPos = map.WorldToCell(mousePosition);
        if (map.HasTile(gridPos) && prevPos != gridPos)
        {
            if (map.HasTile(prevPos))
                map.SetTile(prevPos, tile_Normal);
            map.SetTile(gridPos, tile_Selected);
            prevPos = gridPos;
        }
        else if (!map.HasTile(gridPos) && map.HasTile(prevPos))
        {
            map.SetTile(prevPos, tile_Normal);
            prevPos = new Vector3Int();
        }

    }

    IEnumerator ShowTileTimer()
    {
        foreach (var item in GridMap)
        {
            map.SetTile(item.Key, tile_Selected);
            yield return new WaitForSeconds(0.1f);
            map.SetTile(item.Key, tile_Normal);
        }
        //StartCoroutine(ShowTileTimer());
    }
}
