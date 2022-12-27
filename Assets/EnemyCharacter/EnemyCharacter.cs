using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : MonoBehaviour, IF_GameCharacter
{
    public float Health = 5;
    private float MaxHealth;

    public Vector3Int TileVector;

    public Image HealthBar;
    Vector3Int IF_GameCharacter.TileVector
    {
        get
        {
            return TileVector;
        }
        set
        {
            TileVector = value;
        }
    }
    private void Start()
    {
        MaxHealth = Health;
        HealthChange();
    }
    public void GetDamage(float i_dmgVal)
    {
        Debug.Log(this.name + " get " + i_dmgVal + " damage!!");
        Health = Mathf.Clamp(Health - i_dmgVal, 0, Health - i_dmgVal);
        HealthChange();
        if (Health <= 0)
        {
            if (TileManager_TileMap.TileManager != null) TileManager_TileMap.TileManager.CharacterLeaveTile(TileVector);
            Destroy(this.gameObject);
        }
    }

    void HealthChange()
    {
        if (HealthBar != null)
            HealthBar.fillAmount = Health / MaxHealth;
    }
}
