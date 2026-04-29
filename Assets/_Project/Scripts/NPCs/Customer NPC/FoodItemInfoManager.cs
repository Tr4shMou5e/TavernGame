using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
public class FoodItemInfoManager : SerializedMonoBehaviour
{
    private static FoodItemInfoManager instance;
    public static FoodItemInfoManager Instance => instance;
    public Dictionary<GameObject, FoodItem> foodItemDictionary;
    public Dictionary<CustomerOrderKey, List<(InteractableObject, bool)>> customersOrderDictionary;
    [SerializeField] private MenuData menuData;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        foodItemDictionary = new Dictionary<GameObject, FoodItem>();
        customersOrderDictionary = new Dictionary< CustomerOrderKey, List<(InteractableObject, bool)>>();
    }
    /// <summary>
    /// Always return the first food item in the dictionary
    /// </summary>
    /// <returns></returns>
    public FoodItem GetFoodItem()
    {
        if (foodItemDictionary.Count == 0)
            return null;
        return foodItemDictionary.GetValueOrDefault(foodItemDictionary.Keys.FirstOrDefault());
    }

    public FoodItem GetFoodItemFromCustomer()
    {
        return foodItemDictionary.Count == 0 ? null : customersOrderDictionary.Keys.First().FoodItem;
    }
    public GameObject GetCustomer(FoodItem item)
    {
        return foodItemDictionary.Where(x => x.Value == item).Select(x => x.Key).FirstOrDefault();
    }
    public void AddCustomerOrder(List<(InteractableObject, bool)> order, GameObject customer, FoodItem foodItem)
    {
        var key = new CustomerOrderKey(customer, foodItem);
        customersOrderDictionary.Add(key, order);
    }
    public List<CustomerOrderKey> GetCustomerOrderKeys()
    {
        return customersOrderDictionary.Keys.ToList();
    }
    public int CompareTo(FoodItemInfoManager other)
    {
        if (other == null) return 1;
        return foodItemDictionary.Count.CompareTo(other.foodItemDictionary.Count);
    }
}