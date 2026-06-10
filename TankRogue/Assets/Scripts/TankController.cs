using UnityEngine;

public class TankController : MonoBehaviour
{
    private TankInput input;

    [Header("Tank Info")] 
    [SerializeField] private Rigidbody tankBody;
    [SerializeField] private LayerMask tankDriveLayers;

    [Header("Suspension Stats")]
    [SerializeField] private Transform[] suspensionPoints;

    [SerializeField] private Transform[] suspensionWheels;
    [SerializeField] private Transform[] trackArmatures;
    
    [SerializeField] private AnimationCurve suspensionCurve;
    [SerializeField] private Vector2 suspensionMinMaxDist;
    [SerializeField] private Vector2 suspensionMinMaxForce;
    //[SerializeField] private float suspensionStrength;
    
    
    void Awake()
    {
        input = new TankInput();
        input.Tank.Enable();
    }

    void FixedUpdate()
    {
        RunSuspension();
    }

    private void RunSuspension()
    {
        for (int i = 0; i < suspensionPoints.Length; i++)
        {
            Ray ray = new Ray(suspensionPoints[i].position, suspensionPoints[i].up * -1);

            Physics.Raycast(ray, out RaycastHit hit, tankDriveLayers);

            float dist = Vector3.Distance(hit.point, suspensionPoints[i].position);

            float modDist = Mathf.Clamp01(Mathf.InverseLerp(suspensionMinMaxDist.x, suspensionMinMaxDist.y, dist));

            modDist = Mathf.Abs(modDist - 1);

            float force = Mathf.Lerp(suspensionMinMaxForce.x, suspensionMinMaxForce.y, modDist);
            
            Debug.Log(modDist);
            
            tankBody.AddForceAtPosition(suspensionPoints[i].up * force, suspensionPoints[i].position);

            float visualDist = Mathf.Clamp(dist, suspensionMinMaxDist.x, suspensionMinMaxDist.y);
            
            //compensate for the pivot on the wheels being offset from the bottom of them
            visualDist -= 0.3f;
            suspensionWheels[i].position = suspensionPoints[i].position +  suspensionPoints[i].up * (-1 * visualDist);
            trackArmatures[i].position = suspensionPoints[i].position +  suspensionPoints[i].up * (-1 * visualDist);
        }
    }

    private void NewSuspension()
    {
        
    }
}
