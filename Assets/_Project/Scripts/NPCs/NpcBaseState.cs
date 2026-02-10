using UnityEngine;

public abstract class NpcBaseState : IState
{
    protected readonly Animator animator;
    protected AIEntitiy entity;
    protected NpcBaseState(AIEntitiy entity, Animator animator)
    {
        this.entity = entity;
        this.animator = animator;
    }
    public virtual void OnEnter()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void OnExit()
    {
        // noop
    }
}