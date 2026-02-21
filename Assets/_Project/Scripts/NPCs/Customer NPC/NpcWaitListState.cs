using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class NpcWaitListState : NpcBaseState
{
    private NavMeshAgent agent;
    private List<OrderNode> orderQueue;
    private ChangeStateCustomerManager changeStateManager;
    public NpcWaitListState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager, List<OrderNode> orderQueue) : base(entity, animator)
    {
        this.agent = agent; 
        this.orderQueue = orderQueue;
        this.changeStateManager = changeStateManager;
    }
    public override void OnEnter()
    {
        Debug.Log("Waitlist entered state");
    }
    public override void Update()
    {
        CheckLineQuantity();
    }

    private void CheckLineQuantity()
    {
        changeStateManager.IsLineFull = orderQueue.All(order => order.isOccupied);
    }
}