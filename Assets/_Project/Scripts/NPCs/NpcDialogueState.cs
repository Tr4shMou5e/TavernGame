using UnityEngine;
using UnityEngine.AI;

public class NpcDialogueState : NpcBaseState
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    public NpcDialogueState(AIEntitiy entity, Animator animator, NavMeshAgent agent, Transform playerTransform) : base(entity, animator)
    {
        this.agent = agent;
        this.entity = entity;
        this.playerTransform = playerTransform;
    }

    public override void OnEnter()
    {
        Debug.Log("Dialogue entered state");
        if (agent.path.status != NavMeshPathStatus.PathComplete || agent.path.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = true;
        }
        FacePlayer();
    }
    private void FacePlayer()
    {
        var targetRot = Quaternion.LookRotation(-playerTransform.forward);
        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, targetRot, Time.deltaTime * 180f);
    }
}