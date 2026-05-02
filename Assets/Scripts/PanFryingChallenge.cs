using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanFryingChallenge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI flipCountdownText;
    [SerializeField] private TextMeshProUGUI flipStatusText;
    [SerializeField] private Button flipButton;
    [SerializeField] private Image flipTimerBar;
    [SerializeField] private Image cookLevelIndicator;
    [SerializeField] private TextMeshProUGUI cookLevelText;
    [SerializeField] private TextMeshProUGUI donenessStatusText;
    
    private CookingRequirements recipe;
    private Action<int, int> onComplete;
    
    private float cookTimer;
    private float flipWindowStart;
    private float flipWindowEnd;
    private bool hasFlipped;
    private bool challengeComplete;
    
    private int flipScore;
    private int donenessScore;
    
    private const float FLIP_WINDOW_DURATION = 3f; // 3 second window to flip
    private const int MAX_FLIP_SCORE = 25;
    private const int MAX_DONENESS_SCORE = 25;

    public void Initialize(CookingRequirements cookingRecipe, Action<int, int> completeCallback)
    {
        recipe = cookingRecipe;
        onComplete = completeCallback;
        
        cookTimer = 0f;
        hasFlipped = false;
        challengeComplete = false;
        flipScore = 0;
        donenessScore = 0;
        
        // Flip window: target time +/- 1.5 seconds
        flipWindowStart = recipe.flipWindowTime - 1.5f;
        flipWindowEnd = recipe.flipWindowTime + 1.5f;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update flip countdown
        float timeUntilFlip = flipWindowStart - cookTimer;
        if (timeUntilFlip > 0)
        {
            flipCountdownText.text = $"Flip in: {timeUntilFlip:F1}s";
            flipStatusText.text = "Get ready...";
            flipButton.interactable = false;
        }
        else if (!hasFlipped && cookTimer < flipWindowEnd)
        {
            flipCountdownText.text = "FLIP NOW!";
            flipStatusText.text = "⚠ Flip window open!";
            flipButton.interactable = true;
            flipTimerBar.color = Color.yellow;
        }
        else if (!hasFlipped && cookTimer >= flipWindowEnd)
        {
            flipCountdownText.text = "Missed flip!";
            flipStatusText.text = "❌ Flip window closed";
            flipButton.interactable = false;
        }

        // Update cook level indicator
        float cookProgress = cookTimer / recipe.cookDuration;
        UpdateCookLevelDisplay(cookProgress);

        // Update flip timer bar
        if (!hasFlipped && cookTimer >= flipWindowStart && cookTimer < flipWindowEnd)
        {
            float windowProgress = (cookTimer - flipWindowStart) / FLIP_WINDOW_DURATION;
            flipTimerBar.fillAmount = windowProgress;
        }
    }

    private void UpdateCookLevelDisplay(float cookProgress)
    {
        // Show visual representation of doneness
        if (cookProgress < 0.33f)
        {
            cookLevelText.text = "Rare";
            cookLevelIndicator.color = new Color(1f, 0.7f, 0.7f); // Light red
        }
        else if (cookProgress < 0.66f)
        {
            cookLevelText.text = "Medium";
            cookLevelIndicator.color = new Color(1f, 0.8f, 0.6f); // Orange
        }
        else
        {
            cookLevelText.text = "Well-Done";
            cookLevelIndicator.color = new Color(0.6f, 0.4f, 0.2f); // Brown
        }

        // Determine if current doneness matches target
        DoneLevel currentLevel = GetCurrentDoneLevel(cookProgress);
        if (currentLevel == recipe.doneLevelTarget)
        {
            donenessStatusText.text = "✓ Perfect doneness";
            donenessStatusText.color = Color.green;
        }
        else
        {
            donenessStatusText.text = $"Target: {recipe.doneLevelTarget}";
            donenessStatusText.color = Color.red;
        }
    }

    private DoneLevel GetCurrentDoneLevel(float cookProgress)
    {
        if (cookProgress < 0.33f) return DoneLevel.Rare;
        if (cookProgress < 0.66f) return DoneLevel.Medium;
        return DoneLevel.WellDone;
    }

    public void UpdateChallenge()
    {
        if (challengeComplete) return;

        cookTimer += Time.deltaTime;

        // Check for spacebar flip input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnFlipButtonPressed();
        }

        // Check if cooking is done
        if (cookTimer >= recipe.cookDuration)
        {
            FinishCooking();
            return;
        }

        UpdateUI();
    }

    private void OnFlipButtonPressed()
    {
        if (hasFlipped || challengeComplete) return;

        // Check if flip was in the correct window
        if (cookTimer >= flipWindowStart && cookTimer <= flipWindowEnd)
        {
            // Perfect flip timing
            flipScore = MAX_FLIP_SCORE;
            flipStatusText.text = "✓ Perfect flip!";
            flipStatusText.color = Color.green;
            flipButton.interactable = false;
            hasFlipped = true;
            SoundManager.PlaySound(SoundType.ScoreSound);
            Debug.Log($"Flip Score: {flipScore}/25");
        }
        else
        {
            // Missed the window
            flipScore = 0;
            flipStatusText.text = "❌ Bad timing!";
            flipStatusText.color = Color.red;
            flipButton.interactable = false;
            hasFlipped = true;
            Debug.Log("Flip Score: 0/25 (Missed window)");
        }
    }

    private void FinishCooking()
    {
        challengeComplete = true;
        
        // Determine final doneness score based on how close they got to target
        float finalCookProgress = cookTimer / recipe.cookDuration;
        DoneLevel finalLevel = GetCurrentDoneLevel(finalCookProgress);
        
        if (finalLevel == recipe.doneLevelTarget)
        {
            donenessScore = MAX_DONENESS_SCORE; // Perfect doneness
        }
        else if ((int)finalLevel == (int)recipe.doneLevelTarget + 1 || 
                 (int)finalLevel == (int)recipe.doneLevelTarget - 1)
        {
            donenessScore = 10; // Off by one level
        }
        else
        {
            donenessScore = 0; // Way off
        }

        flipButton.interactable = false;
        flipCountdownText.text = "Cooking complete!";
        
        Debug.Log($"Doneness Score: {donenessScore}/25 (Target: {recipe.doneLevelTarget}, Actual: {finalLevel})");
        Debug.Log($"Total Challenge Score: {flipScore + donenessScore}/50");
        
        // Call completion callback after a short delay so UI updates are visible
        Invoke(nameof(CompleteChallenge), 1f);
    }

    private void CompleteChallenge()
    {
        onComplete?.Invoke(flipScore, donenessScore);
    }
}