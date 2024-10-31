using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerInputManager : MonoBehaviour
{
    [Tooltip("The PlayerCharacter associated with this player.")]
    [SerializeField] PlayerCharacter character;

    [Tooltip("The pause menu object associated with this character.")]
    [SerializeField] PauseMenu pauseMenu;

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
        if (character != GlobalPlayerManager.instance.localPlayer.character)
        {
            return;
        }

        playerController.Move(context);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (character != GlobalPlayerManager.instance.localPlayer.character)
        {
            return;
        }

        playerController.Jump(context);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (character != GlobalPlayerManager.instance.localPlayer.character)
        {
            return;
        }

        pauseMenu.Pause(context);
    }

    public PlayerCharacter GetPlayerCharacter()
    {
        return character;
    }
}
