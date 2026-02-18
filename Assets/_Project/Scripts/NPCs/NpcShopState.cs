using UnityEngine;
using UnityEngine.AI;
using ImprovedTimers;
public class NpcShopState : NpcBaseState
{
    private readonly NavMeshAgent agent;
    
    private readonly CountdownTimer shopWaitTimer;
    private const float WaitTime = 6.5f;
    
    private GameObject[] shops;
    ChangeStateWandererManager changeStateManager;
    private bool isShoppingDone;
    public NpcShopState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateWandererManager changeStateManager) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        
        shopWaitTimer = new CountdownTimer(WaitTime);
    }
    
    public override void OnEnter()
    {
        Debug.Log("Shop entered state");
        shops = GameObject.FindGameObjectsWithTag("Shop");
        Debug.Log(shops.Length);
        shopWaitTimer.Reset();
        shopWaitTimer.Start();
        changeStateManager.IsShoppingDone = false;
        changeStateManager.IsShopWaitTimeDone = false;
        CalculateNearestShop();
    }
    public override void Update()
    {
        if (HasReachedDestination())
        {
            Shop();
        }
    }

    private void CalculateNearestShop()
    {
        if (agent == null || shops == null || shops.Length == 0 || shops[0] == null) return;

        var nearestIndex = 0;
        var nearestDistance = Vector3.Distance(entity.transform.position,shops[0].transform.position);
        Debug.Log(nearestDistance);
        for (var i = 1; i < shops.Length; i++)
        {
            var currentDistance = Vector3.Distance(entity.transform.position,shops[i].transform.position);
            Debug.Log(currentDistance);
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestIndex = i;
            }
        }

        agent.SetDestination(shops[nearestIndex].transform.position);
    }

    private void Shop()
    {
        if (shopWaitTimer.IsFinished)
        {
            //!Instantiate a shopping bag?
            changeStateManager.ShopTimer.Reset();
            changeStateManager.IsShopWaitTimeDone = true;
            changeStateManager.IsShoppingDone = true;
        }
    }
    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
    
}