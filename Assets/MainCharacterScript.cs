using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCharacterScript : CharacterStateMachine
{
    private MouseInput mouseInput;

    public TileGridData PlayerTile;

    public int MoveRange = 3;

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
        SetState(new SelectState(this));
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.ActionSelect.AddListener(state.ButtonAction);
        }
        mouseInput.MainActionMap.MouseClick.performed += _ => state.MouseClick(mouseInput.MainActionMap.MousePosition.ReadValue<Vector2>());
        state.StartFunction();
    }


    void Update()
    {
        //state.UpdateFunction();
    }

    public void SetTile(TileGridData _tile)
    {
        this.PlayerTile = _tile;
    }
}
