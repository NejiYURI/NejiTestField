using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MainCharacterScript : CharacterStateMachine, IF_GameCharacter
{
    private MouseInput mouseInput;

    public Vector3Int PlayerTileVector;

    Vector3Int IF_GameCharacter.TileVector
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
        SetState(new SelectState(this));
        mouseInput.MainActionMap.MouseClick.performed += _ => state.MouseClick(mouseInput.MainActionMap.MousePosition.ReadValue<Vector2>());
        mouseInput.MainActionMap.MouseRightClick.performed += _ => state.MouseRClick();
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.PlayerTurn.AddListener(TurnStart);
            GameEventManager.gameEvent.ActionSelect.AddListener(ButtonAction);
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
            case "EndRound":
                MainGameManager.mainGameManager.TurnEnd();
                SetState(new WaitTurnState(this));
                break;
        }
    }

    public void SetTileVector(Vector3Int _tile)
    {
        this.PlayerTileVector = _tile;
    }

    public void GetDamage(float i_dmgVal)
    {

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
}

