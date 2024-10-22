using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerInputManager : MonoBehaviour
{
    public static Vector2 movement;

    private PlayerInput playerInput;
    private PlayerController playerController;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerController.Move(context);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        playerController.Jump(context);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (playerController.GetPlayerCharacter() != GlobalPlayerManager.instance.localPlayer.character)
        {
            return;
        }
        PauseMenu.instance.Pause(context);
    }
}
