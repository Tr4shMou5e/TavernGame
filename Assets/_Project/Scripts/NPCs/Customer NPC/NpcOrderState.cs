using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using TMPro;
using UnityEngine.UI;
public class NpcOrderState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private List<OrderNode> orderQueue;
    private MenuData menu;
    private CustomerName customerName;
    private FoodItem selectedItem;
    private Vector3 targetPosition;
    private OrderNode currentOrderNode;
    private Canvas canvas;
    private Image selectedItemImage;
    private TextMeshProUGUI customerNameText;
    private Transform playerTransform;
    
    private FoodItemInfoManager foodItemInfoManager;
    private string name;
    private int currentOrderIndex;
    private bool isFoodSelected;

    public static event Action OnOrderTaken;
    public NpcOrderState(AIEntitiy entity, 
        Animator animator, 
        NavMeshAgent agent, 
        Transform playerTransform, 
        ChangeStateCustomerManager changeStateManager, 
        List<OrderNode> orderQueue, 
        MenuData menu, 
        Canvas canvas, 
        CustomerName customerName) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        this.orderQueue = orderQueue;
        this.menu = menu;
        this.customerName = customerName;
        this.canvas = canvas;
        this.playerTransform = playerTransform;
        isFoodSelected = false;
        foodItemInfoManager = FoodItemInfoManager.Instance;
    }
    public override void OnEnter()
    {
        Debug.Log("Order entered state");
        NpcCustomer.OnOrderTaken += RegisterFoodItemOrder;
        // Checking if the Line is full
        for (int i = 0; i < orderQueue.Count; i++)
        {
            if (!orderQueue[i].isOccupied)
            {
                currentOrderNode = orderQueue[i];
                targetPosition = orderQueue[i].position;
                orderQueue[i].isOccupied = true;
                currentOrderIndex = i;
                changeStateManager.HasOrderNode = true;
                break;
            }
        }
        
        // If the line is not full, the agent will move to the next order node
        if (!changeStateManager.LineFull() && currentOrderNode is not null)
        {
            agent.SetDestination(targetPosition);
        }
        // If the line is full, the agent will go to the waiting area
        else if(currentOrderNode is null)
        {
            changeStateManager.HasOrderNode = false;
        }
        if(selectedItemImage is null) selectedItemImage = canvas.gameObject.GetComponentInChildren<Image>();
        if(customerNameText is null) customerNameText = canvas.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if(customerNameText is not null) name = customerName.GetRandomName();
    }
    public override void Update()
    {
        UpdateOrderNode();
        CheckLineQuantity();
        DisplayOrder();
        
        
    }

    private void CheckLineQuantity()
    {
        changeStateManager.IsLineFull = orderQueue.All(order => order.isOccupied);
    }

    private void UpdateOrderNode()
    {
        if(orderQueue.Count == 0) return;
        for(int i = currentOrderIndex; i >= currentOrderIndex - 1; i--)
        {
            if(i < 0) break;
            
            if(!orderQueue[i].isOccupied)
            {
                orderQueue[i].isOccupied = true;
                orderQueue[currentOrderIndex].isOccupied = false;
                currentOrderNode = orderQueue[i];
                currentOrderIndex = i;
                break;
            }
        }

        if (!changeStateManager.LineFull() && currentOrderNode is not null)
        {
            agent.SetDestination(currentOrderNode.position);
        }
            
    }
    private void DisplayOrder()
    {
        if (currentOrderIndex == 0 && currentOrderNode is not null)
        {
            if(!isFoodSelected)
                SelectFoodItem();
            ShowWorldGUI();
            FacePlayer();
        }
    }
    
    private void SelectFoodItem()
    {   
        canvas.gameObject.SetActive(true);
        selectedItem = menu.SelectRandomMenuItem();
        var foodItem = new FoodItem
        {
            dishName = selectedItem.dishName,
            dishImage = selectedItem.dishImage,
            price = selectedItem.price,
            id = selectedItem.id,
            processesRequired = selectedItem.processesRequired
        };
        entity.gameObject.name = name;
        selectedItem = foodItem;
        isFoodSelected = true;
    }

    private void RegisterFoodItemOrder(AIEntitiy customer)
    {
        if (customer != entity)
            return;
        if (selectedItem == null)
            SelectFoodItem();

        if (selectedItem == null)
        {
            Debug.LogError("Selected item is still null after SelectFoodItem().");
            return;
        }

        if (foodItemInfoManager == null || entity == null || entity.gameObject == null)
        {
            Debug.LogError("Missing required references in RegisterFoodItemOrder.");
            return;
        }

        var key = new CustomerOrderKey(entity.gameObject, selectedItem);
        if (foodItemInfoManager.customersOrderDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Order already registered for {entity.gameObject.name}");
            return;
        }

        foodItemInfoManager.foodItemDictionary[entity.gameObject] = selectedItem;

        var order = new List<(InteractableObject, bool)>();

        foreach (var processObject in selectedItem.processesRequired)
        {
            if (processObject == null) continue;

            var interactable = processObject.GetComponent<InteractableObject>();
            if (interactable != null)
                order.Add((interactable, false));
        }

        if (order.Count == 0)
        {
            Debug.LogError($"No valid interactable processes found for {selectedItem.dishName}.");
            return;
        }

        foodItemInfoManager.AddCustomerOrder(order, entity.gameObject, selectedItem);
        OnOrderTaken?.Invoke();
    }

    private void ShowWorldGUI()
    {
        if (selectedItem is null || name is null) return;
             
        selectedItemImage.sprite = selectedItem.dishImage;
        customerNameText.text = name;
    }
    private void FacePlayer()
    {
        var targetRot = Quaternion.LookRotation(-playerTransform.forward);
        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, targetRot, Time.deltaTime * 180f);
    }
    public override void OnExit()
    {
        NpcCustomer.OnOrderTaken -= RegisterFoodItemOrder;
        if (currentOrderNode != null)
            currentOrderNode.isOccupied = false;
    }
}