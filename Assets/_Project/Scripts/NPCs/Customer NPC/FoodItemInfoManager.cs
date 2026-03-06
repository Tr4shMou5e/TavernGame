using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class FoodItemInfoManager : SerializedMonoBehaviour
{
    private static FoodItemInfoManager instance;
    public static FoodItemInfoManager Instance => instance;
    public Dictionary<string, FoodItem> foodItemDictionary;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        foodItemDictionary = new Dictionary<string, FoodItem>();
    }
}