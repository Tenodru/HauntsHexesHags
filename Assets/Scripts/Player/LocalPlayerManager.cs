using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles local player data.
/// </summary>
public class LocalPlayerManager : MonoBehaviour
{
    public static LocalPlayerManager instance;

    public Player localPlayer;
    public Player guestPlayer;


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
}
