using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomTileSystem;
using EnemySystem;
public class EnemyCharacter : EnemyStateMachine, IF_GameCharacter, IF_EnemyFunc
{
    public float Health = 5;
    private float MaxHealth;

    public int MoveRange = 2;

    public int AtkRange = 1;

    public Vector2Int TileVector;

    public Image HealthBar;

    public bool IsMove;
    public bool IsAction;

    Vector2Int IF_GameCharacter.TileVector
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

    float IF_GameCharacter.Health
    {
        get { return Health; }
        set { Health = value; }
    }
    private void Start()
    {
        MaxHealth = Health;
        HealthChange();
        SetState(new WaitState(this));
    }
    public void GetDamage(float i_dmgVal)
    {
        Debug.Log(this.name + " get " + i_dmgVal + " damage!!");
        Health = Mathf.Clamp(Health - i_dmgVal, 0, Health - i_dmgVal);
        HealthChange();
        if (Health <= 0)
        {
            if (TileManager.tileManager != null) TileManager.tileManager.CharacterLeaveTile(TileVector);
            if (MainGameManager.mainGameManager != null) MainGameManager.mainGameManager.RemoveEnemy(this);
            Destroy(this.gameObject);
        }
    }

    public void StartAction()
    {
        SetState(new TurnStart(this));
    }

    void HealthChange()
    {
        if (HealthBar != null)
            HealthBar.fillAmount = Health / MaxHealth;
    }

    public TileInteractScript GetIneractScript()
    {
        if (TileInteractScript.tileInteract != null)
            return TileInteractScript.tileInteract;

        return null;
    }

    public TileManager GetTileManager()
    {
        if (TileManager.tileManager != null)
            return TileManager.tileManager;

        return null;
    }

    public MainGameManager GetMainGameManager()
    {
        if (MainGameManager.mainGameManager != null)
            return MainGameManager.mainGameManager;

        return null;
    }
}
