using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "FoodItemScore", menuName = "Data/FoodItemScore")]
public class FoodItemScore : ScriptableObject
{
    [SerializeReference]
    [SerializeField] private Dictionary<Sprite, int> foodItemScoreDictionary = new Dictionary<Sprite, int>();
    [SerializeField] private List<MenuData> ingredients;

    private void OnEnable()
    {
        foreach (var v in ingredients.SelectMany(ingredient => ingredient.GetMenuItems()))
        {
            foodItemScoreDictionary.Add(v.dishImage, v.score);
        }
    }

    public int GetScoreForFoodItem(Sprite foodItem)
    {
        return foodItemScoreDictionary.GetValueOrDefault(foodItem, 0);
    }
}