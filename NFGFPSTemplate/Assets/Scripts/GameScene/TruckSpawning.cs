using Unity.Netcode;
using UnityEngine;

public class TruckSpawning : MonoBehaviour
{
    [SerializeField] private GameObject truckPrefab;

    private void Start()
    {
        if(ConnectionManager.Instance.networkManager.LocalClient.IsSessionOwner)
        {
            SpawnTruck();
        }
    }

    private void SpawnTruck()
    {
        GameObject truck = Instantiate(truckPrefab);
        truck.transform.position = new Vector3(0, 10, 0);

        NetworkObject truckNetworkedObject = truck.GetComponent<NetworkObject>();
        truckNetworkedObject.Spawn();
    }
}
