using ImprovedTimers;
using UnityEngine;
using UnityEngine.AI;

public class NpcWandererState : NpcBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Vector3 startPoint;
    private readonly float wanderRadius;

    private readonly CountdownTimer waitTimer;
    private readonly CountdownTimer shopTimer;
    
    private const float WaitTime = 5.5f;
    private const float ShopTime = 25f;
    private bool waitingForNextPoint;
    
    public NpcWandererState(AIEntitiy entity, Animator animator, NavMeshAgent agent, float wanderRadius) : base(entity, animator)
    {
        this.agent = agent;
        this.wanderRadius = wanderRadius;
        startPoint = entity.transform.position;
        
        waitTimer = new CountdownTimer(WaitTime);
        shopTimer = new CountdownTimer(ShopTime);
    }
    public override void OnEnter()
    {
        Debug.Log("Wanderer entered state");
        waitingForNextPoint = false;
        waitTimer.Reset();
        shopTimer.Reset();
        Wander();
    }
    public override void Update()
    {
        UpdateWandererBehavior();
    }

    private void UpdateWandererBehavior()
    {
        //This state will exit once the shop timer is finished
        shopTimer.Tick();
        if (waitingForNextPoint)
        {
            waitTimer.Tick();

            if (waitTimer.IsFinished)
            {
                waitingForNextPoint = false;
                waitTimer.Stop();
                
                Wander();
            }

            return;
        }

        if (HasReachedDestination())
        {
            waitingForNextPoint = true;
            agent.ResetPath();
            waitTimer.Reset();
            waitTimer.Start();
        }
    }

    private void Wander()
    {
        var randomPoint = Random.insideUnitSphere * wanderRadius;
        randomPoint += startPoint;
        NavMesh.SamplePosition(randomPoint, out var hit, wanderRadius, 1);
        var finalPosition = hit.position;
        agent.SetDestination(finalPosition);
    }
    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    public bool ExitForShopState()
    {
        return shopTimer.IsFinished;
    }
}