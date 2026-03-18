using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Linq;
public class NpcSitState : NpcBaseState
{
    private NavMeshAgent agent;
    private ChangeStateCustomerManager changeStateManager;
    private FoodItem selectedItem;
    private CustomerName customerName;
    //private GameObject heldItem;
    private FoodItemInfoManager foodItemInfoManager;
    private GameObject[] chairs;
    private ChairData randomChair;
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
        
        var pickedChairs = chairs
            .Select(chair => chair.GetComponent<ChairData>())
            .Where(chairData => chairData != null && !chairData.isOccupied)
            .ToList();
        Debug.Log(pickedChairs.Count);
        if (pickedChairs.Count > 0)
        {
            randomChair = pickedChairs[Random.Range(0, pickedChairs.Count)];
        }
        
        agent.SetDestination(randomChair.position);
        randomChair.isOccupied = true;
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
    public override void OnExit()
    {
        randomChair.isOccupied = false;
    }
}