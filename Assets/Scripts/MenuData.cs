using UnityEngine;

[System.Serializable]
public class FoodItem
{
    public string dishName;
    public Sprite dishImage;
    public int price;
}

[System.Serializable]
public class CustomerOrderKey
{
    public FoodItem FoodItem { get; set; }

    public CustomerOrderKey(Transform customer, FoodItem foodItem)
    {
        FoodItem = foodItem;
    }
}

public class MenuData : ScriptableObject
{
    [SerializeField] public FoodItem[] menuItems;

    public FoodItem SelectRandomMenuItem()
    {
        if (menuItems == null || menuItems.Length == 0)
            return null;
        return menuItems[Random.Range(0, menuItems.Length)];
    }
}
