using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class NpcOrderState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private List<OrderNode> orderQueue;
    private MenuData menu;
    private Vector3 targetPosition;
    private OrderNode currentOrderNode;
    private int currentOrderIndex;
    public NpcOrderState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager, List<OrderNode> orderQueue, MenuData menu) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        this.orderQueue = orderQueue;
        this.menu = menu;
    }
    public override void OnEnter()
    {
        Debug.Log("Order entered state");
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
        if (!changeStateManager.LineFull() && currentOrderNode != null)
        {
            agent.SetDestination(targetPosition);
        }
        // If the line is full, the agent will go to the waiting area
        else if(currentOrderNode == null)
        {
            changeStateManager.HasOrderNode = false;
        }
            
    }
    public override void Update()
    {
        UpdateOrderNode();
        CheckLineQuantity();
    }

    private void CheckLineQuantity()
    {
        changeStateManager.IsLineFull = orderQueue.All(order => order.isOccupied);
    }

    private void UpdateOrderNode()
    {
        Debug.Log("Updating order node");
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
        if(!changeStateManager.LineFull() && currentOrderNode != null)
            agent.SetDestination(currentOrderNode.position);
    }

    public override void OnExit()
    {
        if (currentOrderNode != null)
            currentOrderNode.isOccupied = false;
    }
}