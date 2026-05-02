using UnityEngine;

public static class PriceUpgrader
{
    public static float GetPrice(FoodItem item)
    {
        float basePrice = item.price;

        float multiplier = UpgradeManager.Instance.GetTotalPriceMultiplier();

        return basePrice * multiplier;
    }
}
