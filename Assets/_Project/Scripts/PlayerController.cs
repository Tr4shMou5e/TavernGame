using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float turnSpeed = 180f;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Camera cam;
    private bool groundedPlayer;

    [Header("Input Actions")]
    private InputManager moveAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        cam = Camera.main;
        moveAction = InputManager.Instance;
    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0f)
            playerVelocity.y = -2f;

        Vector2 input = moveAction.GetPlayerPosition();

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.y + camRight * input.x);
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        if (moveDir.sqrMagnitude < 0.001f || moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move (horizontal + vertical)
        Vector3 motion = moveDir * playerSpeed;
        motion.y = playerVelocity.y;

        controller.Move(motion * Time.deltaTime);
    }
}
