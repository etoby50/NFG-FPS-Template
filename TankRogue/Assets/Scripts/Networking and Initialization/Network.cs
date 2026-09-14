using Unity.Netcode;
using UnityEngine;
using NaughtyAttributes;

public class Network : MonoBehaviour
{
    private static Network instance;

    public static Network Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private NetworkManager networkManager;

    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    [Button]
    [Rpc(SendTo.ClientsAndHost)]
    public void NetworkTest()
    {
        Debug.Log("testRPC!");
    }
}
