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

            GlobalPlayerManager.instance.lastPlayerID++;
            LocalPlayerManager.instance.localPlayer = new Player(GlobalPlayerManager.instance.lastPlayerID);
            localPlayerSet = true;

            GlobalPlayerManager.instance.AddPlayer(new Player(GlobalPlayerManager.instance.lastPlayerID));
        }
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        // Called before OnClientConnect() on host
        base.OnServerConnect(conn);
        Debug.Log("Client connecting to server!");
        Debug.Log("numPlayers: " + numPlayers);

        if (numPlayers == maxPlayers)
        {
            SteamLobby.instance.isLobbyFull = true;
        }

        Debug.Log("Adding player to playerList");
        GlobalPlayerManager.instance.lastPlayerID++;
        ulong playerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.GetLobbyID(), GlobalPlayerManager.instance.playerList.Count);
        Debug.Log("SteamLobby: " + SteamLobby.instance.GetLobbyID());
        Debug.Log("playerList Count: " + GlobalPlayerManager.instance.playerList.Count);
        Debug.Log("Steam ID: " + playerSteamID);
        GlobalPlayerManager.instance.AddPlayer(new Player(GlobalPlayerManager.instance.lastPlayerID, conn.connectionId, playerSteamID));
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client disconnecting from this server!");

        GlobalPlayerManager.instance.lastPlayerID--;
        GlobalPlayerManager.instance.RemovePlayerWithConnectionID(conn.connectionId);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //base.OnServerAddPlayer(conn);

        Debug.Log("Adding Player Object!");
    }

    public void Disconnect()
    {
        GlobalPlayerManager.instance.ClearPlayerList();
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

        GlobalPlayerManager.instance.lastPlayerID = 0;
    }
}

public enum NetworkState
{
    None,
    Host,
    Client
}
