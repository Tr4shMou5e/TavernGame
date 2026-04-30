using UnityEngine;
using Sirenix.OdinInspector;

public class MenuDataPopulator : MonoBehaviour
{
    [SerializeField] private MenuData menuData;

    [Button("Populate Menu Data")]
    public void PopulateMenuData()
    {
        var foodList = menuData.GetMenuItems();
        
        if (menuData == null || foodList == null)
        {
            Debug.LogError("MenuData or foodItems list is null!");
            return;
        }

        // Pies (4) - Baking
        SetDishCookingType("Sweet Potato Pie", FoodItem.CookingType.Baking);
        SetDishCookingType("Pumpkin Pie", FoodItem.CookingType.Baking);
        SetDishCookingType("Apple Pie", FoodItem.CookingType.Baking);
        SetDishCookingType("Cherry Pie", FoodItem.CookingType.Baking);

        // Pot Dishes - Some may need cutting board first
        SetDishCookingType("Vintage Stew", FoodItem.CookingType.Pot, requiresCuttingBoard: true);
        SetDishCookingType("Curry", FoodItem.CookingType.Pot, requiresCuttingBoard: true);
        SetDishCookingType("Sushi", FoodItem.CookingType.Pot, requiresCuttingBoard: true);

        // No Cooking Required
        SetDishCookingType("Charcuterie Board", FoodItem.CookingType.None);

        // Drinks - No Cooking
        SetDishCookingType("Honey Ale", FoodItem.CookingType.None);
        SetDishCookingType("Spiced Cider", FoodItem.CookingType.None);
        SetDishCookingType("Berry Mead", FoodItem.CookingType.None);
        SetDishCookingType("Sailor's Lemon Brew", FoodItem.CookingType.None);
        SetDishCookingType("Herbal Tonic", FoodItem.CookingType.None);

        // NOW setup all processesRequired based on cooking types
        SetupAllProcessesRequired();

        Debug.Log("Menu data populated successfully!");
    }

    private void SetDishCookingType(string dishName, FoodItem.CookingType cookingType, bool requiresCuttingBoard = false)
    {
        var foodList = menuData.GetMenuItems();
        
        foreach (var item in foodList)
        {
            if (item.dishName == dishName)
            {
                item.cookingType = cookingType;
                item.requiresCuttingBoard = requiresCuttingBoard;
                Debug.Log($"Set {dishName} to {cookingType}" + (requiresCuttingBoard ? " (requires cutting board)" : ""));
                return;
            }
        }
        Debug.LogWarning($"Dish '{dishName}' not found in MenuData!");
    }

    /// <summary>
    /// Calls SetupProcessesRequired() on all food items.
    /// This populates processesRequired based on their cookingType.
    /// </summary>
    private void SetupAllProcessesRequired()
    {
        var foodList = menuData.GetMenuItems();
        
        foreach (var item in foodList)
        {
            item.SetupProcessesRequired();
        }
        
        Debug.Log("All food items processes setup complete!");
    }
}