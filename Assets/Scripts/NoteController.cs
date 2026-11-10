using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class NoteController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode closeKey;

    [Space(10)]
    [SerializeField] private FirstPersonController player;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

    [Space(10)]
    [SerializeField] private string noteText;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    private bool isOpen = false;
    public void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        disablePlayer(true);
        //Disable player
        isOpen = true;
    }

    void disableNote()
    {
        noteCanvas.SetActive(false);
        disablePlayer(false);
        isOpen = false;
    }

    void disablePlayer(bool disable)
    {
        player.enabled = !disable;
    }

    public void Update()
    {
        if (Input.GetKeyDown(closeKey))
        {
            disableNote();
        }
    }
}
