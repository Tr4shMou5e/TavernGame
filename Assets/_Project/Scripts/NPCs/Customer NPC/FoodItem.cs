using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class FoodItem
{
    public string dishName;
    public GameObject dishPrefab;
    public Sprite dishImage;
    public string id;
    public float price;
    public int score;
    [FormerlySerializedAs("processes")]
    public List<GameObject> processesRequired;
    
    // ADD THIS ENUM AND FIELD:
    public enum CookingType { Pan, Pot, None }
    public CookingType cookingType = CookingType.None;
    
    public enum ProcessType
    {
        Catch,
        Fill,
        Shake
    }

    public List<IngredientsItems> ingredients;
    public List<ProcessType> processes;
}