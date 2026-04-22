using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FoodItem
{
    public string dishName;
    public Sprite dishImage;
    public string id;
    public float price;
    public int score;
    [Tooltip("This is the list of what the food needs to be processed to be ready to eat.")]
    public enum ProcessType
    {
        Catch,
        Fill,
        Shake
    }

    public List<IngredientsItems> ingredients;
    public List<ProcessType> processes;
}