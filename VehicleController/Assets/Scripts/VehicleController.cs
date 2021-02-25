using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    public VehicleMovement vehicleMovement;         //  Vehicle movement component
    public PlayerInput playerInput;                 //  Player Input Component

    private Vector3 _RawInput;

    //  INPUT SYSTEM ACTIONS EVENTS
    public void OnMovement(InputAction.CallbackContext value)
    {
        //  Read the Input value and save in a variable
        Vector2 inputMovement = value.ReadValue<Vector2>();
        _RawInput = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnBreaking(InputAction.CallbackContext value)
    {
        //  If the buttom was pressed start braking
        if (value.started)
        {
            vehicleMovement.StartBraking();
        }
        //  If the buttom was released stop braking
        if (value.canceled)
        {
            vehicleMovement.StopBraking();
        }
    }

    public void OnResetVehicle(InputAction.CallbackContext value)
    {
        //  Reset the rotation of the vehicle
        vehicleMovement.ResetPosition();
    }
    //

    private void Update()
    {
        //  Update the input vector
        UpdateVehicleMovement();
    }

    private void UpdateVehicleMovement()
    {
        //  Update the Input variable
        vehicleMovement.UpdateMovementData(_RawInput);
        //  You can add smooth movement o whatever
    }
}
