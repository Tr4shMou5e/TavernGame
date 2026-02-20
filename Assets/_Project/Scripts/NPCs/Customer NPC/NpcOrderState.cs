using UnityEngine;
using UnityEngine.AI;

public class NpcOrderState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    
    public NpcOrderState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
    }
    
} 