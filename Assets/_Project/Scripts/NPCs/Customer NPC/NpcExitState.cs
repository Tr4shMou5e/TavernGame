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
        NpcCustomerSpawnerObjectPoolManager.Instance.ReleaseCustomer(entity.gameObject);
        NpcCustomerSpawnerObjectPoolManager.Instance.activeCustomers.Remove(entity.gameObject);
    }
    public override void OnExit()
    {
        changeStateManager.IsOrderTaken = false;
        changeStateManager.HasFinishedEating = false;
        changeStateManager.OrderServed = false;
        changeStateManager.HasOrderNode = false;
    }
}