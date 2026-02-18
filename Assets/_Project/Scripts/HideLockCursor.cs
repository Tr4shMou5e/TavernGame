using System;
using UnityEngine;

public class HideLockCursor : MonoBehaviour
{
    [SerializeField] CursorLockMode lockState;
    public CursorLockMode LockState { set => lockState = value; }
    [SerializeField] bool isVisible; 
    public bool IsVisible { set => isVisible = value; }
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
        Debug.Log("Setting lock state to " + state);
        LockState = state;
    }
    public void SetVisibility(bool state)
    {
        Debug.Log("Setting visibility to " + state);
        isVisible = state;
    }
}
