using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkedData : NetworkBehaviour
{
    [SyncVar] public SyncList<Player> playerList = new SyncList<Player>();

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
            playerList.Add(playerToAdd);
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
