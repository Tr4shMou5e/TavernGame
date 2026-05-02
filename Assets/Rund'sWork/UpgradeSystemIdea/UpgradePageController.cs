using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePageController : MonoBehaviour
{
    public GameObject upgradePage;

    private bool isOpen = false;

    void Awake()
    {
        upgradePage.SetActive(false);
    }

    void Update()
    {   
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            ToggleUpgradePage();
        }
    }

    void ToggleUpgradePage()
    {
        isOpen = !isOpen;

        upgradePage.SetActive(isOpen);

        if (isOpen)
        {
            HideLockCursor.Instance.SetLockState(CursorLockMode.None);
            HideLockCursor.Instance.SetVisibility(true);
        }
        else
        {
            HideLockCursor.Instance.SetLockState(CursorLockMode.Locked);
            HideLockCursor.Instance.SetVisibility(false);
        }
    }
}