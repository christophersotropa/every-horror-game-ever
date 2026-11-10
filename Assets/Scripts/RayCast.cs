using System;
using UnityEngine;
using UnityEngine.UI;

public class RayCast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLength = 5;
    private Camera _camera;

    private NoteController _noteController;

    [Header("Cursor")]
    [SerializeField] private Image cursor;

    [Header("Input Key")]
    [SerializeField] private KeyCode interactKey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null)
            {
                _noteController = readableItem;
                HighLightCrosshair(true);
                //highlightCrosshair
            }
            else
            {
                ClearNote();
            }
        }
        else
        {
            ClearNote();
        }

        if (_noteController != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                _noteController.ShowNote();
            }
        }
    }

    void ClearNote()
    {
        if (_noteController != null)
        {
            HighLightCrosshair(false);
            _noteController = null;
        }
    }

    void HighLightCrosshair(bool on)
    {
        if (on)
        {
            cursor.color = Color.red;
        }
        else
        {
            cursor.color = Color.white;
        }
    }
}
