using System;
using Unity.Cinemachine;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected PlayerController playerScript;
    [SerializeField] protected GameObject miniGameParentGameObject;
    [SerializeField] protected Camera miniGameCamera;
    [SerializeField] protected Texture2D customCursor;
    [SerializeField] protected bool withCustomCursor;
    [SerializeField] protected Canvas scoreCanvas;
    [SerializeField] protected bool withScoreCanvas;
    [SerializeField] protected MenuData menuSO;

    protected FoodItemInfoManager orders;
    protected InputManager inputManager;
    protected bool miniGameRunning;
    protected bool isGameOver;

    private HideLockCursor cursor;
    private CinemachineInputAxisController inputAxisController;

    protected event Action OnMiniGameStart;
    public static event Action<CustomerOrderKey> OnOrderDone;
    public static event Action OnOrderComplete;
    public static event Action OnMiniGameEnd;
    public static event Action<bool> OnGameLost;

    public virtual void Awake()
    {
        inputManager = InputManager.Instance;
        cursor = HideLockCursor.Instance;
        orders = FoodItemInfoManager.Instance;
        inputAxisController = FindFirstObjectByType<CinemachineInputAxisController>();
    }

    public virtual void Interact()
    {
        // Base implementation
    }

    protected void EndMiniGame()
    {
        miniGameRunning = false;
        if (playerScript != null)
            playerScript.enabled = true;
        SetupCursor(false, CursorLockMode.Locked, withCustomCursor, true);
        if (inputAxisController != null)
            inputAxisController.enabled = true;
    }

    protected void SetupCursor(bool isVisible, CursorLockMode state, bool withCustom, bool restCursor = false)
    {
        if (cursor != null)
        {
            cursor.SetVisibility(isVisible);
            cursor.SetLockState(state);
            if (withCustom && !restCursor)
            {
                cursor.ChangeCursorSprite(customCursor);
            }
            else if (restCursor)
            {
                cursor.ChangeCursorSprite(null);
            }
        }
    }

    protected void ToggleHasWon(bool won) => OnGameLost?.Invoke(won);

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Override in subclasses
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Override in subclasses
    }
}
