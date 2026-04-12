using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected PlayerController player;
    [SerializeField] protected GameObject miniGame;
    [SerializeField] protected Camera miniGameCamera;
    [Tooltip("If you don't want a custom cursor, you can leave this field empty it will not bring up an error")]
    [SerializeField] protected Texture2D customCursor;
    protected InputManager inputManager;
    protected bool playerInRange;
    protected bool miniGameRunning;
    /// <summary>
    /// If you want to use a custom cursor,
    /// set this to true and set the customCursor field.
    /// </summary>
    protected bool withCustomCursor;
    private CinemachineInputAxisController inputAxisController;
    private HideLockCursor cursor;
    protected event Action OnMiniGameStart;
    
    public virtual void Interact()
    {
        if (!playerInRange) return;
        if (inputManager.Interact())
        {
            StartComponents();
        }
            
    }

    public virtual void Awake()
    {
        inputManager = InputManager.Instance;
        cursor = HideLockCursor.Instance;
        inputAxisController = FindFirstObjectByType<CinemachineInputAxisController>();
    }
    
    /// <summary>
    /// Starts or stops the mini-game.
    /// Make sure to call this method when the player interacts with the object.
    /// Place this at the top of the Interact() method.
    /// </summary>

    protected void EndMiniGame()
    {
        StopComponents();
    }

    /// <summary>
    /// This reverts the main game to normal after the mini-game is over.
    /// </summary>
    private void StopComponents()
    {
        miniGame.SetActive(false);
        player.enabled = true;
        miniGameRunning = false;
        cursor.IsVisible = false;
        cursor.LockState = CursorLockMode.Locked;
        if (withCustomCursor)
        {
            cursor.ChangeCursorSprite(null);
        }
        if (inputAxisController is not null)
        {
            inputAxisController.enabled = true;
        }
    }
    /// <summary>
    /// This starts the mini-game.
    /// </summary>
    private void StartComponents()
    {
        miniGame.SetActive(true);
        player.enabled = false;
        miniGameRunning = true;
        cursor.IsVisible = true;
        cursor.LockState = CursorLockMode.None;
        if (withCustomCursor)
        {
            cursor.ChangeCursorSprite(customCursor);
        }
        if(inputAxisController is not null)
        {
            inputAxisController.enabled = false;
        }
        OnMiniGameStart?.Invoke();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }
    
}