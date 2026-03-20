using UnityEngine;

[CreateAssetMenu(menuName = "Tavern/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public string[] ingredients;
    public bool isDrink;
}