using System;
using UnityEngine;

[Serializable]
public class FoodItem
{
    public string dishName;
    public Sprite dishImage;
    public string id;
    [Min(0)] public float price;
    void OnEnable()
    {
        id = Guid.NewGuid().ToString();
    }
}