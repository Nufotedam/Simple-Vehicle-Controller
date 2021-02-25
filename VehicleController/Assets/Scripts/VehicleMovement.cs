using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float motorForce = 1000.0f;
    [SerializeField] private float breakForce = 3000.0f;
    [SerializeField] private float maxSteeringAngle = 30.0f;
    [SerializeField] private Vector3 carCenterMass;

    [SerializeField] private WheelCollider frontLeft_WheelCollider;
    [SerializeField] private WheelCollider frontRight_WheelCollider;
    [SerializeField] private WheelCollider backLeft_WheelCollider;
    [SerializeField] private WheelCollider backRight_WheelCollider;

    [SerializeField] private Transform frontLeft_WheelTransform;
    [SerializeField] private Transform frontRight_WheelTransform;
    [SerializeField] private Transform backLeft_WheelTransform;
    [SerializeField] private Transform backRight_WheelTransform;

    private Vector3 movementDirection;
    private float _CurrentBreakForce;
    private float _SteeringAngle;
    private bool isBreaking;

    private Rigidbody _CarRigibody;

    public void UpdateMovementData(Vector3 newMovementDirection)
    {
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
        Vector3 newRotation = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(newRotation);
        StopVehicle();
    }

    private void Start()
    {
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
        frontLeft_WheelCollider.motorTorque = movementDirection.z * motorForce;
        frontRight_WheelCollider.motorTorque = movementDirection.z * motorForce;
        backLeft_WheelCollider.motorTorque = movementDirection.z * motorForce;
        backRight_WheelCollider.motorTorque = movementDirection.z * motorForce;

        _CurrentBreakForce = isBreaking ? breakForce : 0f;
        Breaking();
    }

    private void Breaking()
    {
        frontLeft_WheelCollider.brakeTorque = _CurrentBreakForce;
        frontRight_WheelCollider.brakeTorque = _CurrentBreakForce;
        backLeft_WheelCollider.brakeTorque = _CurrentBreakForce;
        backRight_WheelCollider.brakeTorque = _CurrentBreakForce;
    }

    private void HandleSteering()
    {
        _SteeringAngle = maxSteeringAngle * movementDirection.x;
        frontLeft_WheelCollider.steerAngle = _SteeringAngle;
        frontRight_WheelCollider.steerAngle = _SteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWHeel(frontLeft_WheelCollider, frontLeft_WheelTransform);
        UpdateSingleWHeel(frontRight_WheelCollider, frontRight_WheelTransform);
        UpdateSingleWHeel(backLeft_WheelCollider, backLeft_WheelTransform);
        UpdateSingleWHeel(backRight_WheelCollider, backRight_WheelTransform);
    }

    private void UpdateSingleWHeel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    private void StopVehicle()
    {
        _CarRigibody.velocity = Vector3.zero;
        frontLeft_WheelCollider.motorTorque = 0;
        frontRight_WheelCollider.motorTorque = 0;
        backLeft_WheelCollider.motorTorque = 0;
        backRight_WheelCollider.motorTorque = 0;
    }
}
