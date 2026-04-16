using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideLockCursor : MonoBehaviour
{
    [SerializeField] CursorLockMode lockState;
    public CursorLockMode LockState { set => lockState = value; }
    [SerializeField] bool isVisible; 
    public bool IsVisible { set => isVisible = value; }
    
    private static HideLockCursor instance;
    public static HideLockCursor Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Cursor.lockState = lockState;
        Cursor.visible = isVisible;
    }

    public void SetLockState(CursorLockMode state)
    {
        LockState = state;
    }
    public void SetVisibility(bool state)
    {
        isVisible = state;
    }
    public void ChangeCursorSprite(Texture2D sprite)
    {
        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
    }
}
