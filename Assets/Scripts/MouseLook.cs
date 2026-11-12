using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 2.0f;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = 0f;
        float mouseY = 0f;

        // Tracks input via the new Input System with legacy fallback for trackpads
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            Vector2 delta = UnityEngine.InputSystem.Mouse.current.delta.ReadValue();
            mouseX = delta.x * 0.5f * mouseSensitivity;
            mouseY = delta.y * 0.5f * mouseSensitivity;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 10f;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 10;
        }

        // Tilt camera up and down (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Turn the player body left and right (Yaw)
        if (transform.parent != null)
        {
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}