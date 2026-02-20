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

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        // Read input
        Vector2 input = moveAction.GetPlayerPosition();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = Vector3.ClampMagnitude(move, 1f);
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        move = forward * move.z + right * move.x;
        forward.y = 0;
        right.y = 0;
        move.y = 0;
        if (move != Vector3.zero)
            transform.forward = move;

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Move
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);

        if (!(finalMove.magnitude  > 0.1f)) return;
        var targetRotation = Quaternion.LookRotation(move, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
