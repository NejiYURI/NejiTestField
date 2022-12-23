using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject basicTile;
    public TileData BaseTileData;

    public Transform SpawnTarget;

    public int Row = 5;
    public int Col = 5;
    public float TileSize = 1;

    [SerializeField]
    public Dictionary<Vector2?, GameObject> GridMap;

    private Isometric_Matrix GirdMatrix = new Isometric_Matrix();

    private void Awake()
    {
        GridMap = new Dictionary<Vector2?, GameObject>();
        GenerateBaseTile();
    }
    void Start()
    {

    }

    public Vector2? GetTileVector(int index)
    {
        int count = 0;
        foreach (var item in GridMap)
        {
            if (count != index)
            {
                count++; 
                continue;
            }
            return item.Key;

        }
        return null;
    }

    public bool HasTile(Vector2? i_pos)
    {
        return GridMap.ContainsKey(i_pos);
    }

    public void SetTile(Vector2? i_pos, TileData tileData)
    {
        if (!HasTile(i_pos)) return;
        GameObject tile = GridMap[i_pos];
        if (tile.GetComponent<TileObject>() != null)
        {
            tile.GetComponent<TileObject>().TileSet(tileData);
        }
    }

    public void ResetTile(Vector2? i_pos)
    {
        if (!HasTile(i_pos)) return;
        GameObject tile = GridMap[i_pos];
        if (tile.GetComponent<TileObject>() != null)
        {
            tile.GetComponent<TileObject>().TileSet(BaseTileData);
        }
    }


    public void GenerateBaseTile()
    {
        ClearBaseTile();
        for (int x = 0; x < Row; x++)
        {
            for (int y = 0; y < Col; y++)
            {

                Transform SpawnBase = this.transform;
                if (SpawnTarget != null)
                    SpawnBase = SpawnTarget;
                Vector2 pos = ToScreenVector(new Vector2(x, y), TileSize);
                pos += (Vector2)SpawnBase.position;
                GameObject newTile = Instantiate(basicTile, pos, Quaternion.identity);


                if (newTile.GetComponent<TileObject>() != null)
                {
                    newTile.GetComponent<TileObject>().TileData = BaseTileData;
                    newTile.GetComponent<TileObject>().TileSet();
                    newTile.name = "Tile[" + x + "," + y + "]";
                    newTile.transform.localScale = new Vector2(TileSize, TileSize);
                    if (SpawnTarget != null)
                        newTile.transform.SetParent(SpawnTarget);
                    else
                        newTile.transform.SetParent(this.transform);
                    if (!GridMap.ContainsKey(pos))
                    {
                        Debug.Log("Add Key " + pos);
                        GridMap.Add(pos, newTile);
                    }

                }
            }
        }
    }

    public void ClearBaseTile()
    {
        GridMap = new Dictionary<Vector2?, GameObject>();
        if (SpawnTarget != null)
        {
            while (SpawnTarget.childCount > 0)
            {
                GameObject child = SpawnTarget.GetChild(0).gameObject;
                child.transform.SetParent(null);
#if UNITY_EDITOR
                if (Application.isPlaying)
                {

                    Destroy(child);
                }
                else
                    DestroyImmediate(child);
#else
                    Destroy(transform.GetChild(0).gameObject);
#endif
            }

        }
        else
            while (transform.childCount > 0)
            {
                GameObject child = transform.GetChild(0).gameObject;
                child.transform.SetParent(null);
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Destroy(child);
                }
                else
                    DestroyImmediate(child);
#else
                    Destroy(transform.GetChild(0).gameObject);
#endif
            }
    }

    private Vector2 ToScreenVector(Vector2 i_vector, float i_tilesize = 1)
    {
        return (new Vector2(i_vector.x * GirdMatrix.i_hat.x + i_vector.y * GirdMatrix.j_hat.x, i_vector.x * GirdMatrix.i_hat.y + i_vector.y * GirdMatrix.j_hat.y)) * i_tilesize;
    }

    private Vector2 ToGridVector(Vector2 i_vector, float i_tilesize = 1)
    {
        float a = GirdMatrix.i_hat.x * i_tilesize;
        float b = GirdMatrix.j_hat.x * i_tilesize;
        float c = GirdMatrix.i_hat.y * i_tilesize;
        float d = GirdMatrix.j_hat.y * i_tilesize;
        Isometric_Matrix inv = invert_matrix(a, b, c, d);
        return (new Vector2(i_vector.x * inv.i_hat.x + i_vector.y * inv.i_hat.y, i_vector.x * inv.j_hat.x + i_vector.y * inv.j_hat.y)) * i_tilesize;
    }

    private Isometric_Matrix invert_matrix(float a, float b, float c, float d)
    {
        float det = (1 / a * d - b * c);
        return new Isometric_Matrix(new Vector2(det * d, det * -1f * b), new Vector2(det * -1f * c, det * a));
    }


}
