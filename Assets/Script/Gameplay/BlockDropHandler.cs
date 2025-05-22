using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockDropHandler : MonoBehaviour
{
    private XRGrabInteractable grab;
    private Rigidbody rb;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Cast a short overlap sphere to detect DropZone (like the table)
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (var hit in hits)
        {
            DropZone zone = hit.GetComponent<DropZone>();
            if (zone != null)
            {
                Transform slot = zone.GetAvailableSlot();
                if (slot != null)
                {
                    // Snap block to the available slot
                    transform.position = slot.position;
                    transform.rotation = slot.rotation;
                    transform.SetParent(slot); // Parent to slot
                    rb.isKinematic = true;

                    zone.UpdateAnswerDisplay();
                }
                return;
            }
        }
    }
}
