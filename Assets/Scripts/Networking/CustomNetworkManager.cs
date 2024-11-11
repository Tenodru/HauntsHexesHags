using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    [Header ("Game-specific Variables")]
    public NetworkState networkState = NetworkState.None;
    public int maxPlayers = 2;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connecting!");

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Client connecting to server!");

        if (numPlayers == maxPlayers)
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

        if (numPlayers < maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = false;
        }
    }
}

public enum NetworkState
{
    None,
    Host,
    Client
}
