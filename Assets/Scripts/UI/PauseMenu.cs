using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("The pause menu gameObject.")]
    [SerializeField] RectTransform pauseMenu;

    [Header("Basic Pause Menu Elements")]
    [Tooltip("The host lobby button.")]
    [SerializeField] Button hostLobbyButton;

    [Tooltip("The join lobby button.")]
    [SerializeField] Button joinLobbyButton;

    [Header("Join Lobby UI")]
    [Tooltip("The join lobby UI.")]
    [SerializeField] RectTransform joinLobbyScreen;

    [Tooltip("The join lobby code input field.")]
    [SerializeField] TMP_InputField codeInputField;


    private bool isJoinLobbyScreenActive = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Debug.Log("Disabling pause menu on start");
        joinLobbyScreen.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pause key pressed.");
            if (pauseMenu.gameObject.activeSelf)
            {
                Debug.Log("Closing pause menu.");
                pauseMenu.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Opening pause menu.");
                pauseMenu.gameObject.SetActive(true);
            }
        }
    }

    public void HideHostLobbyButton()
    {
        hostLobbyButton.gameObject.SetActive(false);
    }

    public void ShowHostLobbyButton()
    {
        hostLobbyButton.gameObject.SetActive(true);
    }

    public void ToggleJoinLobbyScreen()
    {
        isJoinLobbyScreenActive = !isJoinLobbyScreenActive;
        joinLobbyScreen.gameObject.SetActive(isJoinLobbyScreenActive);
    }

    public void LobbyCodeEntered()
    {
        string codeInput = codeInputField.text;
        ulong convertedCode = (ulong)(Decimal.Parse(codeInput));

        Debug.Log("Converted code: " + convertedCode);

        SteamLobby.instance.JoinLobby(convertedCode);
    }
}
