using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcWanderer : AIEntitiy
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float wanderRadius;
    StateMachine stateMachine;
    
    void Start()
    {
        stateMachine = new StateMachine();
        
        var wanderState = new NpcWandererState(this, animator, agent, wanderRadius);
        var shopState = new NpcShopState(this, animator, agent);
        
        At(wanderState, shopState, new FuncPredicate(() => wanderState.ExitForShopState()));
        At(shopState, wanderState, new FuncPredicate(() => shopState.ExitForWandererState()));
        
        stateMachine.SetState(wanderState);
    }
    
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    public void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    
    private void Update() => stateMachine.Update();
}