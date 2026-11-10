using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Settings")]
    public bool startOn = false;
    public Key toggleKey = Key.F;

    [Header("Optional Audio")]
    public AudioSource clickSound; // leave empty if you don't want a click

    private Light flashlight;

    void Start()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = startOn;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard[toggleKey].wasPressedThisFrame)
        {
            flashlight.enabled = !flashlight.enabled;

            if (clickSound != null)
                clickSound.Play();
        }
    }
}