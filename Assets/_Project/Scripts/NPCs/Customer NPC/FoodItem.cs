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
    [FormerlySerializedAs("processes")] [Tooltip("This is the list of what the food needs to be processed to be ready to eat.")]
    public List<GameObject> processesRequired;
    public enum CookingType
    {
        Pot,
        Baking
    }
    public bool RequiresCuttingBoard;
    public enum ProcessType
    {
        Catch,
        Fill,
        Shake
    }
    public CookingType cookingType;
    public List<IngredientsItems> ingredients;
    public List<ProcessType> processes;
    
}