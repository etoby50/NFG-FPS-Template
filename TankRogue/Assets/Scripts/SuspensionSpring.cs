using UnityEngine;
using NaughtyAttributes;

public class SuspensionSpring : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Rigidbody vehicleBody;
    [SerializeField] private Transform tireTransform;

    [Header("Tire Stats")] 
    [SerializeField] private float numberOfTires = 14;

    [Header("Suspension Stats")]
    [SerializeField] LayerMask tireCollisionLayers;
    [SerializeField] private float suspensionOffset;
    [SerializeField] private float suspensionStrength;
    [SerializeField] private float dampingStrength;

    [Header("Grip Stats")] 
    [SerializeField] private float gripFactor = 1;

    [Header("Wheel Visuals")] 
    [SerializeField] private Transform wheelVisuals;

    [SerializeField] private float visualsOffset;
    [SerializeField] private float visualsMaxDistanceFromOrigin;

    private void FixedUpdate()
    {
        //run physics
        Ray ray = new Ray(tireTransform.position, tireTransform.up * -1);

        if (Physics.Raycast(ray, out RaycastHit hit, 2, tireCollisionLayers))
        {
            //suspension
            Vector3 tireVelocity = vehicleBody.GetPointVelocity(tireTransform.position);

            float offset = suspensionOffset - hit.distance;

            float suspensionVelocity = Vector3.Dot(tireTransform.up, tireVelocity);

            float force = (offset * suspensionStrength) - (suspensionVelocity * dampingStrength);
            
            vehicleBody.AddForceAtPosition(tireTransform.up * force, tireTransform.position);
            
            //grip
            float slideVelocity = Vector3.Dot(tireTransform.right, tireVelocity);

            float grip = -slideVelocity * gripFactor;
            float counterForce = grip / Time.fixedDeltaTime;
            
            vehicleBody.AddForceAtPosition(tireTransform.right * (vehicleBody.mass/numberOfTires) * counterForce, tireTransform.position);
            
            //acceleration + braking
            
            
            //handle visuals
            Debug.Log(-offset + visualsOffset);
            wheelVisuals.transform.position =
                tireTransform.position + (tireTransform.up * -1 * (-offset + visualsOffset));
        }
    }
}
