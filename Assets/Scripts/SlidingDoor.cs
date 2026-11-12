using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour, IInteractable
{
    [Header("Lock")]
    public int keysRequired = 1;        // set to 4 for the exit door
    public bool consumeKeys = false;    // leave off so keys still count toward the exit
    public string lockedMessage = "It's locked. I need a key.";

    [Header("Motion")]
    public float slideHeight = 3f;      // how far up the door slides
    public float slideSpeed = 2f;       // units per second
    public AudioClip openSound;         // optional
    [Range(0f, 1f)] public float volume = 1f;

    private bool isOpen;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * slideHeight;
    }

    public string GetPrompt() => isOpen ? "" : "Click to open door";

    public void Interact(PlayerInteractor player)
    {
        if (isOpen) return;

        if (!player.Inventory.HasKeys(keysRequired))
        {
            string msg = keysRequired > 1
                ? $"I need {keysRequired} keys. ({player.Inventory.KeyCount}/{keysRequired})"
                : lockedMessage;
            player.ShowMessage(msg);
            return;
        }

        if (consumeKeys) player.Inventory.UseKeys(keysRequired);

        isOpen = true;

        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, transform.position, volume);

        StartCoroutine(Slide());
    }

    IEnumerator Slide()
    {
        while (Vector3.Distance(transform.position, openPosition) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, openPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = openPosition;
    }
}