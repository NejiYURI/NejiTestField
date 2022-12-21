using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isometric_PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 1f;

    private Rigidbody2D _rg;

    private void Awake()
    {
        this._rg = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector2 cur_Pos = this._rg.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput,VerticalInput);

        Vector2 movement = inputVector * MoveSpeed;
        Vector2 newPos = cur_Pos + movement * Time.fixedDeltaTime;
        _rg.MovePosition(newPos);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
