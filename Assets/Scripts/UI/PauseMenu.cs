using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [Tooltip("The pause menu gameObject.")]
    [SerializeField] RectTransform pauseMenu;


    void Start()
    {
        Debug.Log("Disabling pause menu on start");
        gameObject.SetActive(false);
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
}
