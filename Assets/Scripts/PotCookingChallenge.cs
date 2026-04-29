using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotCookingChallenge : MonoBehaviour
{
    [SerializeField] private Button stirButton;
    [SerializeField] private TextMeshProUGUI stirCountText;
    [SerializeField] private Transform stirGridParent;
    [SerializeField] private GameObject stirGridCellPrefab;
    [SerializeField] private TextMeshProUGUI nextIngredientText;
    [SerializeField] private TextMeshProUGUI ingredientStatusText;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private CookingRequirements recipe;
    private Action<int, int> onComplete;
    
    private float cookTimer;
    private bool challengeComplete;
    
    private int stirScore;
    private int ingredientScore;
    private int totalStirCount;
    private const int REQUIRED_STIR_COUNT = 8;
    private const int MAX_STIR_SCORE = 25;
    private const int MAX_INGREDIENT_SCORE = 25;
    
    // Ingredient timing
    private int currentIngredientIndex = 0;
    private List<float> ingredientAddTimes = new List<float>();
    private List<bool> ingredientAdded = new List<bool>();

    public void Initialize(CookingRequirements cookingRecipe, Action<int, int> completeCallback)
    {
        recipe = cookingRecipe;
        onComplete = completeCallback;
        
        cookTimer = 0f;
        challengeComplete = false;
        stirScore = 0;
        ingredientScore = 0;
        totalStirCount = 0;
        
        // Setup ingredient timings
        ingredientAddTimes.Clear();
        ingredientAdded.Clear();
        
        if (recipe.ingredientTimings != null && recipe.ingredientTimings.Count > 0)
        {
            foreach (var timing in recipe.ingredientTimings)
            {
                ingredientAddTimes.Add(timing.timeToAdd);
                ingredientAdded.Add(false);
            }
        }
        
        stirButton.onClick.AddListener(OnStirButtonPressed);
        InitializeStirGrid();
        UpdateUI();
    }

    private void InitializeStirGrid()
    {
        // Create grid of stir zones (4x2)
        for (int i = 0; i < REQUIRED_STIR_COUNT; i++)
        {
            var cell = Instantiate(stirGridCellPrefab, stirGridParent);
            cell.GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdateChallenge()
    {
        if (challengeComplete) return;

        cookTimer += Time.deltaTime;

        // Check ingredient timings
        if (recipe.ingredientTimings != null)
        {
            for (int i = 0; i < ingredientAddTimes.Count; i++)
            {
                if (!ingredientAdded[i] && cookTimer >= ingredientAddTimes[i])
                {
                    OnIngredientTimingReached(i);
                }
            }
        }

        // Check if cooking is done
        if (cookTimer >= recipe.cookDuration)
        {
            FinishCooking();
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        timerText.text = $"Time: {cookTimer:F1}s / {recipe.cookDuration:F1}s";
        stirCountText.text = $"Stirs: {totalStirCount}/{REQUIRED_STIR_COUNT}";
        
        // Show next ingredient to add
        if (recipe.ingredientTimings != null && recipe.ingredientTimings.Count > 0)
        {
            if (currentIngredientIndex < recipe.ingredientTimings.Count)
            {
                var nextIngredient = recipe.ingredientTimings[currentIngredientIndex];
                float timeUntilAdd = nextIngredient.timeToAdd - cookTimer;
                
                if (timeUntilAdd > 0)
                {
                    nextIngredientText.text = $"Add {nextIngredient.ingredientName} in {timeUntilAdd:F1}s";
                    nextIngredientText.color = Color.white;
                }
                else
                {
                    nextIngredientText.text = "All ingredients added!";
                    nextIngredientText.color = Color.green;
                }
            }
            else
            {
                nextIngredientText.text = "All ingredients added!";
                nextIngredientText.color = Color.green;
            }
        }
    }

    private void OnIngredientTimingReached(int ingredientIndex)
    {
        ingredientAdded[ingredientIndex] = true;
        currentIngredientIndex++;
        
        var ingredient = recipe.ingredientTimings[ingredientIndex];
        ingredientStatusText.text = $"✓ Added {ingredient.ingredientName}";
        ingredientStatusText.color = Color.green;
        
        // Award points for timely ingredient addition
        ingredientScore += Mathf.RoundToInt(MAX_INGREDIENT_SCORE / (float)recipe.ingredientTimings.Count);
        
        SoundManager.PlaySound(SoundType.ScoreSound);
        Debug.Log($"Added ingredient: {ingredient.ingredientName}");
    }

    private void OnStirButtonPressed()
    {
        if (challengeComplete) return;

        totalStirCount++;
        
        // Update grid visualization
        var gridCells = stirGridParent.GetComponentsInChildren<Image>();
        if (totalStirCount <= gridCells.Length)
        {
            gridCells[totalStirCount - 1].color = Color.green;
        }

        // Calculate stir score
        if (totalStirCount >= REQUIRED_STIR_COUNT)
        {
            stirScore = MAX_STIR_SCORE;
            stirCountText.color = Color.green;
            stirButton.interactable = false;
        }
        else
        {
            stirScore = Mathf.RoundToInt((totalStirCount / (float)REQUIRED_STIR_COUNT) * MAX_STIR_SCORE);
        }

        SoundManager.PlaySound(SoundType.ScoreSound);
        Debug.Log($"Stir count: {totalStirCount}/{REQUIRED_STIR_COUNT}");
    }

    private void FinishCooking()
    {
        challengeComplete = true;
        stirButton.interactable = false;
        
        // Final scoring
        stirScore = Mathf.Min(stirScore, MAX_STIR_SCORE);
        ingredientScore = Mathf.Min(ingredientScore, MAX_INGREDIENT_SCORE);
        
        timerText.text = "Cooking complete!";
        
        Debug.Log($"Stir Score: {stirScore}/25");
        Debug.Log($"Ingredient Score: {ingredientScore}/25");
        Debug.Log($"Total Challenge Score: {stirScore + ingredientScore}/50");
        
        // Call completion callback after a short delay
        Invoke(nameof(CompleteChallenge), 1f);
    }

    private void CompleteChallenge()
    {
        onComplete?.Invoke(stirScore, ingredientScore);
    }

    private void OnDestroy()
    {
        stirButton?.onClick.RemoveListener(OnStirButtonPressed);
    }
}