using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBlock : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Camera mainCamera;
    public LayerMask dropZoneLayer;
    public float dropCheckRadius = 1f;

    private Transform assignedSlot;
    private DropZone currentZone;

    void Start()
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f;
            transform.position = mainCamera.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, dropCheckRadius, dropZoneLayer);

        foreach (var hit in hits)
        {
            DropZone zone = hit.GetComponent<DropZone>() ?? hit.GetComponentInParent<DropZone>();

            if (zone != null)
            {
                assignedSlot = zone.GetAvailableSlot();
                if (assignedSlot != null)
                {
                    transform.position = assignedSlot.position;
                    transform.SetParent(assignedSlot);
                    currentZone = zone;
                    zone.UpdateAnswerDisplay();
                    return;
                }
            }
        }
        ReturnToOriginalPosition();
    }

    public void ResetBlock()
    {
        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        transform.SetParent(originalParent);

        if (currentZone != null)
        {
            currentZone.UnassignBlock(transform);
            currentZone = null;
        }
    }
}