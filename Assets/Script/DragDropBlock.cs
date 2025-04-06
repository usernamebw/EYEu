using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropBlock : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Camera mainCamera;
    public LayerMask dropZoneLayer; // Assign DropZone layer in inspector
    public float dropCheckRadius = 1f;

    private Transform assignedSlot;

    private void Start()
    {
        originalPosition = transform.position;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2f; // Adjust based on camera distance
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
        Debug.Log($"[DropCheck] Checking {hits.Length} colliders in radius {dropCheckRadius}");

        foreach (var hit in hits)
        {
            Debug.Log($"[DropCheck] Hit object: {hit.gameObject.name}");

            if (hit.gameObject == this.gameObject)
                continue;

            DropZone zone = hit.GetComponent<DropZone>() ?? hit.GetComponentInParent<DropZone>();

            if (zone != null)
            {
                Debug.Log("[DropCheck] Valid DropZone found.");

                assignedSlot = zone.GetAvailableSlot();
                if (assignedSlot != null)
                {
                    transform.position = assignedSlot.position;
                    transform.SetParent(assignedSlot); // Make the block a child of the slot
                    Debug.Log($"[DropCheck] Snapped to slot at: {assignedSlot.position}");

                    zone.UpdateAnswerDisplay(); // Update TMP answer text
                    return;
                }
                else
                {
                    Debug.Log("[DropCheck] No available slot.");
                }
            }
        }

        Debug.Log("[DropCheck] Returning to original position.");
        transform.position = originalPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dropCheckRadius);
    }
}
