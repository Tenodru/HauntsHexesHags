using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor.Search;
using Edgegap;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    [Header("Basic Main Menu Elements")]
    [Tooltip("The main menu screen.")]
    [SerializeField] RectTransform mainMenuScreen;

    [Header("Lobby Search Screen Elements")]
    [SerializeField] RectTransform lobbySearchScreen;

    [Tooltip("The host lobby button.")]
    [SerializeField] Button hostLobbyButton;

    [Tooltip("The join lobby button.")]
    [SerializeField] Button joinLobbyButton;

    [Tooltip("The back button on the Lobby Search screen.")]
    [SerializeField] Button lobbySearchBackButton;

    [Header("Join Lobby UI")]
    [Tooltip("The join lobby UI.")]
    [SerializeField] RectTransform joinLobbyScreen;

    [Tooltip("The join lobby code input field.")]
    [SerializeField] TMP_InputField codeInputField;

    [Header("Lobby Main Screen Elements")]
    [SerializeField] RectTransform lobbyMainScreen;

    [Tooltip("The lobby ID display on the Lobby Main screen.")]
    [SerializeField] TextMeshProUGUI lobbyIDDisplay;

    [Tooltip("The small title on the Lobby Main screen.")]
    [SerializeField] TextMeshProUGUI lobbyMainSubtitle;

    [Tooltip("The left Steam Profile icon on the Lobby Main screen.")]
    [SerializeField] Image steamIcon_1;

    [Tooltip("The right Steam Profile icon on the Lobby Main screen.")]
    [SerializeField] Image steamIcon_2;

    [Tooltip("The Invite From Steam button on the Lobby Main screen.")]
    [SerializeField] Button steamInviteButton;

    [Tooltip("The back button on the Lobby Main screen.")]
    [SerializeField] Button lobbyMainBackButton;

    [Tooltip("The Start Game button.")]
    [SerializeField] Button startGameButton;

    private bool isJoinLobbyScreenActive = false;


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
        TR_ShowMainMenu();
    }

    public void TR_ShowMainMenu()
    {
        // Goes back to the Main Menu screen
        lobbySearchScreen.gameObject.SetActive(false);
        lobbyMainScreen.gameObject.SetActive(false);
        mainMenuScreen.gameObject.SetActive(true);
    }

    public void TR_MainMenu_to_LobbySearch()
    {
        // Goes from Main Menu to the Lobby Search screen
        mainMenuScreen.gameObject.SetActive(false);
        lobbySearchScreen.gameObject.SetActive(true);
    }

    public void TR_LobbySearch_to_MainMenu()
    {
        // Goes from Lobby Search to Main Menu screen
        lobbySearchScreen.gameObject.SetActive(false);
        mainMenuScreen.gameObject.SetActive(true);
    }

    public void TR_LobbySearch_to_LobbyMain()
    {
        // Goes from Lobby Search to Lobby Main screen
        lobbySearchScreen.gameObject.SetActive(false);
        lobbyMainScreen.gameObject.SetActive(true);
        startGameButton.gameObject.SetActive(false);
    }

    public void TR_LobbyMain_to_LobbySearch()
    {
        // Goes from Lobby Main to Lobby Search screen
        lobbySearchScreen.gameObject.SetActive(true);
        lobbyMainScreen.gameObject.SetActive(false);
    }

    public void LobbyMainBackButton()
    {
        // TODO: Change this so that no matter what, the back button will close the lobby and go back to lobby search
        //       Add a warning popup that asks users if they want to confirm if they want to leave

        if (SteamLobby.instance.IsLobbyFull())
        {
            // If both players are in, go back to main menu
            TR_ShowMainMenu();
        } else
        {
            // If only one player is in, leave lobby and go back to lobby search
            TR_LobbyMain_to_LobbySearch();
            SteamLobby.instance.LeaveLobby();
        }
    }

    public void ChangeLobbyIDDisplay(ulong lobbyID)
    {
        lobbyIDDisplay.text = "LOBBY ID: " + lobbyID.ToString();
    }

    public void HideHostLobbyButton()
    {
        hostLobbyButton.gameObject.SetActive(false);
    }

    public void ShowHostLobbyButton()
    {
        hostLobbyButton.gameObject.SetActive(true);
    }

    public void ToggleJoinLobbyInput()
    {
        isJoinLobbyScreenActive = !isJoinLobbyScreenActive;
        joinLobbyScreen.gameObject.SetActive(isJoinLobbyScreenActive);
    }

    public void LobbyCodeEntered()
    {
        string codeInput = codeInputField.text;
        ulong convertedCode = (ulong)(Decimal.Parse(codeInput));

        Debug.Log("Converted code: " + convertedCode);

        SteamLobby.instance.JoinLobby(convertedCode);
    }

    public void EnterLobbyScreen()
    {
        // Should be called from SteamLobby when a lobby is joined
        TR_LobbySearch_to_LobbyMain();
        steamInviteButton.gameObject.SetActive(false);
        lobbyIDDisplay.text = "LOBBY ID: " + SteamLobby.instance.GetLobbyID();
    }
}
