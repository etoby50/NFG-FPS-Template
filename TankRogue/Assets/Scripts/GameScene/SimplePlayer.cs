using NaughtyAttributes;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class SimplePlayer : NetworkBehaviour, ICharacter
{
    [Header("Components")]

    [SerializeField] private Rigidbody networkRigid;

    public override void OnNetworkSpawn()
    {
        if(this.OwnerClientId != ConnectionManager.Instance.networkManager.LocalClientId)
        {
            Debug.Log("Disabling player cuz they're not here!");
            this.enabled = false;
        }
        else
        {
            PlayerInputHandler.Instance.PlayerCharacter = this;

            //set up the local player
            Camera.main.gameObject.transform.parent = cameraParent;
            Camera.main.gameObject.transform.position = cameraParent.position;
            Camera.main.gameObject.transform.rotation = cameraParent.rotation;
        }
    }

    #region Movement

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;

    public void Jump()
    {
        networkRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void MoveWASD(Vector2 moveInput)
    {
        Vector3 forwards = yawTrans.forward * moveInput.y;
        Vector3 right = yawTrans.right * moveInput.x;

        Vector3 movement = (forwards + right).normalized * movementSpeed;

        networkRigid.linearVelocity = new Vector3(movement.x, networkRigid.linearVelocity.y, movement.z);
    }

    [Header("Look Settings")]
    [SerializeField] private Transform cameraParent;
    [SerializeField] private Transform yawTrans;
    [SerializeField] private Transform pitchTrans;

    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 pitchMinMax;

    public void LookAround(Vector2 lookInput)
    {
        Vector3 yawRot = yawTrans.rotation.eulerAngles;

        yawRot = new Vector3(yawRot.x, yawRot.y + (lookInput.x * lookSensitivity), yawRot.z);
        yawTrans.rotation = Quaternion.Euler(yawRot);

        Vector3 pitchRot = pitchTrans.rotation.eulerAngles;

        pitchRot = new Vector3(pitchRot.x + (lookInput.y * lookSensitivity * -1), pitchRot.y, pitchRot.z);

        //handle min
        if(pitchRot.x > pitchMinMax.y && pitchRot.x < 180)
        {
            pitchRot.x = 70;
        }

        if(pitchRot.x < 360 + pitchMinMax.x && pitchRot.x > 180)
        {
            pitchRot.x = 360 + pitchMinMax.x;
        }

        pitchTrans.rotation = Quaternion.Euler(pitchRot);

    }

    #endregion Movement
}
