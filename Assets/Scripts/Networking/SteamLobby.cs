using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using Edgegap;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;

    [SerializeField] TextMeshProUGUI lobbyDisplay;

    // Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;
    protected Callback<SteamNetConnectionStatusChangedCallback_t> NetConnectionStatusChanged;

    // Variables
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager networkManager;
    private GlobalPlayerManager globalPlayerManager;

    //GameObjects
    public TextMeshProUGUI LobbyNameText;

    // Singletons
    private CustomNetworkManager NetworkManager
    {
        get
        {
            if (networkManager != null) { return networkManager; }

            return networkManager = (CustomNetworkManager)CustomNetworkManager.singleton;
        }
    }
    private GlobalPlayerManager GlobalPlayerManager
    {
        get
        {
            if (globalPlayerManager != null) { return globalPlayerManager; }

            return globalPlayerManager = GlobalPlayerManager.instance;
        }
    }


    private ulong currentLobbyID;
    public bool isLobbyFull = false;

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
        if(!SteamManager.Initialized) { return; }

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }

        Debug.Log("Lobby created successfully!");

        NetworkManager.StartHost();
        NetworkManager.networkState = NetworkState.Host;
        SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID());

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");

        Debug.Log("Lobby ID: " + new CSteamID(callback.m_ulSteamIDLobby));
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {

        Debug.Log("Request to join lobby detected!");

        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        // For everyone who enters lobby, including host
        Debug.Log("<color=orange>Entering Steam lobby.</color>");
        currentLobbyID = callback.m_ulSteamIDLobby;
        MainMenu.instance.ChangeLobbyIDDisplay(currentLobbyID);

        // For client ONLY
        if (NetworkServer.active) { return; }

        NetworkManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        NetworkManager.StartClient();
        NetworkManager.networkState = NetworkState.Client;
        NetworkManager.ClientReady();

        //MainMenu.instance.EnterLobbyScreen();

        //lobbyDisplay.text = "Lobby: " + callback.m_ulSteamIDLobby;
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, NetworkManager.maxConnections);
    }

    public void JoinLobby(ulong lobbyID)
    {
        //string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyID), )
        SteamMatchmaking.JoinLobby(new CSteamID(lobbyID));
    }

    public void LeaveLobby()
    {
        Debug.Log("Leaving current lobby: " + currentLobbyID);
        SteamMatchmaking.LeaveLobby(new CSteamID(currentLobbyID));
        isLobbyFull = false;

        NetworkManager.Disconnect();
    }

    public ulong GetLobbyID() { return currentLobbyID; }
}

