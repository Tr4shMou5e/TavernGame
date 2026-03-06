using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcCustomer : AIEntitiy
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] ChangeStateCustomerManager changeStateManager;
    [SerializeField] List<OrderNode> orderQueue;
    [SerializeField] MenuData menuData;
    [SerializeField] CustomerName customerName;
    [SerializeField] Canvas canvas;
    [SerializeField] private float eatDuration = 10f;
    private StateMachine stateMachine;
    void Start()
    {
        stateMachine = new StateMachine();
        var customerNameObject = ScriptableObject.CreateInstance<CustomerName>();
        customerNameObject.names = customerName.names;
        
        var orderState = new NpcOrderState(this, animator, agent, player, changeStateManager, orderQueue, menuData, canvas, customerNameObject);
        var waitListState = new NpcWaitListState(this, animator, agent, changeStateManager, orderQueue);
        var sitState = new NpcSitState(this, animator, agent, changeStateManager, customerName);
        var eatState = new NpcEatState(this, animator, agent, changeStateManager, eatDuration);
        var exitState = new NpcExitState(this, animator, agent, changeStateManager);
        
        At(orderState, sitState, new FuncPredicate(() => changeStateManager.OrderTaken()));
        At(orderState, waitListState, new FuncPredicate(() => changeStateManager.LineFull() && !changeStateManager.HasOrder()));
        At(waitListState, orderState, new FuncPredicate(() => !changeStateManager.LineFull()));
        At(sitState, eatState, new FuncPredicate(() => changeStateManager.HasOrderServed()));
        At(eatState, exitState, new FuncPredicate(() => changeStateManager.FinishedEating()));
        
        stateMachine.SetState(orderState);
    }
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    void Update()
    {
        stateMachine.Update();
        
        changeStateManager.PlayerInRange = isPlayerInRange;
        if (!isPlayerInRange) return;
        
        if (InputManager.Instance.Interact())
        {
            changeStateManager.IsOrderTaken = true;
        }
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}