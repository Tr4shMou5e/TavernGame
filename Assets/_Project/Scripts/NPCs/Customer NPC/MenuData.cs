using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "Menu", menuName = "MenuData")]
public class MenuData: ScriptableObject
{
    public List<FoodItem> menuItems;
    
    public FoodItem SelectRandomMenuItem()
    {
        var randomIndex = Random.Range(0, menuItems.Count);
        return menuItems[randomIndex];
    }
    
}