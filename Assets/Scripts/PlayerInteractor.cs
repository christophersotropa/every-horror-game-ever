using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public SubtitleManager subtitles;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactMask = ~0;

    public PlayerInventory Inventory { get; private set; }

    void Awake()
    {
        Inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;

        IInteractable target = FindTarget();
        if (target != null)
            target.Interact(this);
    }

    IInteractable FindTarget()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask,
                            QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponentInParent<IInteractable>();
        }
        return null;
    }

    // Door and key scripts call this, and it shows through the subtitle system
    public void ShowMessage(string message, float duration = 2f)
    {
        if (subtitles != null)
            subtitles.ShowSubtitle(message, duration);
    }
}