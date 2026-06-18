using UnityEngine;

public class TankController : MonoBehaviour
{
    private TankInput input;

    [SerializeField] private SuspensionSpring[] leftTreadWheels;
    [SerializeField] private SuspensionSpring[] rightTreadWheels;

    void Awake()
    {
        input = new TankInput();
        input.Tank.Enable();
    }

    void Update()
    {
        RunTankMovement();

        for(int i = 0; i < leftTreadWheels.Length; i++)
        {
            leftTreadWheels[i].accelerationInput = leftTrackSpeed;
        }

        for(int i = 0; i < rightTreadWheels.Length; i++)
        {
            rightTreadWheels[i].accelerationInput = rightTrackSpeed;
        }
    }

    private float leftTrackSpeed;
    private float rightTrackSpeed;

    private void RunTankMovement()
    {
        Vector2 move = input.Tank.WASD.ReadValue<Vector2>();

        //stopped
        if(move == Vector2.zero)
        {
            leftTrackSpeed = 0;
            rightTrackSpeed = 0;
            return;
        }

        //straight forwards/backwards
        if(move.x == 0 && move.y != 0)
        {
            leftTrackSpeed = move.y;
            rightTrackSpeed = move.y;
            return;
        }

        //hard turns
        if(move.x != 0 && move.y == 0)
        {
            rightTrackSpeed = -move.x;
            leftTrackSpeed = move.x;
        }

    }
}
