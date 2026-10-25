using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInputControl playerInput;
    public Rigidbody2D rb;
    public Vector2 InputDir;
    public float moveSpeed = 5f;


    private void Awake()
    {
        playerInput = new PlayerInputControl();//创建一个新的PlayerInputControl实例
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        InputDir = playerInput.Player.Move.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        rb.velocity = new Vector2(InputDir.x * moveSpeed, InputDir.y * moveSpeed);

    }

}
