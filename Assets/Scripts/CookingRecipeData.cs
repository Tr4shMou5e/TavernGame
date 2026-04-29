using System;
using System.Collections.Generic;
using UnityEngine;

public enum CookingType { Pan, Pot }
public enum DoneLevel { Rare, Medium, WellDone }

[System.Serializable]
public struct IngredientAddTiming
{
    public string ingredientName;
    public float timeToAdd; // When to add during cook (in seconds)
}

[System.Serializable]
public struct CookingRequirements
{
    public string dishName;
    public CookingType cookingType;
    public float targetTemperature; // in Fahrenheit
    public float flipWindowTime; // For pan: when to flip (in seconds from start)
    public float cookDuration; // Total cook time in seconds
    public DoneLevel doneLevelTarget; // For pan: Rare, Medium, WellDone
    public List<IngredientAddTiming> ingredientTimings; // For pot: when to add ingredients
}

public class CookingRecipeData : ScriptableObject
{
    [SerializeField] private List<CookingRequirements> recipes = new List<CookingRequirements>();

    public CookingRequirements GetRecipeForDish(string dishName)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.dishName == dishName)
                return recipe;
        }
        
        Debug.LogWarning($"Recipe not found for dish: {dishName}");
        return default;
    }

    public List<CookingRequirements> GetAllRecipes() => recipes;
}