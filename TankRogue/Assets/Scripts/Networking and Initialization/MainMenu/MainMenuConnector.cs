using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Unity.Netcode;
using System;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject lobbyMenu;

    

    private void Awake()
    {
        EnterStartMenu();

        startMenu.SetActive(true);
        lobbyMenu.SetActive(false);

        ConnectionManager.Instance.OnConnectionStarted += StartConnection;
        ConnectionManager.Instance.OnConnectionIssue += ConnectionIssue;
        ConnectionManager.Instance.OnConnectionFinished += ConnectionFinished;
    }
    
    public async Task LoadGameScene()
    {
        await SceneManager.UnloadSceneAsync(Bootstrap.Instance.mainMenuSceneName);
        Debug.Log("Unloaded Menu Scene");
        await SceneManager.LoadSceneAsync(Bootstrap.Instance.gameSceneName, LoadSceneMode.Additive);
        Debug.Log("Loaded Game Scene");
    }

    #region Hosting and Joining

    [Header("Start Menu")]
    [SerializeField] private GameObject[] startMenuPanels;

    public void EnterStartMenu()
    {
        startMenuPanels[0].SetActive(true);

        for(int i = 1; i < startMenuPanels.Length; i++)
        {
            startMenuPanels[i].SetActive(false);
        }
    }

    [Header("Host Panel")]

    [SerializeField] private TMP_InputField hostSessionNameInput;
    [SerializeField] private TMP_InputField hostSessionPlayerNameInput;

    public void HostSession()
    {
        ConnectionManager.Instance.CreateSessionAsync(hostSessionNameInput.text, hostSessionPlayerNameInput.text);
    }

    [Header("Join Panel")]
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private TMP_InputField joinPlayerNameInput;

    public void JoinSession()
    {
        ConnectionManager.Instance.JoinSessionAsync(joinCodeInput.text, joinPlayerNameInput.text);
    }

    [Header("Connection")]
    [SerializeField] private TMP_Text connectionStatus;
    [SerializeField] private GameObject backButton;

    private void StartConnection(string message)
    {
        connectionStatus.text = message;
        connectionStatus.color = Color.white;
        backButton.SetActive(false);
    }

    private void ConnectionIssue(string message)
    {
        connectionStatus.text = message;
        connectionStatus.color = Color.red;
        backButton.SetActive(true);
    }

    private void ConnectionFinished()
    {
        EnterLobbyMenu();
    }

    #endregion Hosting and Joining

    #region Lobby

    [Header("Lobby Menu")]
    [SerializeField] private GameObject[] lobbyMenuPanels;

    [SerializeField] private TMP_Text joinCodeDisplay;
    [SerializeField] public GameObject startGameButton;

    private void EnterLobbyMenu()
    {
        lobbyMenuPanels[0].SetActive(true);

        for(int i = 1; i < lobbyMenuPanels.Length; i++)
        {
            lobbyMenuPanels[i].SetActive(false);
        }

        if (ConnectionManager.Instance.isSessionOwner)
        {
            joinCodeDisplay.text = ConnectionManager.Instance.session.Code;
            startGameButton.SetActive(true);
        }
        else
        {
            joinCodeDisplay.text = "";
            startGameButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        GameSceneManager.Instance.StartGameRpc();
    }


    #endregion Lobby
}
