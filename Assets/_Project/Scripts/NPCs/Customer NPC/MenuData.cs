using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[CreateAssetMenu(fileName = "Menu", menuName = "Data/MenuData")]
public class MenuData: ScriptableObject
{
    [SerializeField] protected List<FoodItem> menuItems;
    void OnEnable()
    {
        foreach (var item in menuItems)
        {
            item.id = item.dishName.GetHashCode().ToString();
        }
    }
    public FoodItem SelectRandomMenuItem()
    {
        var randomIndex = Random.Range(0, menuItems.Count);
        return menuItems[randomIndex];
    }
    
}