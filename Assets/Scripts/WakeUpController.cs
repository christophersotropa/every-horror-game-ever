using System.Collections;
using UnityEngine;

public class WakeUpController : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float holdDuration = 3.0f;
    public float fadeDuration = 2.0f;
    public float trackpadSensitivity = 2.0f;


    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool canLook = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 1f;
        StartCoroutine(WakeUpSequence());
    }

    void Update()
    {
        if (!canLook) return;

        float mouseX = 0f;
        float mouseY = 0f;
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            Vector2 delta = UnityEngine.InputSystem.Mouse.current.delta.ReadValue();
            mouseX = delta.x * 0.05f * trackpadSensitivity;
            mouseY = delta.y * 0.05f * trackpadSensitivity;
        }

        rotationX -= mouseY;
        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    IEnumerator WakeUpSequence()
    {
        yield return new WaitForSeconds(holdDuration);
        canLook = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }
}
    
