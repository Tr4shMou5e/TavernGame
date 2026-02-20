using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private InputSystem_Actions input;
    private static InputManager instance;
    public static InputManager Instance => instance;
    
    // Singleton Pattern Implementation
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("InputManager already exists!");
            return;
        }

        instance = this;
        input = new InputSystem_Actions();
    }
    public Vector2 GetPlayerPosition()
    {
        return input.Player.Move.ReadValue<Vector2>();
    }

    public bool Interact()
    {
        return input.Player.Interact.WasPressedThisFrame();
    }

    public bool PlayerIsJumping()
    {
        return input.Player.Jump.triggered;
    }
    void OnEnable()
    {
        input.Enable();
    }
    void OnDisable()
    {
        input.Disable();
    }
}
