using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class CustomNetworkManager : NetworkManager
{
    [Header ("Game-specific Variables")]
    public NetworkState networkState = NetworkState.None;
    public int maxPlayers = 2;

    private int lastPlayerID = 0;
    private bool localPlayerSet = false;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connecting!");
        Debug.Log("numPlayers: " + numPlayers);

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }

        lastPlayerID++;
        if (!localPlayerSet)
        {
            Debug.Log("Adding local player to playerList");
            GlobalPlayerManager.instance.localPlayer = new Player(lastPlayerID);
            localPlayerSet = true;
            GlobalPlayerManager.instance.playerList.Add(new Player(lastPlayerID));
        } else
        {
            Debug.Log("Adding outside player to playerList");
            GlobalPlayerManager.instance.playerList.Add(new Player(lastPlayerID, NetworkClient.connection.connectionId));
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Client connecting to server!");
        Debug.Log("numPlayers: " + numPlayers);

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client disconnecting from this server!");

        GlobalPlayerManager.instance.RemovePlayer(GlobalPlayerManager.instance.localPlayer);
    }

    public void Disconnect()
    {
        GlobalPlayerManager.instance.RemovePlayer(GlobalPlayerManager.instance.localPlayer);
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
