using System;
using UnityEngine;
using UnityEngine.AI;
public class NpcCustomer : AIEntitiy
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] ChangeStateCustomerManager changeStateManager;
    private StateMachine stateMachine;
    void Start()
    {
        stateMachine = new StateMachine();
        
        var orderState = new NpcOrderState(this, animator, agent, changeStateManager);
        var sitState = new NpcSitState(this, animator, agent, changeStateManager);
        var exitState = new NpcExitState(this, animator, agent, changeStateManager);
        
        stateMachine.SetState(orderState);
    }
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    public void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    void Update()
    {
        stateMachine.Update();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}