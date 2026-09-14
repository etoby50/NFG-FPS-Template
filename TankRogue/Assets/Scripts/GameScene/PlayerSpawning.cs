using UnityEngine;
using Unity.Netcode;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(0, 10, 0);

        NetworkObject playerNetworkedObject = player.GetComponent<NetworkObject>();
        playerNetworkedObject.Spawn();
    }
}