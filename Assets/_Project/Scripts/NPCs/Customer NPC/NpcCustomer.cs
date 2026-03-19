using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

public class NpcCustomer : AIEntitiy
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] ChangeStateCustomerManager changeStateManager;
    [SerializeField] List<OrderNode> orderQueue;
    [SerializeField] MenuData menuData;
    [SerializeField] Canvas canvas;
    [SerializeField] private float eatDuration = 10f;
    
    private bool hasPendingBind;
    private StateMachine stateMachine;
    
    private IState orderState;
    private IState waitListState;
    private IState sitState;
    private IState eatState;
    private IState exitState;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        orderQueue = FindObjectsByType<OrderNode>(FindObjectsSortMode.None).ToList();
        orderQueue.Sort((a,b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
    }
    private void Start()
    {
        stateMachine = new StateMachine();
        
        var customerNameObject = ScriptableObject.CreateInstance<CustomerName>();
        customerNameObject.names = GetComponentInChildren<SkinnedMeshRenderer>().gameObject.CompareTag("Male") ? customerMaleNames.names : customerFemaleNames.names;
        
        orderState = new NpcOrderState(this, animator, agent, player, changeStateManager, orderQueue, menuData, canvas, customerNameObject);
        waitListState = new NpcWaitListState(this, animator, agent, changeStateManager, orderQueue);
        sitState = new NpcSitState(this, animator, agent, changeStateManager, customerMaleNames);
        eatState = new NpcEatState(this, animator, agent, changeStateManager, eatDuration);
        exitState = new NpcExitState(this, animator, agent, changeStateManager);
        
        At(orderState, sitState, new FuncPredicate(() => changeStateManager.OrderTaken()));
        At(orderState, waitListState, new FuncPredicate(() => changeStateManager.LineFull() && !changeStateManager.HasOrder()));
        At(waitListState, orderState, new FuncPredicate(() => !changeStateManager.LineFull()));
        At(sitState, eatState, new FuncPredicate(() => changeStateManager.HasOrderServed()));
        At(eatState, exitState, new FuncPredicate(() => changeStateManager.FinishedEating()));
        At(exitState, waitListState, new FuncPredicate(() => changeStateManager.IsReleasedFromPool()));
        
        
        if (hasPendingBind && data != null)
        {
            RestoreSavedState();
        }
        else
        {
            stateMachine.SetState(waitListState);
        }
    }
    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    private void Update()
    {
        if (data != null)
        {
            data.position = transform.position;
            data.rotation = transform.rotation;
            data.currentState = GetCurrentStateType();
        }
        
        stateMachine.Update();
        
        changeStateManager.PlayerInRange = isPlayerInRange;
        if (!isPlayerInRange || !IsPlayerLookingAtMe()) return;
        
        if (InputManager.Instance.Interact())
        {
            changeStateManager.IsOrderTaken = true;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public override void Bind(AIEntityData data)
    {
        this.data = data;
        this.data.Id = Id;
        
        transform.position = data.position;
        transform.rotation = data.rotation;
        
        hasPendingBind = true;
    }
    private CustomerStateType GetCurrentStateType()
    {
        var current = stateMachine.GetCurrentState();

        if (current is NpcOrderState) return CustomerStateType.Order;
        if (current is NpcWaitListState) return CustomerStateType.WaitList;
        if (current is NpcSitState) return CustomerStateType.Sit;
        if (current is NpcEatState) return CustomerStateType.Eat;
        if (current is NpcExitState) return CustomerStateType.Exit;

        return CustomerStateType.WaitList;
    }
    private void RestoreSavedState()
    {
        switch (data.currentState)
        {
            case CustomerStateType.Order:
                stateMachine.SetState(orderState);
                break;
            case CustomerStateType.Sit:
                stateMachine.SetState(sitState);
                break;
            case CustomerStateType.Eat:
                stateMachine.SetState(eatState);
                break;
            case CustomerStateType.Exit:
                stateMachine.SetState(exitState);
                break;
            default:
                stateMachine.SetState(waitListState);
                break;
        }

        hasPendingBind = false;
    }
}