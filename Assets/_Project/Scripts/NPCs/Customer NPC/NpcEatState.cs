using System;
using UnityEngine;
using UnityEngine.AI;
using ImprovedTimers;
public class NpcEatState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private CountdownTimer eatTimer;
    private const float EatDuration = 10f;
    public NpcEatState(NpcCustomer npcCustomer, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager) : base(npcCustomer, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        eatTimer = new CountdownTimer(EatDuration);
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