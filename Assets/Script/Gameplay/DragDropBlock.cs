using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBlock : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Transform originalParent;
    private Camera mainCamera;

    public LayerMask dropZoneLayer;
    public float dropCheckRadius = 1f;

    private Transform assignedSlot;
    private DropZone currentZone;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
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

        // Optional: disable gravity while dragging
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
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
                    transform.rotation = assignedSlot.rotation;
                    transform.SetParent(assignedSlot);
                    transform.localScale = originalScale; // Fix squishing/stretching
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
        transform.rotation = originalRotation;
        transform.SetParent(originalParent);
        transform.localScale = originalScale; // Restore original scale

        // Restore physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Unassign from drop zone if needed
        if (currentZone != null)
        {
            currentZone.UnassignBlock(transform);
            currentZone = null;
        }
    }
}
