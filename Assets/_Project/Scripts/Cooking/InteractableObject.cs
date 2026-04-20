using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected PlayerController playerScript;
    [SerializeField] protected GameObject miniGameParentGameObject;
    [SerializeField] protected Camera miniGameCamera;
    [Tooltip("If you don't want a custom cursor, you can leave this field empty it will not bring up an error")]
    [SerializeField] protected Texture2D customCursor;
    [Tooltip("If you want to use a custom cursor, set this to true and set the customCursor field.")]
    [SerializeField] protected bool withCustomCursor;
    [SerializeField] protected Canvas scoreCanvas;
    [SerializeField] protected bool withScoreCanvas;
    [SerializeField] protected MenuData menuSO; // Scriptable Object
    protected FoodItemInfoManager orders;
    protected InputManager inputManager;
    private bool playerInRange;
    protected bool miniGameRunning;
    private bool canPlayMiniGame;
    private bool miniGameHasBeenPlayed;
    private CinemachineInputAxisController inputAxisController;
    private HideLockCursor cursor;
    protected event Action OnMiniGameStart;
    public static event Action OnOrderComplete;
    public static event Action OnMiniGameEnd;
    public virtual void Interact()
    {
        if (!playerInRange) return;
        if (!inputManager.Interact()) return;

        canPlayMiniGame = CheckOrderForProcessing();
        if (!canPlayMiniGame) return;

        var (customerOrder, myType) = GetCurrentOrderKey();

        if (!orders.customersOrderDictionary.TryGetValue(customerOrder, out var orderList))
        {
            
            return;
        }
            

        miniGameHasBeenPlayed = false;

        foreach (var order in orderList)
        {
            if (order.Item1 is not null && order.Item1.GetType() == myType)
            {
                miniGameHasBeenPlayed = order.Item2;
                break;
            }
        }

        if (miniGameHasBeenPlayed) return;

        StartComponents();
    }
    bool CheckOrderForProcessing()
    {
        var currentOrder = orders.GetFoodItemFromCustomer();
        if (currentOrder == null)
            return false;
        
        Debug.Log($"I've made it passed menuOrder: {currentOrder.dishName}");
        if (currentOrder.processes == null || currentOrder.processes.Count == 0)
            return false;

        var myType = GetType();
        
        Debug.Log($"I've made it passed myType: {myType}");
        foreach (var process in currentOrder.processes)
        {
            if (process == null)
                continue;

            var processComponent = process.GetComponent<InteractableObject>();
            Debug.Log($"I've made it passed processComponent: {processComponent}");
            if (processComponent == null)
                continue;

            if (processComponent.GetType() == myType)
            {
                Debug.Log($"I've made it passed GetType: {processComponent.GetType()}");
                return true;
            }
        }
        Debug.Log("I've made it passed the end of the loop");
        return false;
    }

    public virtual void Awake()
    {
        inputManager = InputManager.Instance;
        cursor = HideLockCursor.Instance;
        orders = FoodItemInfoManager.Instance;
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
        miniGameParentGameObject.SetActive(true);
        playerScript.enabled = false;
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
        
        MiniGameComplete();
        playerScript.enabled = true;
        SetupCursor(false, CursorLockMode.Locked, withCustomCursor, true);
        
        if (inputAxisController is not null)
        {
            inputAxisController.enabled = true;
        }
    }

    private void MiniGameComplete()
    {
        var (customerOrder, myType) = GetCurrentOrderKey();

        if (!orders.customersOrderDictionary.TryGetValue(customerOrder, out var ordersList))
            return;
        for (int i = 0; i < ordersList.Count; i++)
        {
            var entry = ordersList[i];
            if (entry.Item1 is not null && entry.Item1.GetType() == myType)
            {
                entry.Item2 = true;
                ordersList[i] = entry;
                OnMiniGameEnd?.Invoke();
                break;
            }
        }
        CheckOrdersComplete(customerOrder,ordersList);
    }

    private (CustomerOrderKey, Type) GetCurrentOrderKey()
    {
        var myType = GetType();
        var item = orders.GetFoodItemFromCustomer();
        if (item == null) return (default, myType);

        var customer = orders.GetCustomer(item);
        return (new CustomerOrderKey(customer, item), myType);
    }

    private void CheckOrdersComplete(CustomerOrderKey customer, List<(InteractableObject, bool)> ordersList)
    {
        var completedProcesses = ordersList.Count(order => order.Item2);

        if (completedProcesses == ordersList.Count)
        {
            var removed = orders.customersOrderDictionary.Remove(customer);
            OnOrderComplete?.Invoke();
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