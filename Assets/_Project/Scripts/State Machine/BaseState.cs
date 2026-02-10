using UnityEngine;
using UnityEngine.LowLevel;

public abstract class BaseState : IState
{
    //!This is an example if we were to create a base state for the player
    
    // protected readonly PlayerController player;
    // protected readonly Animator animator;
    //
    // protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    // protected static readonly int JumpHash = Animator.StringToHash("Jump");
    //
    // protected const float crossFadeDuration = 0.1f;
    //
    // protected BaseState(PlayerController player, Animator animator)
    // {
    //     this.player = player;
    //     this.animator = animator;
    // }
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