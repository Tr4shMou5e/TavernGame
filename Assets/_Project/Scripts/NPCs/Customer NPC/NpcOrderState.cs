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
        // Checking if the Line is full
        Debug.Log(orderQueue.Count);
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
        if (!changeStateManager.LineFull() && currentOrderNode != null)
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
        if(customerNameText != null) name = customerName.GetRandomName();
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
        Debug.Log("Updating order node");
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

        if (!changeStateManager.LineFull() && currentOrderNode != null)
        {
            agent.SetDestination(currentOrderNode.position);
        }
            
    }
    private void DisplayOrder()
    {
        if (currentOrderIndex == 0 && currentOrderNode != null)
        {
            if(!isFoodSelected)
                SelectFoodItem();
            ShowWorldGUI();
            FacePlayer();
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
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
            processes = selectedItem.processes
        };
        entity.gameObject.name = name;
        foodItemInfoManager.foodItemDictionary[entity.gameObject] = foodItem;
        
        var order = new List<(InteractableObject, bool)>();
        foreach (var item in foodItem.processes)
        {
            order.Add((item.GetComponent<InteractableObject>(), false));
        }
        
        foodItemInfoManager.AddCustomerOrder(order, entity.gameObject, foodItem);
        OnOrderTaken?.Invoke();
        Debug.Log("Order taken");
        isFoodSelected = true;
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
        if (currentOrderNode != null)
            currentOrderNode.isOccupied = false;
        
        canvas.gameObject.SetActive(false);
    }
}