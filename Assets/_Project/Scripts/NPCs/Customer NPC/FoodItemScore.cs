using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "FoodItemScore", menuName = "Data/FoodItemScore")]
public class FoodItemScore : ScriptableObject
{
    [SerializeReference]
    [SerializeField] private Dictionary<Sprite, int> foodItemScoreDictionary = new Dictionary<Sprite, int>();
    [SerializeField] private MenuData ingredients;

    private void OnEnable()
    {
        foreach (var ingredient in ingredients.GetMenuItems())
        {
            foodItemScoreDictionary.Add(ingredient.dishImage, ingredient.score);
        }
    }

    public int GetScoreForFoodItem(Sprite foodItem)
    {
        return foodItemScoreDictionary.GetValueOrDefault(foodItem, 0);
    }
}