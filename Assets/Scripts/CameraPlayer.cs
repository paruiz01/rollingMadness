using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    public float sensitivity = 100f;  // Camera sensitivity for stick input
    public Transform target;       // Target object to orbit around
    public float distance = 5f;    // Distance from the target

    [SerializeField]
    private float xRotation = 0f;   // Current X rotation of the camera
    [SerializeField]
    private float yRotation = 0f;   // Current Y rotation of the camera

    private Vector2 rightStickInput; // Store the right stick input

    private void Update()
    {
        if (target != null)
        {

            // Get the gamepad right stick input values
            rightStickInput = InputSystem.GetDevice<Gamepad>().rightStick.ReadValue();

            // Calculate the camera's rotation based on the current X and Y rotation
            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);

            // Calculate the desired camera position based on the rotation and distance from the target
            Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;

            // Update the camera position and rotation
            transform.position = desiredPosition;
            transform.rotation = rotation;
        }
    }

    private void LateUpdate()
    {

        // Adjust camera rotation based on right stick input

        xRotation += rightStickInput.y * sensitivity;
        yRotation += rightStickInput.x * sensitivity;

        // Clamp the X rotation to avoid flipping the camera
        xRotation = Mathf.Clamp(xRotation, -10f, 10f);
    }
}