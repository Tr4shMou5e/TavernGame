using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool Interact()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }
}
