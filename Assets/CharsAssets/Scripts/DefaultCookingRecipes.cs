using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides default cooking configurations for all your tavern dishes.
/// Use these as reference when creating your CookingRecipeData ScriptableObject in the inspector.
/// </summary>
public static class DefaultCookingRecipes
{
    public static List<CookingRequirements> GetDefaultRecipes()
    {
        return new List<CookingRequirements>
        {
            // PIES - Baking (oven baking)
            new CookingRequirements
            {
                dishName = "Sweet Potato Pie",
                cookingType = CookingType.Baking,
                targetTemperature = 350f,
                cookDuration = 45f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Sweet Potato", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Sugar", timeToAdd = 5f },
                    new IngredientAddTiming { ingredientName = "Pie crust", timeToAdd = 10f }
                }
            },
            new CookingRequirements
            {
                dishName = "Pumpkin Pie",
                cookingType = CookingType.Baking,
                targetTemperature = 350f,
                cookDuration = 50f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Pumpkin", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Sugar", timeToAdd = 5f },
                    new IngredientAddTiming { ingredientName = "Pie crust", timeToAdd = 10f }
                }
            },
            new CookingRequirements
            {
                dishName = "Apple Pie",
                cookingType = CookingType.Baking,
                targetTemperature = 350f,
                cookDuration = 45f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Apple", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Sugar", timeToAdd = 5f },
                    new IngredientAddTiming { ingredientName = "Pie crust", timeToAdd = 10f }
                }
            },
            new CookingRequirements
            {
                dishName = "Cherry Pie",
                cookingType = CookingType.Baking,
                targetTemperature = 350f,
                cookDuration = 50f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Cherry Bunches", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Sugar", timeToAdd = 5f },
                    new IngredientAddTiming { ingredientName = "Pie crust", timeToAdd = 10f }
                }
            },

            // STEW - Pot cooking
            new CookingRequirements
            {
                dishName = "Vintage Stew",
                cookingType = CookingType.Pot,
                targetTemperature = 325f,
                cookDuration = 60f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Beef", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Carrot", timeToAdd = 15f },
                    new IngredientAddTiming { ingredientName = "Potato", timeToAdd = 30f }
                }
            },

            // CURRY - Pot cooking
            new CookingRequirements
            {
                dishName = "Curry",
                cookingType = CookingType.Pot,
                targetTemperature = 340f,
                cookDuration = 40f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Beef", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Curry Spices", timeToAdd = 10f },
                    new IngredientAddTiming { ingredientName = "Rice", timeToAdd = 25f }
                }
            }
        };
    }
}