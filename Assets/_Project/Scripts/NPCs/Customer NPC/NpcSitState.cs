using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Linq;
using TMPro;

public class NpcSitState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private FoodItem selectedItem;
    private CustomerName customerName;
    private GameObject heldItem;
    private FoodItemInfoManager foodItemInfoManager;
    private GameObject[] chairs;
    private ChairData randomChair;
    private Canvas canvas;
    private TextMeshProUGUI nameText;
    private Transform playerTransform;
    private Transform parentTransform;
    private bool isPlayerLookingAtMe;
    
    public static event Action<AIEntitiy> OnFoodGiven;
    public NpcSitState(AIEntitiy entity, 
        Animator animator, 
        NavMeshAgent agent, 
        ChangeStateCustomerManager changeStateManager, 
        CustomerName customerName,
        Transform playerTransform, 
        Canvas canvas, 
        Transform parentTransform, 
        bool isPlayerLookingAtMe) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        this.customerName = customerName;
        this.canvas = canvas;
        this.playerTransform = playerTransform;
        this.parentTransform = parentTransform;
        foodItemInfoManager = FoodItemInfoManager.Instance;
        chairs = GameObject.FindGameObjectsWithTag("Chair");
        this.isPlayerLookingAtMe = isPlayerLookingAtMe;
    }

    public override void OnEnter()
    {
        var pickedChairs = chairs
            .Select(chair => chair.GetComponent<ChairData>())
            .Where(chairData => chairData != null && !chairData.isOccupied)
            .ToList();
        
        nameText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        if (pickedChairs.Count > 0)
        {
            randomChair = pickedChairs[Random.Range(0, pickedChairs.Count)];
        }
        
        agent.SetDestination(randomChair.position);
        randomChair.isOccupied = true;
        if(foodItemInfoManager.foodItemDictionary.TryGetValue(entity.gameObject, out var foodItem))
        {
            selectedItem = foodItem;
        }
    }

    public override void Update()
    {
        UpdateWorldUI();
        if(foodItemInfoManager.foodItemDictionary.TryGetValue(entity.gameObject, out var foodItem))
        {
            selectedItem = foodItem;
        }
        if (!changeStateManager.PlayerInRange || !entity.IsPlayerLookingAtMe()) return;
        
        if (InputManager.Instance.Interact())
        {
            GiveFood();
        }
    }

    private void UpdateWorldUI()
    {
        var targetRot = Quaternion.LookRotation(playerTransform.forward);
        nameText.gameObject.transform.rotation = Quaternion.Slerp(nameText.gameObject.transform.rotation, targetRot, Time.deltaTime * 180f);
        
    }

    private void GiveFood()
    {
        heldItem = parentTransform.GetChild(0).gameObject;
        var order = foodItemInfoManager.GetCustomerOrderKeys()[0];
        // Check if the player is holding the correct food item the customer ordered and if the customer is the one who ordered it
        if (heldItem.name != selectedItem.dishName && 
            order.Customer.name != entity.gameObject.name) return;
                
        // Notify the player that the food has been given, so the food can be destroyed in the background;
        OnFoodGiven?.Invoke(entity); 
        changeStateManager.OrderServed = true;
    }
    public override void OnExit()
    {
        randomChair.isOccupied = false;
    }

    public T Get<T>() where T : ChairData
    {
        return randomChair as T;
    }
    public void Set<T>(T component)
    {
        randomChair = component as ChairData;
    }
}