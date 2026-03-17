using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcWanderer : AIEntitiy
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float wanderRadius;
    [SerializeField] ChangeStateWandererManager changeStateManager;
    
    private StateMachine stateMachine;
    private bool isNameSelected;
    private string characterName;

    void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetUpCharacterCostume();
    }

    private void SetUpCharacterCostume()
    {
        var costumes = GetComponentsInChildren<SkinnedMeshRenderer>(true).ToList();
        var currentActiveCostume = GetComponentInChildren<SkinnedMeshRenderer>();
        currentActiveCostume.gameObject.SetActive(false);
        
        var randomCostume = costumes[Random.Range(0, costumes.Count)];
        randomCostume.gameObject.SetActive(true);
    }

    void Start()
    {
        stateMachine = new StateMachine();
        
        SetCharacterName();

        var wanderState = new NpcWandererState(this, animator, agent, wanderRadius, DialogueManager.Instance);
        var shopState = new NpcShopState(this, animator, agent, changeStateManager);
        var dialogueState = new NpcDialogueState(this, animator, agent, player);
        
        At(wanderState, shopState, new FuncPredicate(() => changeStateManager.ExitForShopState()));
        At(shopState, wanderState, new FuncPredicate(() => changeStateManager.ExitForWandererState()));
        At(dialogueState, wanderState, new FuncPredicate(() => changeStateManager.DialogueIsDone()));
        Any(dialogueState, new FuncPredicate(() => changeStateManager.ExitForDialogueState() && !changeStateManager.DialogueIsDone()));
        stateMachine.SetState(wanderState);
    }

    private void SetCharacterName()
    {
        var customerNameObject = ScriptableObject.CreateInstance<CustomerName>();
        customerNameObject.names = GetComponentInChildren<SkinnedMeshRenderer>().gameObject.CompareTag("Male") ? customerMaleNames.names : customerFemaleNames.names;
        
        if (dialogue == null || isNameSelected) return;
        
        characterName = customerNameObject.GetRandomName();
        isNameSelected = true;
    }

    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update()
    {
        stateMachine.Update();
        
        //This statement checks if the player is in range of the NPC, and if so, it will open the dialogue box
        if (!isPlayerInRange || !IsPlayerLookingAtMe())
        {
            changeStateManager.IsDialogueStateStarted = false;
            return;
        }
        
        if (InputManager.Instance.Interact())
        {
            changeStateManager.IsDialogueStateStarted = true;
            dialogue.characterName = characterName;
            DialogueManager.Instance.EnterDialogueMode(dialogue.textAsset, dialogue.characterName, dialogue.characterImage);
        }
    } 
    private void FixedUpdate() => stateMachine.FixedUpdate();
}