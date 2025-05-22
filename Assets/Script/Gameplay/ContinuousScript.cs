using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource = XRNode.LeftHand;
    public float speed = 1f;
    public float gravity = -9.81f;
    public float additionalHeight = 0.2f;

    private Vector2 inputAxis;
    private CharacterController characterController;
    private XROrigin xrOrigin;
    private float fallingSpeed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();
    }

    void Update()
    {
        UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(inputSource);

        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 axis))

        {
            inputAxis = axis;
        }
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, xrOrigin.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        characterController.Move(direction * Time.fixedDeltaTime * speed);

        // Apply gravity
        bool grounded = characterController.isGrounded;
        if (grounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    private void CapsuleFollowHeadset()
    {
        Transform cameraTransform = xrOrigin.Camera.transform;
        Vector3 center = transform.InverseTransformPoint(cameraTransform.position);
        characterController.height = cameraTransform.localPosition.y + additionalHeight;
        characterController.center = new Vector3(center.x, characterController.height / 2 + characterController.skinWidth, center.z);
    }
}
