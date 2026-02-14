using UnityEngine;
using UnityEngine.AI;
using ImprovedTimers;
public class NpcShopState : NpcBaseState
{
    private readonly NavMeshAgent agent;
    private readonly Vector3 startPoint;
    
    private readonly CountdownTimer shopWaitTimer;
    private const float WaitTime = 6.5f;
    
    private GameObject[] shops;
    private bool isShoppingDone;
    public NpcShopState(AIEntitiy entity, Animator animator, NavMeshAgent agent) : base(entity, animator)
    {
        this.agent = agent;
        startPoint = entity.transform.position;
        
        shopWaitTimer = new CountdownTimer(WaitTime);
    }
    
    public override void OnEnter()
    {
        Debug.Log("Shop entered state");
        shops = GameObject.FindGameObjectsWithTag("Shop");
        shopWaitTimer.Reset();
        isShoppingDone = false;
    }
    public override void Update()
    {
        CalculateNearestShop();
        if (HasReachedDestination())
        {
            Shop();
        }
    }

    private void CalculateNearestShop()
    {
        var shortestDistance = 0;
        for(var i = 1; i < shops.Length; i++)
        {
            var currentDistance = Vector3.Distance(shops[i].transform.position, startPoint);
            var previousDistance = Vector3.Distance(shops[shortestDistance].transform.position, startPoint);
            if (previousDistance > currentDistance)
            {
                shortestDistance = i;
            }
        }
        agent.SetDestination(shops[shortestDistance].transform.position);
    }

    private void Shop()
    {
        shopWaitTimer.Tick();
        if (shopWaitTimer.IsFinished)
        {
            //!Instantiate a shopping bag
            isShoppingDone = true;
        }
    }
    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
    public bool ExitForWandererState()
    {
        return isShoppingDone;
    }
}