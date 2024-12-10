using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.Events;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [Header ("Game-specific Variables")]
    public NetworkState networkState = NetworkState.None;
    public int maxPlayers = 2;

    private bool localPlayerSet = false;

    // Event Delegates
    /// <summary>
    /// Called when this player disconnects from the server.
    /// </summary>
    public delegate void OnDisconnect();
    public static OnDisconnect onDisconnect;


    // Singletons
    private GlobalPlayerManager globalPlayerManager;
    private GlobalPlayerManager GlobalPlayerManager
    {
        get
        {
            if (globalPlayerManager != null) { return globalPlayerManager; }

            return globalPlayerManager = GlobalPlayerManager.instance;
        }
    }

    /*
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        if (networkState == NetworkState.Host) { return;  }

        Debug.Log("Client connecting!");
        Debug.Log("numPlayers: " + numPlayers);

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }

        
        if (!localPlayerSet)
        {
            Debug.Log("Adding local player to playerList");

            GlobalPlayerManager.lastPlayerID++;
            LocalPlayerManager.instance.localPlayer = new Player(GlobalPlayerManager.lastPlayerID);
            localPlayerSet = true;

            GlobalPlayerManager.AddPlayer(new Player(GlobalPlayerManager.lastPlayerID));
        }
    }*/

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        // Called before OnClientConnect() on host
        base.OnServerConnect(conn);
        Debug.Log("Client connecting to server!");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client disconnecting from this server!");

        GlobalPlayerManager.lastPlayerID--;
        GlobalPlayerManager.RemovePlayer(conn.connectionId);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //base.OnServerAddPlayer(conn);

        Debug.Log("Adding Player Object!");
        Debug.Log("numPlayers: " + numPlayers);

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
            return;
        }

        Debug.Log("Adding player to playerList");
        GlobalPlayerManager.lastPlayerID++;
        ulong playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.GetLobbyID(), GlobalPlayerManager.playerList.Count);
        Debug.Log("SteamLobby: " + SteamLobby.instance.GetLobbyID());
        Debug.Log("playerList Count: " + GlobalPlayerManager.playerList.Count);
        Debug.Log("Steam ID: " + playerSteamID);
        GlobalPlayerManager.AddPlayer(new Player(GlobalPlayerManager.lastPlayerID, conn.connectionId, playerSteamID));
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected from server.");
        onDisconnect();
    }

    public void Disconnect()
    {
        GlobalPlayerManager.ClearPlayerList();
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

        GlobalPlayerManager.lastPlayerID = 0;
    }
}

public enum NetworkState
{
    None,
    Host,
    Client
}
