using UnityEngine;

public class VehicleMainCamera : MonoBehaviour
{
    public float rotationThreshold = 1f;
    public float cameraStickiness = 10.0f;
    public float cameraRotationSpeed = 5.0f;

    Transform _CarTransform;
    Rigidbody _CarRigibody;

    private void Awake()
    {
        //  Get the parent component, It is the car or vehicle
        _CarTransform = GetComponentInParent<Transform>();
        //  Get the Rigibody of the car or vehicle
        _CarRigibody = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Quaternion cameraRot;

        //  Interpolate the current position of the camera and move it with the car
        transform.position = Vector3.Lerp(transform.position, _CarTransform.position, cameraStickiness * Time.fixedDeltaTime);
                
        if (_CarRigibody.velocity.magnitude < rotationThreshold)
        {
            //  If the vehicle is stop, then look forward
            cameraRot = Quaternion.LookRotation(_CarTransform.forward);
        }
        else
        {
            //  If the vehicle is moving, then the camera rotates where the vehicle is moving
            cameraRot = Quaternion.LookRotation(_CarRigibody.velocity.normalized);
        }

        //  Interpolate the current rotation of the camera
        cameraRot = Quaternion.Slerp(transform.rotation, cameraRot, cameraRotationSpeed * Time.fixedDeltaTime);
        transform.rotation = cameraRot;
    }
}
