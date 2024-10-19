using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Obsolete("Class currently unused.")]
public class PlayerInputManager : MonoBehaviour
{
    public static Vector2 movement;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        //Debug.Log("Jump Button pressed?" + jumpAction);
    }
}
