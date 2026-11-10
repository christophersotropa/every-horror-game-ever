using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [Header("Movement Detection")]
    public float minMoveSpeed = 0.5f;
    public float sprintSpeedThreshold = 6.5f; // between your walk and sprint speeds

    [Header("Audio")]
    public float walkPitch = 1f;
    public float sprintPitch = 1.3f; // faster playback = faster-sounding steps

    private CharacterController controller;
    private AudioSource audioSource;
    private bool isPlayingSteps;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Horizontal speed only, ignoring falling/jumping
        Vector3 flatVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        float speed = flatVelocity.magnitude;

        bool shouldPlay = controller.isGrounded && speed > minMoveSpeed;

        if (shouldPlay)
        {
            audioSource.pitch = speed > sprintSpeedThreshold ? sprintPitch : walkPitch;

            if (!isPlayingSteps)
            {
                // Resume where it left off, or start fresh the first time
                if (audioSource.time > 0f) audioSource.UnPause();
                else audioSource.Play();
                isPlayingSteps = true;
            }
        }
        else if (isPlayingSteps)
        {
            audioSource.Pause();
            isPlayingSteps = false;
        }
    }
}