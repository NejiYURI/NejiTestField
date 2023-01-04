using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CustomTileSystem;
using CharacterSystem;
public class MainCharacterScript : CharacterStateMachine, IF_GameCharacter
{
    private MouseInput mouseInput;

    public Vector2Int PlayerTileVector;

    public float Health = 5;
    private float MaxHealth;

    Vector2Int IF_GameCharacter.TileVector
    {
        get
        {
            return PlayerTileVector;
        }
        set
        {
            PlayerTileVector = value;
        }
    }

    float IF_GameCharacter.Health
    {
        get { return Health; }
        set { Health = value; }
    }

    public int MoveRange = 3;

    public int AtkRange = 2;

    private bool IsMove;
    private bool IsAction;

    private void Awake()
    {
        mouseInput = new MouseInput();
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }

    private void OnDisable()
    {
        mouseInput.Disable();
    }

    private void Start()
    {
        IsMove = false;
        IsAction = false;
        MaxHealth = Health;
        SetState(new SelectState(this));
        mouseInput.MainActionMap.MouseClick.performed += _ => state.MouseClick(Camera.main.ScreenToWorldPoint(mouseInput.MainActionMap.MousePosition.ReadValue<Vector2>()));
        mouseInput.MainActionMap.MouseRightClick.performed += _ => state.MouseRClick();
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.PlayerTurn.AddListener(TurnStart);
            GameEventManager.gameEvent.ActionSelect.AddListener(ButtonAction);
            GameEventManager.gameEvent.SetUIImageFillAmount.Invoke("PlayerHealthBar", Health, MaxHealth);
            GameEventManager.gameEvent.PlayerTurnOver.AddListener(TurnOver);
        }
    }


    void Update()
    {
        //state.UpdateFunction();
    }
    void ButtonAction(string i_input)
    {
        switch (i_input)
        {
            case "Cancel":
                state.MouseRClick();
                break;
            case "EndTurn":
                MainGameManager.mainGameManager.TurnEnd();
                SetState(new WaitTurnState(this));
                break;
        }
    }

    public void SetTileVector(Vector3Int _tile)
    {
        this.PlayerTileVector = (Vector2Int)_tile;
    }

    public void SetTileVector(Vector2Int _tile)
    {
        this.PlayerTileVector = _tile;
    }

    public void GetDamage(float i_dmgVal)
    {
        Health -= i_dmgVal;
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.SetUIImageFillAmount.Invoke("PlayerHealthBar", Health, MaxHealth);
        }
        if (Health<=0 && MainGameManager.mainGameManager != null)
        {
            MainGameManager.mainGameManager.RestartLevel();
        }
    }

    void TurnOver()
    {
        SetState(new WaitTurnState(this));
    }
    void TurnStart()
    {
        IsMove = IsAction = false;
        SetState(new SelectState(this));
    }

    public bool IsMoved()
    {
        return this.IsMove;
    }

    public void PlayerMoved()
    {
        this.IsMove = true;
    }

    public void PlayerActioned()
    {
        this.IsAction = true;
    }

    public bool IsActioned()
    {
        return this.IsAction;
    }

    public bool CheckIsActionOver()
    {
        return IsMove && IsAction;
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

