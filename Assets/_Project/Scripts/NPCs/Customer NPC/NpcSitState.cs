using UnityEngine;
using UnityEngine.AI;

public class NpcSitState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private GameObject[] chairs;
    public NpcSitState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }

    public override void OnEnter()
    {
        Debug.Log("Sitting entered state");
        
        agent.SetDestination(chairs[0].transform.position);
    }

    public override void Update()
    {
        Debug.Log("Sitting update state");
        Debug.Log(changeStateManager.PlayerInRange);
        if (!changeStateManager.PlayerInRange) return;
        Debug.Log("Player in range");
        if (InputManager.Instance.Interact())
        {
            GiveFood();
        }
    }

    private void GiveFood()
    {
        //!Give Order (Implement Later)
        changeStateManager.OrderServed = true;
    }
}