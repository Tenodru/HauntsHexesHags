using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("The pause menu gameObject.")]
    [SerializeField] RectTransform pauseMenu;

    [Tooltip("The host lobby button.")]
    [SerializeField] Button hostLobbyButton;

    [Tooltip("The join lobby button.")]
    [SerializeField] Button joinLobbyButton;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Debug.Log("Disabling pause menu on start");
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
}
