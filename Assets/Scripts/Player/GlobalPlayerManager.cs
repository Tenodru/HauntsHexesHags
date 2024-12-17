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

    public delegate void OnPlayerAdded(Player player);
    public static OnPlayerAdded onPlayerAdded;
    public delegate void OnPlayerRemoved(Player player);
    public static OnPlayerRemoved onPlayerRemoved;
    public delegate void OnPlayerListClear();
    public static OnPlayerListClear onPlayerListClear;

    public delegate void OnPlayerAddedBySteamID(ulong steamID);
    public static OnPlayerAddedBySteamID onPlayerAddedBySteamID;
    public delegate void OnPlayerRemovedBySteamID(ulong steamID);
    public static OnPlayerRemovedBySteamID onPlayerRemovedBySteamID;


    public List<Player> playerList;
    public Queue<string> steamUserUpdateQueue = new Queue<string>();
    public List<string> steamUserUpdateList = new List<string>();

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

    private void Update()
    {
        /*
        if (steamUserUpdateList.Count > 0) 
        {
            if (UpdatePlayerListSteamIDs(steamUserUpdateList[0])) { steamUserUpdateList.RemoveAt(0); }
        }
        */
    }

    [Command(requiresAuthority = false)]
    public void AddPlayer(Player playerToAdd)
    {
        try
        {
            Debug.Log("Adding player!");
            playerList.Add(playerToAdd);
            UpdateClientPlayerLists();
            onPlayerListUpdate();
            onPlayerAdded(playerToAdd);
            onPlayerAddedBySteamID(playerToAdd.playerSteamID);
        }
        catch
        {
            Debug.Log("Failed to add Player to PlayerList");
        }
    }

    public void RemovePlayer(int connID)
    {
        try
        {
            Debug.Log("Removing player!");
            Player playerToRemove = playerList.Find(player => player.connectionID == connID);
            onPlayerRemoved(playerToRemove);
            Debug.Log("PLAYER: " + playerToRemove.connectionID);
            onPlayerRemovedBySteamID(playerToRemove.playerSteamID);
            playerList.RemoveAll(player => player.connectionID == connID);
            onPlayerListUpdate();
        } catch
        {
            Debug.Log("Failed to remove player from playerList.");
        }
        
    }

    public void ClearPlayerList()
    {
        playerList.Clear();
        try
        {
            onPlayerListClear();
        } catch
        {
            Debug.Log("Could not clear networked list.");
        }
        
    }


    public void UpdateClientPlayerLists()
    {
        Debug.Log("<color=orange>GPM - Updating client player list.</color>");
        foreach (Player player in playerList)
        {
            Debug.Log("<color=orange>GPM - Adding player to playerList from server.</color>");
            RpcClientAddPlayer(player.playerID, player.connectionID, player.playerSteamID);
        }
    }

    [ClientRpc]
    public void RpcClientAddPlayer(int playerID, int playerConnID, ulong playerSteamID)
    {
        Debug.Log("Adding player to client networked data.");
        Player playerToAdd = new Player(playerID, playerConnID, playerSteamID);
        if (!playerList.Contains(playerToAdd))
        {
            playerList.Add(playerToAdd);
        }
    }




    [Obsolete]
    public void RemovePlayerWithConnectionID(int connID)
    {
        try
        {
            playerList.RemoveAll(player => player.connectionID == connID);
            onPlayerListUpdate();
            //onPlayerRemoved();
        }
        catch
        {
            Debug.Log("Player with connection ID not found; no player removed from playerList.");
        }
    }

    [Obsolete]
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

    [Obsolete]
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

    public override string ToString()
    {
        return $"PlayerID:{playerID}, ConnID:{connectionID}, SteamID:{playerSteamID}";
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
