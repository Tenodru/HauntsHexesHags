using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [SerializeField] RectTransform pauseMenu;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
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
