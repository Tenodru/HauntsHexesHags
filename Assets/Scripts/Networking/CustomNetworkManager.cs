using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    [Header ("Game-specific Variables")]
    public NetworkState networkState = NetworkState.None;
    public int playerCount = 1;
    public int maxPlayers = 2;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connecting!");
        playerCount += 1;

        if (playerCount == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }
    }

    public void Disconnect()
    {
        if (networkState == NetworkState.Host)
        {
            Debug.Log("Disconnecting host.");
            StopHost();
        } else if (networkState == NetworkState.Client)
        {
            Debug.Log("Disconnecting client.");
            StopClient();
            
        }
        playerCount = 1;
    }
}

public enum NetworkState
{
    None,
    Host,
    Client
}
