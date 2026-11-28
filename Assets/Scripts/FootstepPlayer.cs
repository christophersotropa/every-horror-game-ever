using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class FootstepPlayer : MonoBehaviour
{
    public AudioClip[] footstepClips;
    [Range(0f, 1f)] public float volume = 0.5f;
    public float walkInterval = 0.5f;
    public float sprintInterval = 0.35f;

    AudioSource source;
    CharacterController controller;
    StarterAssetsInputs input;
    float timer;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        bool moving = controller.isGrounded && input.move.sqrMagnitude > 0.01f;
        if (!moving) { timer = 0f; return; }

        timer -= Time.deltaTime;
        if (timer <= 0f && footstepClips.Length > 0)
        {
            source.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)], volume);
            timer = input.sprint ? sprintInterval : walkInterval;
        }
    }
}