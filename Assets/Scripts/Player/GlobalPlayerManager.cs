using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerManager : NetworkBehaviour
{
    public static GlobalPlayerManager instance;

    [SyncVar] public List<Player> playerList;

    public int numPlayers = 0;
    public int lastPlayerID = 0;

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

    public void RemovePlayer(Player playerToRemove)
    {
        try
        {
            playerList.Remove(playerToRemove);
        } catch
        {
            Debug.Log("Player not found; no player removed from playerList.");
        }
    }
}


[System.Serializable]
public class Player
{
    public PlayerCharacter character = PlayerCharacter.None;
    public int playerID;
    public int connectionID;

    public Player (PlayerCharacter newChar, int newID)
    {
        this.character = newChar;
        this.playerID = newID;
    }

    public Player(int newID, int newConnID = 0)
    {
        this.character = PlayerCharacter.None;
        this.playerID = newID;
        this.connectionID = newConnID;
    }

    public Player()
    {
        this.character = PlayerCharacter.None;
        this.playerID = 0;
        this.connectionID = 0;
    }

    public void ChangePlayerCharacter(PlayerCharacter newChar)
    {
        character = newChar;
    }
}


[System.Serializable]
public enum PlayerCharacter
{
    None,
    Human,
    Ghost
}
