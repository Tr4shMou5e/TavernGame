using System.Collections.Generic;
using UnityEngine;

public class FoodItemInfoManager : MonoBehaviour
{
    public static FoodItemInfoManager Instance { get; private set; }

    public Dictionary<CustomerOrderKey, List<(InteractableObject, bool)>> customersOrderDictionary 
        = new Dictionary<CustomerOrderKey, List<(InteractableObject, bool)>>();

    private FoodItem currentFoodItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public FoodItem GetFoodItemFromCustomer()
    {
        return currentFoodItem;
    }

    public void SetCurrentFoodItem(FoodItem foodItem)
    {
        currentFoodItem = foodItem;
    }

    public Transform GetCustomer(FoodItem foodItem)
    {
        return null; // Stub
    }
}
