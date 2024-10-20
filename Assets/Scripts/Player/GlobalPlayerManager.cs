using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerManager : MonoBehaviour
{
    public static GlobalPlayerManager instance;

    public Player localPlayer;
    public Player guestPlayer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }
}


[System.Serializable]
public class Player
{
    public PlayerCharacter character = PlayerCharacter.None;
    public string playerID;

    public void ChangePlayerCharacter(PlayerCharacter newChar)
    {
        character = newChar;
    }
}


[System.Serializable]
public enum PlayerCharacter
{
    Player1,
    Player2,
    None
}
