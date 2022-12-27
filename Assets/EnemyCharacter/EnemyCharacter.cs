using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour, IF_GameCharacter
{
    public float Health = 5;

    public Vector3Int TileVector;

    public void GetDamage(float i_dmgVal)
    {
        Debug.Log(this.name + " get " + i_dmgVal + " damage!!");
        Health = Mathf.Clamp(Health - i_dmgVal, 0, Health - i_dmgVal);
        if (Health <= 0)
        {
            if (TileManager_TileMap.TileManager != null) TileManager_TileMap.TileManager.CharacterLeaveTile(TileVector);
            Destroy(this.gameObject);
        }
    }
}
