using UnityEngine;
using TMPro;

public class RecipePageContent : MonoBehaviour
{
    [Header("Recipe Info")]
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private TMP_Text ingredients;

    [Header("Data")]
    [SerializeField] private RecipeData recipe;

    void OnEnable()
    {
        if (recipe == null) return;
        recipeName.text   = recipe.recipeName;
        ingredients.text  = string.Join("\n", recipe.ingredients);
    }
}