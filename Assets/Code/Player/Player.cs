using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    public PlayerInputControl playerInput;
    public Rigidbody2D rb;
    public Vector2 InputDir;
    private PlayerStats playerStats;


    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
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
        rb.velocity = new Vector2(InputDir.x * playerStats.playerSpeed, InputDir.y * playerStats.playerSpeed);

    }

}
