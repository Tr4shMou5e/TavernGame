using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour, IBind<PlayerData>
{
    [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
    [SerializeField] private PlayerData data;
    [SerializeField] float turnSpeed = 180f;
    [SerializeField] private float footstepInterval = 0.2f;
    private float playerSpeed = 5.0f;
    private float gravityValue = -9.81f;
    private float timer = 0f;
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
        //This is to make the player data persistent
        data.position = transform.position;
        data.rotation = transform.rotation;
        
        //Actual movement code starts here
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
        
        if (moveDir.sqrMagnitude > 0.001f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                SoundManager.PlaySound(SoundType.Footstep);
                timer = footstepInterval; 
            }
        }
    }

    public void Bind(PlayerData data)
    {
        this.data = data;
        this.data.Id = Id;
        transform.position = data.position;
        transform.rotation = data.rotation;
    }
}