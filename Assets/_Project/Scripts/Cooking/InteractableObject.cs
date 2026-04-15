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
    [Tooltip("If you want to use a custom cursor, set this to true and set the customCursor field.")]
    [SerializeField] protected bool withCustomCursor;
    [SerializeField] protected Canvas scoreCanvas;
    [SerializeField] protected bool withScoreCanvas;
    protected InputManager inputManager;
    private bool playerInRange;
    protected bool miniGameRunning;
    private bool canPlayMiniGame;
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
        if (cursor is null)
        {
            Debug.LogError("Cursor not found! Trying again with a different method");
            cursor = FindAnyObjectByType<HideLockCursor>();
        }
        inputAxisController = FindFirstObjectByType<CinemachineInputAxisController>();
    }
    
    /// <summary>
    /// This starts the mini-game.
    /// </summary>
    private void StartComponents()
    {
        miniGame.SetActive(true);
        player.enabled = false;
        miniGameRunning = true;
        SetupCursor(true, CursorLockMode.None, withCustomCursor);
        if (withScoreCanvas)
        {
            scoreCanvas.gameObject.SetActive(true);
        }
        if(inputAxisController is not null)
        {
            inputAxisController.enabled = false;
        }
        OnMiniGameStart?.Invoke();
    }

    /// <summary>
    /// This reverts the main game to normal after the mini-game is over.
    /// Make sure to set miniGame.gameObject.SetActive(false) outside of this method.
    /// </summary>
    private void StopComponents()
    {
        player.enabled = true;
        SetupCursor(false, CursorLockMode.Locked, withCustomCursor, true);
        if (withScoreCanvas)
        {
            scoreCanvas.gameObject.SetActive(false);
        }
        
        if (inputAxisController is not null)
        {
            inputAxisController.enabled = true;
        }
    }
    /// <summary>
    /// Stops the mini-game.
    /// </summary>

    protected void EndMiniGame()
    {
        StopComponents();
    }
    protected void SetupCursor(bool isVisible, CursorLockMode state, bool withCustom, bool restCursor = false)
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
    protected void CanPlay(bool trigger) => canPlayMiniGame = trigger;
}