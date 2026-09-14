using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class SimplePlayer : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform cameraParent;

    [Header("Components")]

    [SerializeField] private Rigidbody networkRigid;

    void Update()
    {
        if (Input.GetButton("Space"))
        {
            networkRigid.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        }
    }

    public override void OnNetworkSpawn()
    {
        if(this.OwnerClientId != ConnectionManager.Instance.networkManager.LocalClientId)
        {
            Debug.Log("Disabling player cuz they're not here!");
            this.enabled = false;
        }
        else
        {
            //set up the local player
            Camera.main.gameObject.transform.parent = cameraParent;
            Camera.main.gameObject.transform.position = cameraParent.position;
            Camera.main.gameObject.transform.rotation = cameraParent.rotation;

        }
    }
}
