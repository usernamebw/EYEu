using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XROriginMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction; // Assign in Inspector
    [SerializeField] private float moveSpeed = 2.0f;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing on XR Origin!");
        }
    }

    private void Update()
    {
        if (moveAction == null || characterController == null)
            return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        moveDirection = transform.TransformDirection(moveDirection); // Move relative to XR Origin rotation
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
