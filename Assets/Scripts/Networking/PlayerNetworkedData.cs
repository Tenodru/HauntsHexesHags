using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkedData : NetworkBehaviour
{
    public static PlayerNetworkedData instance;

    public List<Player> playerList;

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

    private void Start()
    {
        DontDestroyOnLoad(this);
        GlobalPlayerManager.onPlayerAdded += AddPlayer;
        GlobalPlayerManager.onPlayerRemoved += RemovePlayer;
        GlobalPlayerManager.onPlayerListClear += ClearPlayerList;
    }

    public void AddPlayer(Player playerToAdd)
    {
        try
        {
            Debug.Log("Adding player to networked data.");
            if (!playerList.Contains(playerToAdd))
            {
                playerList.Add(playerToAdd);
            }
        }
        catch
        {
            Debug.Log("Failed to add Player to PlayerList");
        }
    }

    [ClientRpc]
    public void ClientAddPlayer(Player playerToAdd)
    {
        try
        {
            Debug.Log("Adding player to client networked data.");
            if (!playerList.Contains(playerToAdd))
            {
                playerList.Add(playerToAdd);
            }
        }
        catch
        {
            Debug.Log("Failed to add Player to Client PlayerList");
        }
    }

    public void RemovePlayer(Player playerToRemove)
    {
        try
        {
            Debug.Log("Removing player from networked data.");
            Debug.Log("Player stats: " + playerToRemove);
            //playerList.Remove(playerToRemove);
            playerList.RemoveAll(player => player.connectionID == playerToRemove.connectionID);
        }
        catch
        {
            Debug.Log("Player not found; no player removed from playerList.");
        }
    }

    public void ClearPlayerList()
    {
        playerList.Clear();
    }
}
