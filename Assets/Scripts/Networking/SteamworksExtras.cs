using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

public class SteamworksExtras : MonoBehaviour
{
    // Callbacks
    protected Callback<AvatarImageLoaded_t> AvatarLoaded;


    private void Start()
    {
        AvatarLoaded = Callback<AvatarImageLoaded_t>.Create(UpdateMainMenuAvatars);
    }

    public void UpdateMainMenuAvatars(AvatarImageLoaded_t callback)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") { return; }
    }
}
