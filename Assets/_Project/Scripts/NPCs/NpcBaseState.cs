using UnityEngine;

public abstract class NpcBaseState : IState
{   
    protected static readonly int WalkHash = Animator.StringToHash("Walking");
    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int SitHash = Animator.StringToHash("sitting down");
    protected static readonly int FoodHash = Animator.StringToHash("Food state");

    protected static int isSitting = Animator.StringToHash("IsSitIdle");
    protected static int isWalking = Animator.StringToHash("isWalking");
    protected static int isEating = Animator.StringToHash("HasFood");


    protected const float crossFadeDuration = 0.1f;

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

    public virtual T Get<T>()
    {
        // noop
        return default;
    }

    public virtual void Set<T>(T component)
    {
        // noop
    }

    protected void WalkingAnimationState(bool walkingState)
    {
        animator.SetBool(isWalking,walkingState);
    }

    protected void SittingAnimationState(bool sittingState)
    {
        animator.SetBool(isSitting,sittingState);
    }

    protected void EatingAnimationState(bool eatingState)
    {
        animator.SetBool(isEating,eatingState);
    }
}