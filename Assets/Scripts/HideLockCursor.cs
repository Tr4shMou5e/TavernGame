using UnityEngine;

public class HideLockCursor : MonoBehaviour
{
    public static HideLockCursor Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    public void SetLockState(CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
    }

    public void ChangeCursorSprite(Texture2D cursorTexture)
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
