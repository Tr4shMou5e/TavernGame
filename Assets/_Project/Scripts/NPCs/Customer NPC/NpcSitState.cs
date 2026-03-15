using UnityEngine;
using UnityEngine.AI;

public class NpcSitState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private FoodItem selectedItem;
    private CustomerName customerName;
    //private GameObject heldItem;
    private FoodItemInfoManager foodItemInfoManager;
    private GameObject[] chairs;
    public NpcSitState(AIEntitiy entity, Animator animator, NavMeshAgent agent, ChangeStateCustomerManager changeStateManager, CustomerName customerName) : base(entity, animator)
    {
        this.agent = agent;
        this.changeStateManager = changeStateManager;
        this.customerName = customerName;
        foodItemInfoManager = FoodItemInfoManager.Instance;
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }

    public override void OnEnter()
    {
        Debug.Log("Sitting entered state");
        
        agent.SetDestination(chairs[0].transform.position);
        if(foodItemInfoManager.foodItemDictionary.TryGetValue(entity.gameObject, out var foodItem))
        {
            selectedItem = foodItem;
        }
    }

    public override void Update()
    {
        Debug.Log("Sitting update state");
        if(foodItemInfoManager.foodItemDictionary.TryGetValue(entity.gameObject, out var foodItem))
        {
            selectedItem = foodItem;
        }
        if (!changeStateManager.PlayerInRange) return;
        Debug.Log("Player in range");
        if (InputManager.Instance.Interact())
        {
            GiveFood();
        }
    }

    private void GiveFood()
    {
        //!Give Order (Implement Later)
        // Check if the player is holding the correct food item the customer ordered
        changeStateManager.OrderServed = true;
    }
}