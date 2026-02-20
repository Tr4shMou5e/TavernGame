using UnityEngine;
using UnityEngine.AI;

public class NpcDialogueState : NpcBaseState
{
    private NavMeshAgent agent;
    
    public NpcDialogueState(AIEntitiy entity, Animator animator, NavMeshAgent agent) : base(entity, animator)
    {
        this.agent = agent;
        this.entity = entity;
    }

    public override void OnEnter()
    {
        Debug.Log("Dialogue entered state");
        if (agent.path.status != NavMeshPathStatus.PathComplete || agent.path.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = true;
        }
    }
}