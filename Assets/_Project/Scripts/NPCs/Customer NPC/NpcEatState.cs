using System;
using UnityEngine;
using UnityEngine.AI;
using ImprovedTimers;
public class NpcEatState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private CountdownTimer eatTimer;
    public NpcEatState(NpcCustomer npcCustomer, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager, float eatDuration = 10f) : base(npcCustomer, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        eatTimer = new CountdownTimer(eatDuration);
    }
    
    public override void OnEnter()
    {
        Debug.Log("Eating entered state");
        eatTimer.Reset();
        eatTimer.Start();
    }
    public override void Update()
    {
        if (!eatTimer.IsFinished) return;
        Debug.Log("Eating finished");
        changeStateManager.HasFinishedEating = true;
    }
}