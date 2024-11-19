using Mirror;
using Mirror.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GlobalPlayerManager : NetworkBehaviour
{
    public static GlobalPlayerManager instance;

    // Event delegates
    public delegate void OnPlayerListUpdate();
    public static OnPlayerListUpdate onPlayerListUpdate;


    [SyncVar] public List<Player> playerList;
    public Queue<string> steamUserUpdateQueue = new Queue<string>();
    [SyncVar] public List<string> steamUserUpdateList = new List<string>();

    [SyncVar] public int lastPlayerID = 0;

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

    private void Update()
    {
        /*
        if (steamUserUpdateList.Count > 0) 
        {
            if (UpdatePlayerListSteamIDs(steamUserUpdateList[0])) { steamUserUpdateList.RemoveAt(0); }
        }
        */
    }

    public bool UpdatePlayerListSteamIDs(ulong newSteamID)
    {
        if (playerList.Count == 0) { return false; }
        Debug.Log("Updating with new steamID: " + newSteamID);

        if (playerList.Where(player => player.playerSteamID == newSteamID).Any())
        {
            Debug.Log("SteamID already assigned!");
            return false;
        }

        foreach (var player in playerList)
        {
            if (player.playerSteamID == 0)
            {
                player.playerSteamID = newSteamID;
                return true;
            }
        }

        return false;
    }

    public void AddPlayer(Player playerToAdd)
    {
        try
        {
            playerList.Add(playerToAdd);
            onPlayerListUpdate();
        }
        catch
        {
            Debug.Log("Failed to add Player to PlayerList");
        }
    }

    public void RemovePlayer(Player playerToRemove)
    {
        try
        {
            playerList.Remove(playerToRemove);
            onPlayerListUpdate();
        } catch
        {
            Debug.Log("Player not found; no player removed from playerList.");
        }
    }

    public void RemovePlayerWithConnectionID(int connID)
    {
        try
        {
            playerList.RemoveAll(player => player.connectionID == connID);
            onPlayerListUpdate();
        }
        catch
        {
            Debug.Log("Player with connection ID not found; no player removed from playerList.");
        }
    }

    public void ClearPlayerList()
    { 
        playerList.Clear(); 
    }


    public void AddSteamUserToQueue(string steamID)
    {
        if (steamUserUpdateList.Contains(steamID))
        {
            Debug.Log("Steam ID already in queue!");
            return;
        }

        Debug.Log("Adding Steam ID to queue.");
        steamUserUpdateList.Add(steamID);
    }
}


[System.Serializable]
public class Player
{
    public PlayerCharacter character = PlayerCharacter.None;
    public int playerID;
    public int connectionID;
    public ulong playerSteamID;

    public Player (PlayerCharacter newChar, int newID)
    {
        this.character = newChar;
        this.playerID = newID;
    }

    public Player(int newID, int newConnID = 0, ulong playerSteamID = 0)
    {
        this.character = PlayerCharacter.None;
        this.playerID = newID;
        this.connectionID = newConnID;
        this.playerSteamID = playerSteamID;
    }

    public Player()
    {
        this.character = PlayerCharacter.None;
        this.playerID = 0;
        this.connectionID = 0;
        this.playerSteamID = 0;
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
