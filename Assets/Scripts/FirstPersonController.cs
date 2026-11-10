using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Look")]
    public float mouseSensitivity = 0.1f;
    public float maxLookAngle = 85f;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.2f;
    public float gravity = 20f; // enter as a positive number

    private CharacterController controller;
    private float pitch;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        HandleLook(mouse);
        HandleMovement(keyboard);

        // Escape frees the cursor while testing
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleLook(Mouse mouse)
    {
        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

        // Yaw: rotate the player body
        transform.Rotate(Vector3.up * delta.x);

        // Pitch: tilt only the camera
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement(Keyboard keyboard)
    {
        // Horizontal input
        float x = 0f;
        float z = 0f;
        if (keyboard.dKey.isPressed) x += 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        float speed = keyboard.leftShiftKey.isPressed ? sprintSpeed : walkSpeed;

        // Gravity and jumping
        float g = -Mathf.Abs(gravity); // always pulls down

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (keyboard.spaceKey.wasPressedThisFrame && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * g);

        verticalVelocity += g * Time.deltaTime;

        // One Move call for horizontal and vertical together
        Vector3 finalMove = move * speed + Vector3.up * verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);
    }
}