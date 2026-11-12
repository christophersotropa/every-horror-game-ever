using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    public AudioClip pickupSound; // optional

    public string GetPrompt() => "Click to pick up key";

    public void Interact(PlayerInteractor player)
    {
        player.Inventory.AddKey();
        player.ShowMessage($"Picked up a key ({player.Inventory.KeyCount})", 1.5f);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}   