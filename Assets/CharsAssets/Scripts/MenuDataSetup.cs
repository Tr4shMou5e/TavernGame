using UnityEngine;
using System.Collections.Generic;

public class MenuDataSetup : MonoBehaviour
{
    [SerializeField] private MenuData menuData;

    [ContextMenu("Setup All Recipes")]
    public void SetupTestRecipes()
    {
        if (menuData == null)
        {
            Debug.LogError("MenuData not assigned!");
            return;
        }

        var menuItems = menuData.GetMenuItems();

        // Define recipe requirements
        var recipeRequirements = new Dictionary<string, (FoodItem.CookingType cookingType, bool requiresCuttingBoard)>
        {
            // Pies - Baking
            { "Sweet Potato Pie", (FoodItem.CookingType.Baking, false) },
            { "Pumpkin Pie", (FoodItem.CookingType.Baking, false) },
            { "Apple Pie", (FoodItem.CookingType.Baking, false) },
            { "Cherry Pie", (FoodItem.CookingType.Baking, false) },

            // Other Baking dishes
            { "Roast Turkey", (FoodItem.CookingType.Baking, true) },
            { "Tavern Fish", (FoodItem.CookingType.Baking, true) },
            { "Steak", (FoodItem.CookingType.Baking, true) },
            { "Tenders", (FoodItem.CookingType.Baking, true) },

            // Pot dishes - require cutting board first
            { "Vintage Stew", (FoodItem.CookingType.Pot, true) },
            { "Curry", (FoodItem.CookingType.Pot, true) },
            { "Sushi", (FoodItem.CookingType.Pot, true) },

            // No cooking - just cutting board
            { "Charcuterie Board", (FoodItem.CookingType.None, true) },

            // Drinks - no cooking required
            { "Honey Ale", (FoodItem.CookingType.None, false) },
            { "Spiced Cider", (FoodItem.CookingType.None, false) },
            { "Berry Mead", (FoodItem.CookingType.None, false) },
            { "Sailor's Lemon Brew", (FoodItem.CookingType.None, false) },
            { "Herbal Tonic", (FoodItem.CookingType.None, false) },
        };

        int setupCount = 0;

        foreach (var item in menuItems)
        {
            if (recipeRequirements.TryGetValue(item.dishName, out var requirements))
            {
                item.cookingType = requirements.cookingType;
                item.requiresCuttingBoard = requirements.requiresCuttingBoard;

                // Use the SetupProcessesRequired method to auto-populate
                item.SetupProcessesRequired();

                Debug.Log($"✓ Setup {item.dishName} - Type: {item.cookingType}, Requires Cutting Board: {item.requiresCuttingBoard}, Processes: {item.processesRequired.Count}");
                setupCount++;
            }
        }

        // Mark as dirty so changes are saved
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(menuData);
        UnityEditor.AssetDatabase.SaveAssets();
        #endif
        
        Debug.Log($"\n✓ Recipe setup complete! {setupCount} dishes configured.");
    }
}