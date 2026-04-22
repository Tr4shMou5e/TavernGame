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
    [SerializeField] Transform parentTransform; 
    [SerializeField] private float eatDuration = 10f;
    [SerializeField] private float footstepInterval = 0.55f;
    
    private float timer = 0f;
    private StateMachine stateMachine;
    
    private NpcOrderState orderState;
    private NpcWaitListState waitListState;
    private NpcSitState sitState;
    private NpcEatState eatState;
    private NpcExitState exitState;
    
    public static event Action<AIEntitiy> OnOrderTaken;
    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        orderQueue = FindObjectsByType<OrderNode>(FindObjectsSortMode.None).ToList();
        orderQueue.Sort((a,b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        parentTransform = GameObject.Find("Parent Transform").transform;
    }
    private void Start()
    {
        stateMachine = new StateMachine();
        
        var customerNameObject = ScriptableObject.CreateInstance<CustomerName>();
        customerNameObject.names = GetComponentInChildren<SkinnedMeshRenderer>().gameObject.CompareTag("Male") 
            ? customerMaleNames.names 
            : customerFemaleNames.names;
        
        orderState = new NpcOrderState(this, animator, agent, player, changeStateManager, orderQueue, menuData, canvas, customerNameObject);
        waitListState = new NpcWaitListState(this, animator, agent, changeStateManager, orderQueue);
        sitState = new NpcSitState(this, animator, agent, changeStateManager, customerMaleNames, player, canvas, parentTransform);
        eatState = new NpcEatState(this, animator, agent, changeStateManager, eatDuration);
        exitState = new NpcExitState(this, animator, agent, changeStateManager);
        
        At(orderState, sitState, new FuncPredicate(() => changeStateManager.OrderTaken()));
        At(orderState, waitListState, new FuncPredicate(() => changeStateManager.LineFull() && !changeStateManager.HasOrder()));
        At(waitListState, orderState, new FuncPredicate(() => !changeStateManager.LineFull()));
        At(sitState, eatState, new FuncPredicate(() => changeStateManager.HasOrderServed()));
        At(eatState, exitState, new FuncPredicate(() => changeStateManager.FinishedEating()));
        At(exitState, waitListState, new FuncPredicate(() => changeStateManager.IsReleasedFromPool()));
        
        stateMachine.SetState(waitListState);
    }
    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    private void Update()
    {
        if (stateMachine == null) return;
        stateMachine.Update();
        
        if (agent.remainingDistance > 0.1f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                SoundManager.PlaySound(SoundType.Footstep, transform.position);
                timer = footstepInterval; 
            }
        }
        
        changeStateManager.PlayerInRange = isPlayerInRange;
        if (!isPlayerInRange || !IsPlayerLookingAtMe()) return;
        
        if (InputManager.Instance.Interact())
        {
            OnOrderTaken?.Invoke(this);
            changeStateManager.IsOrderTaken = true;
            SoundManager.PlaySound(SoundType.BuySound);
        }
    }

    private void FixedUpdate()
    {
        if (stateMachine == null) return;
        stateMachine.FixedUpdate();
    }
}