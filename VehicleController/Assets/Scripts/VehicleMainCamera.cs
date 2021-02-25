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
        _CarTransform = GetComponentInParent<Transform>();
        _CarRigibody = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Quaternion cameraRot;

        transform.position = Vector3.Lerp(transform.position, _CarTransform.position, cameraStickiness * Time.fixedDeltaTime);

        if (_CarRigibody.velocity.magnitude < rotationThreshold)
        {
            cameraRot = Quaternion.LookRotation(_CarTransform.forward);
        }
        else
        {
            cameraRot = Quaternion.LookRotation(_CarRigibody.velocity.normalized);
        }

        cameraRot = Quaternion.Slerp(transform.rotation, cameraRot, cameraRotationSpeed * Time.fixedDeltaTime);
        transform.rotation = cameraRot;
    }
}
