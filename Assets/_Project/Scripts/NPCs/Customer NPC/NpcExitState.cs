using UnityEngine;
using UnityEngine.AI;

public class NpcExitState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    
    public NpcExitState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
    }

    public override void OnEnter()
    {
        Debug.Log("Exiting entered state");
    }
}