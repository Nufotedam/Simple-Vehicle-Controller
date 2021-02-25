using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float motorForce = 1000.0f;        //  Max force applies to the motor torque
    [SerializeField] private float brakeForce = 3000.0f;        //  Brake force of the vehicle
    [SerializeField] private float maxSteeringAngle = 30.0f;    //  Angle of the steering
    [SerializeField] private Vector3 carCenterMass;             //  Center of the mass of the car, to avoid the car flip

    //  Wheel colliders
    [SerializeField] private WheelCollider[] frontWheelsColliders;
    [SerializeField] private WheelCollider[] backWheelsColliders;

    //  Wheels meshes position
    [SerializeField] private Transform[] frontWheelsTransform;
    [SerializeField] private Transform[] backWheelsTransform;

    private Vector3 movementDirection;          //  Input movement
    private float _CurrentBreakForce;           //  Current brake force apply
    private float _SteeringAngle;               //  Current angle
    private bool isBreaking;

    private Rigidbody _CarRigibody;

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
        //  Method to update the movmeent Input direction 
        movementDirection = newMovementDirection;
    }

    public void StartBraking()
    {
        isBreaking = true;
    }

    public void StopBraking()
    {
        isBreaking = false;
    }

    public void ResetPosition()
    {
        //  Reset the rotation of the car if it has been turned over
        Vector3 newRotation = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(newRotation);
        StopVehicle();
    }

    private void Start()
    {
        //  Get the Rigibody component and set its center of mass
        _CarRigibody = GetComponent<Rigidbody>();
        _CarRigibody.centerOfMass = carCenterMass;
    }

    private void FixedUpdate()
    {
        UpdateWheels();
        HandleMotor();
        HandleSteering();
    }

    private void HandleMotor() 
    {
        //  Add force to be able to move the vehicle
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            frontWheelsColliders[i].motorTorque = movementDirection.z * motorForce;
        }
        
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            backWheelsColliders[i].motorTorque = movementDirection.z * motorForce;
        }

        //  If the vehicle is braking, then apply the brake force
        _CurrentBreakForce = isBreaking ? brakeForce : 0f;
        Breaking();
    }

    private void Breaking()
    {
        //  Apply the brake force to all wheels
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            frontWheelsColliders[i].brakeTorque = _CurrentBreakForce;
        }

        for (int i = 0; i < backWheelsColliders.Length; i++)
        {
            frontWheelsColliders[i].brakeTorque = _CurrentBreakForce;
        }
    }

    private void HandleSteering()
    {
        //  Apply the steering to the front wheels
        _SteeringAngle = maxSteeringAngle * movementDirection.x;
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            frontWheelsColliders[i].steerAngle = _SteeringAngle;
        }
    }

    private void UpdateWheels()
    {
        //  Update every wheel mesh position and rotation
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            UpdateSingleWHeel(frontWheelsColliders[i], frontWheelsTransform[i]);
        }

        for (int i = 0; i < backWheelsColliders.Length; i++)
        {
            UpdateSingleWHeel(backWheelsColliders[i], backWheelsTransform[i]);
        }
    }

    private void UpdateSingleWHeel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        //  Get the wheel collider position and rotation and set the wheel mesh rotation and position
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    private void StopVehicle()
    {
        _CarRigibody.velocity = Vector3.zero;
        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            frontWheelsColliders[i].motorTorque = 0;
        }

        for (int i = 0; i < frontWheelsColliders.Length; i++)
        {
            backWheelsColliders[i].motorTorque = 0;
        }
    }
}
