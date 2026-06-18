using UnityEngine;
using NaughtyAttributes;

public class SuspensionSpring : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Rigidbody vehicleBody;
    [SerializeField] private Transform vehicleTransform;
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

    [Header("Acceleration/Braking Stats")]
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float brakeStrength = 0.9f;
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float wheelTorque = 50;

    [Header("Wheel Visuals")] 
    [SerializeField] private Transform wheelVisuals;

    [SerializeField] private float visualsOffset;
    [SerializeField] private float visualsMaxDistanceFromOrigin;

    //External fields
    [HideInInspector] public float accelerationInput = 0;

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
            if(accelerationInput != 0)
            {
                float vehicleSpeed = Vector3.Dot(vehicleTransform.forward, vehicleBody.linearVelocity);
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(vehicleSpeed) / maxSpeed);

                float torque = powerCurve.Evaluate(normalizedSpeed) * accelerationInput * wheelTorque;
                if(torque < 1)
                {
                    Debug.Log(torque);
                }

                vehicleBody.AddForceAtPosition(tireTransform.forward * torque, tireTransform.position);
            }
            else
            {
                //brake
                float vehicleSpeed = Vector3.Dot(vehicleTransform.forward, vehicleBody.linearVelocity);
                float brakeGrip = -vehicleSpeed * brakeStrength;

                float brakeForce = brakeGrip/Time.fixedDeltaTime;

                vehicleBody.AddForceAtPosition(tireTransform.forward * (vehicleBody.mass/numberOfTires) * brakeForce, tireTransform.position);

            }
            
            //handle visuals
            //Debug.Log(-offset + visualsOffset);
            wheelVisuals.transform.position =
                tireTransform.position + (tireTransform.up * -1 * (-offset + visualsOffset));
        }
    }
}
