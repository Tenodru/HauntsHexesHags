using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;

    [SerializeField] TextMeshProUGUI lobbyDisplay;

    // Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    // Variables
    public ulong LobbyId;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;

    //GameObjects
    public TextMeshProUGUI LobbyNameText;

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

        manager = GetComponent<CustomNetworkManager>();

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }

        Debug.Log("Lobby created successfully!");

        manager.StartHost();

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
        Debug.Log("POG");


        // For client ONLY
        if (NetworkServer.active) { return; }

        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        manager.StartClient();

        lobbyDisplay.text = "Lobby: " + callback.m_ulSteamIDLobby;
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
    }

    public void JoinLobby(ulong lobbyID)
    {
        //string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(lobbyID), )
        SteamMatchmaking.JoinLobby(new CSteamID(lobbyID));
    }
}

