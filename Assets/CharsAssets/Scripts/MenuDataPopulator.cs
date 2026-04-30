using UnityEngine;
using System.Collections.Generic;

public class MenuDataPopulator : MonoBehaviour
{
    [SerializeField] private MenuData menuData;
    [SerializeField] private GameObject cuttingBoardGameObject;
    [SerializeField] private GameObject stoveGameObject; // Used for both cooking AND baking

    [ContextMenu("Populate All Dishes")]
    public void PopulateAllDishes()
    {
        if (menuData == null)
        {
            Debug.LogError("MenuData not assigned!");
            return;
        }

        // Define all dishes with their properties
        var dishes = new List<DishData>
        {
            // Existing dishes - update them
            new DishData 
            { 
                dishName = "Sushi",
                cookingType = FoodItem.CookingType.Pot,
                ingredients = new List<string> { "Fish", "Rice" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Tenders",
                cookingType = FoodItem.CookingType.Pan,
                ingredients = new List<string> { "Chicken", "Breadcrumbs", "Oil" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },

            // Pies - use stove for baking
            new DishData 
            { 
                dishName = "Sweet Potato Pie",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Sweet Potato", "Sugar", "Pie crust" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Pumpkin Pie",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Pumpkin", "Sugar", "Pie crust" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Apple Pie",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Apple", "Sugar", "Pie crust" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Cherry Pie",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Cherry Bunches", "Sugar", "Pie crust" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },

            // Pan dishes
            new DishData 
            { 
                dishName = "Roast Turkey",
                cookingType = FoodItem.CookingType.Pan,
                ingredients = new List<string> { "Turkey", "Herbs", "Butter" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Tavern Fish",
                cookingType = FoodItem.CookingType.Pan,
                ingredients = new List<string> { "Fish", "Lemon", "Butter" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Steak",
                cookingType = FoodItem.CookingType.Pan,
                ingredients = new List<string> { "Beef", "Salt", "Butter" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },

            // Pot dishes
            new DishData 
            { 
                dishName = "Vintage Stew",
                cookingType = FoodItem.CookingType.Pot,
                ingredients = new List<string> { "Beef", "Carrot", "Potato" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },
            new DishData 
            { 
                dishName = "Curry",
                cookingType = FoodItem.CookingType.Pot,
                ingredients = new List<string> { "Beef", "Curry Spices", "Rice" },
                processes = new List<GameObject> { cuttingBoardGameObject, stoveGameObject }
            },

            // No cooking
            new DishData 
            { 
                dishName = "Charcuterie Board",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Cheese", "Grapes", "Cured Meat" },
                processes = new List<GameObject> { cuttingBoardGameObject }
            },

            // Drinks - no cooking
            new DishData 
            { 
                dishName = "Honey Ale",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Ale", "Honey", "Ferment" },
                processes = new List<GameObject>()
            },
            new DishData 
            { 
                dishName = "Spiced Cider",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Apple", "Cinnamon", "Tonic" },
                processes = new List<GameObject>()
            },
            new DishData 
            { 
                dishName = "Berry Mead",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Honey", "Berries", "Elixir" },
                processes = new List<GameObject>()
            },
            new DishData 
            { 
                dishName = "Sailor's Lemon Brew",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Lemon", "Sugar", "Tavern Brew" },
                processes = new List<GameObject>()
            },
            new DishData 
            { 
                dishName = "Herbal Tonic",
                cookingType = FoodItem.CookingType.None,
                ingredients = new List<string> { "Herbs", "Honey", "Tonic" },
                processes = new List<GameObject>()
            },
        };

        var menuItems = menuData.GetMenuItems();
        int updatedCount = 0;

        foreach (var dishData in dishes)
        {
            // Try to find existing dish
            FoodItem existingItem = null;
            foreach (var item in menuItems)
            {
                if (item.dishName == dishData.dishName)
                {
                    existingItem = item;
                    break;
                }
            }

            if (existingItem != null)
            {
                // Update existing
                UpdateDish(existingItem, dishData);
                updatedCount++;
            }
            else
            {
                Debug.LogWarning($"Dish '{dishData.dishName}' not found in MenuData.");
            }
        }

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(menuData);
        UnityEditor.AssetDatabase.SaveAssets();
        #endif

        Debug.Log($"\n✓ Menu population complete! Updated: {updatedCount} dishes");
    }

    private void UpdateDish(FoodItem item, DishData data)
    {
        item.cookingType = data.cookingType;

        // Clear and set processes
        if (item.processesRequired == null)
        {
            item.processesRequired = new List<GameObject>();
        }
        item.processesRequired.Clear();
        item.processesRequired.AddRange(data.processes);

        Debug.Log($"✓ Updated {item.dishName} - Type: {item.cookingType}, Processes: {item.processesRequired.Count}");
    }

    private class DishData
    {
        public string dishName;
        public FoodItem.CookingType cookingType;
        public List<string> ingredients;
        public List<GameObject> processes;
    }
}