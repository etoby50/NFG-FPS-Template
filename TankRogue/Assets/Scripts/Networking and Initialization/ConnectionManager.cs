using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private static ConnectionManager instance;
    public static ConnectionManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public event Action<string> OnConnectionIssue;
    public event Action<string> OnConnectionStarted;

    public event Action OnConnectionFinished;

    [SerializeField] private int maxPlayers;

    public NetworkManager networkManager;

    //Connection Info
    public ISession session;

    public bool isSessionOwner = false;

    private void Awake()
    {
        Instance = this;
        networkManager.OnClientConnectedCallback += OnClientJoined;
        networkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        networkManager.OnClientConnectedCallback += OnClientConnected;

        ConnectToUnityServices();
    }

    public void JoinOrStart(string sessionName, string sessionID, string playerName)
    {
        CreateSessionAsync(sessionName, playerName);
    }

    private async Task ConnectToUnityServices()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("unity services initialized");
    }

    public async Task CreateSessionAsync(string sessionName, string playerName)
    {
        try
        {
            OnConnectionStarted.Invoke("Creating Session....");

            AuthenticationService.Instance.SwitchProfile(playerName);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var options = new SessionOptions()
            {
                Name = sessionName,
                MaxPlayers = maxPlayers
            }.WithDistributedAuthorityNetwork();

            session = await MultiplayerService.Instance.CreateSessionAsync(options);

            OnConnectionFinished.Invoke();
        }
        catch(Exception e)
        {
            OnConnectionIssue.Invoke(e.Message);
            Debug.LogError("Session could not be created!");
            Debug.LogError(e.Message);
        }
    }

    public async Task JoinSessionAsync(string joinCode, string playerName)
    {
        try
        {
            OnConnectionStarted.Invoke("Joining Session...");

            AuthenticationService.Instance.SwitchProfile(playerName);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);

            OnConnectionFinished.Invoke();
        }
        catch(Exception e)
        {
            OnConnectionIssue.Invoke(e.Message);
            Debug.LogError("Session could not be joined!");
            Debug.LogError(e.Message);
        }
    }

    private void OnClientConnected(ulong clientConnected)
    {
        if(clientConnected == networkManager.CurrentSessionOwner)
        {
            SpawnManagementPrefabs();
        }
        else
        {
            return;
        }
    }

    private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
    {
        if(networkManager.CurrentSessionOwner == networkManager.LocalClientId)
        {
            isSessionOwner = true;
            Debug.Log("You are now the session owner!");
        }
    }

    private void OnClientJoined(ulong clientID)
    {

    }

    #region Spawning Management Prefabs

    public GameObject gameSceneManagerPrefab;

    private void SpawnManagementPrefabs()
    {
        GameObject GSMObject = Instantiate(gameSceneManagerPrefab);
        
        NetworkObject GSMNetworkObject = GSMObject.GetComponent<NetworkObject>();
        GSMNetworkObject.Spawn();
    }

    #endregion Spawning Management Prefabs

    #region Session ID

    private const string IDGlyphs = "0123456789";

    private string GenerateID()
    {
        string newID = "";

        for(int i = 0; i < 8; i++)
        {
            int glyphIndex = UnityEngine.Random.Range(0, 9);
            newID += IDGlyphs[glyphIndex];
        }

        return newID;
    }

    #endregion Session ID
}
