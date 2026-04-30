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
            // PIES - Pot cooking (baking in a covered pot)
            new CookingRequirements
            {
                dishName = "Sweet Potato Pie",
                cookingType = CookingType.Pot,
                targetTemperature = 350f,
                flipWindowTime = 0f,
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
                cookingType = CookingType.Pot,
                targetTemperature = 350f,
                flipWindowTime = 0f,
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
                cookingType = CookingType.Pot,
                targetTemperature = 350f,
                flipWindowTime = 0f,
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
                cookingType = CookingType.Pot,
                targetTemperature = 350f,
                flipWindowTime = 0f,
                cookDuration = 50f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Cherry Bunches", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Sugar", timeToAdd = 5f },
                    new IngredientAddTiming { ingredientName = "Pie crust", timeToAdd = 10f }
                }
            },

            // MEATS - Pan frying
            new CookingRequirements
            {
                dishName = "Roast Turkey",
                cookingType = CookingType.Pan,
                targetTemperature = 375f,
                flipWindowTime = 8f,
                cookDuration = 20f,
                doneLevelTarget = DoneLevel.WellDone,
                ingredientTimings = new List<IngredientAddTiming>()
            },
            new CookingRequirements
            {
                dishName = "Tavern Fish",
                cookingType = CookingType.Pan,
                targetTemperature = 360f,
                flipWindowTime = 6f,
                cookDuration = 15f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>()
            },
            new CookingRequirements
            {
                dishName = "Steak",
                cookingType = CookingType.Pan,
                targetTemperature = 400f,
                flipWindowTime = 7f,
                cookDuration = 18f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>()
            },

            // STEW - Pot cooking
            new CookingRequirements
            {
                dishName = "Vintage Stew",
                cookingType = CookingType.Pot,
                targetTemperature = 325f,
                flipWindowTime = 0f,
                cookDuration = 60f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Beef", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Carrot", timeToAdd = 15f },
                    new IngredientAddTiming { ingredientName = "Potato", timeToAdd = 30f }
                }
            },

            // BOARD - Pan frying
            new CookingRequirements
            {
                dishName = "Charcuterie Board",
                cookingType = CookingType.Pan,
                targetTemperature = 340f,
                flipWindowTime = 5f,
                cookDuration = 12f,
                doneLevelTarget = DoneLevel.Rare,
                ingredientTimings = new List<IngredientAddTiming>()
            },

            // CURRY - Pot cooking
            new CookingRequirements
            {
                dishName = "Curry",
                cookingType = CookingType.Pot,
                targetTemperature = 340f,
                flipWindowTime = 0f,
                cookDuration = 40f,
                doneLevelTarget = DoneLevel.Medium,
                ingredientTimings = new List<IngredientAddTiming>
                {
                    new IngredientAddTiming { ingredientName = "Beef", timeToAdd = 0f },
                    new IngredientAddTiming { ingredientName = "Curry Spices", timeToAdd = 10f },
                    new IngredientAddTiming { ingredientName = "Rice", timeToAdd = 25f }
                }
            }

            // Drinks don't use stovetop, so they're excluded
        };
    }
}