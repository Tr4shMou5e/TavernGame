using UnityEngine;
using System.Collections.Generic;

public class MenuDataSetup : MonoBehaviour
{
    [SerializeField] private MenuData menuData;
    [SerializeField] private GameObject cuttingBoardGameObject;
    [SerializeField] private GameObject stoveGameObject;
    [SerializeField] private GameObject ovenGameObject;

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
        var recipeRequirements = new Dictionary<string, (FoodItem.CookingType cookingType, List<GameObject> processes)>
        {
            // Pies - require cutting board and oven
            { "Sweet Potato Pie", (FoodItem.CookingType.None, new List<GameObject> { cuttingBoardGameObject, ovenGameObject }) },
            { "Pumpkin Pie", (FoodItem.CookingType.None, new List<GameObject> { cuttingBoardGameObject, ovenGameObject }) },
            { "Apple Pie", (FoodItem.CookingType.None, new List<GameObject> { cuttingBoardGameObject, ovenGameObject }) },
            { "Cherry Pie", (FoodItem.CookingType.None, new List<GameObject> { cuttingBoardGameObject, ovenGameObject }) },

            // Pan-fried dishes - require cutting board and stove (pan)
            { "Roast Turkey", (FoodItem.CookingType.Pan, new List<GameObject> { cuttingBoardGameObject, stoveGameObject }) },
            { "Tavern Fish", (FoodItem.CookingType.Pan, new List<GameObject> { cuttingBoardGameObject, stoveGameObject }) },
            { "Steak", (FoodItem.CookingType.Pan, new List<GameObject> { cuttingBoardGameObject, stoveGameObject }) },

            // Pot dishes - require cutting board and stove (pot)
            { "Vintage Stew", (FoodItem.CookingType.Pot, new List<GameObject> { cuttingBoardGameObject, stoveGameObject }) },
            { "Curry", (FoodItem.CookingType.Pot, new List<GameObject> { cuttingBoardGameObject, stoveGameObject }) },

            // No cooking - just cutting board
            { "Charcuterie Board", (FoodItem.CookingType.None, new List<GameObject> { cuttingBoardGameObject }) },

            // Drinks - no cooking required
            { "Honey Ale", (FoodItem.CookingType.None, new List<GameObject>()) },
            { "Spiced Cider", (FoodItem.CookingType.None, new List<GameObject>()) },
            { "Berry Mead", (FoodItem.CookingType.None, new List<GameObject>()) },
            { "Sailor's Lemon Brew", (FoodItem.CookingType.None, new List<GameObject>()) },
            { "Herbal Tonic", (FoodItem.CookingType.None, new List<GameObject>()) },
        };

        int setupCount = 0;

        foreach (var item in menuItems)
        {
            if (recipeRequirements.TryGetValue(item.dishName, out var requirements))
            {
                item.cookingType = requirements.cookingType;

                if (item.processesRequired == null)
                {
                    item.processesRequired = new List<GameObject>();
                }
                else
                {
                    item.processesRequired.Clear();
                }

                // Add all required processes
                foreach (var process in requirements.processes)
                {
                    if (process != null && !item.processesRequired.Contains(process))
                    {
                        item.processesRequired.Add(process);
                    }
                }

                Debug.Log($"✓ Setup {item.dishName} - Type: {item.cookingType}, Processes: {item.processesRequired.Count}");
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