using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamIconImage : MonoBehaviour
{
    public ulong PlayerSteamID;

    RawImage icon;
    int imageID;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start()
    {
        icon = GetComponent<RawImage>();

        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    public void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        Debug.Log("Setting Steam avatar icons on main menu.");
        if (callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            icon.texture = GetSteamImageAsTexture(callback.m_iImage);
            Debug.Log("Steam avatar set.");
        } else
        {
            Debug.Log("Steam avatar not set.");
            return;
        }
    }

    public void GetPlayerIcon()
    {
        Debug.Log("Getting player Steam avatar with SteamID: " + PlayerSteamID);
        imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        icon.texture = GetSteamImageAsTexture(imageID);
        Debug.Log("Getting player Steam avatar with imageID: " + imageID);
        if (imageID == -1)
        {
            Debug.Log("Failed to acquire Steam avatar.");
            return;
        }
        Debug.Log("Getting player Steam avatar with SteamID: " + PlayerSteamID + " successful!");
    }

    public void ClearPlayerIcon()
    {
        Debug.Log("Clearing player icon, probably because this player disconnected or left.");

        PlayerSteamID = 0;
        icon.texture = null;
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        //AvatarReceived = true;
        Debug.Log("Getting player Steam avatar texture.");
        icon.texture = texture;
        return texture;
    }
}
