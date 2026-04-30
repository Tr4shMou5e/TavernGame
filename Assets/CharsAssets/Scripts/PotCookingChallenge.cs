using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PotCookingChallenge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stirPromptText;
    [SerializeField] private TextMeshProUGUI stirCountText;
    [SerializeField] private Image stirProgressBar;
    [SerializeField] private Button stirButton;

    private float cookDuration = 45f;
    private float timeRemaining;
    private bool isCooking = false;
    private int totalStirPrompts = 8;
    private int successfulStirs = 0;
    private int currentStirPoints = 0;
    private float nextStirTime;
    private bool waitingForStir = false;
    private float stirWindowStart;

    private Action<int> onChallengeComplete;

    public void Initialize(CookingRequirements recipe, Action<int> callback)
    {
        cookDuration = recipe.cookDuration;
        timeRemaining = cookDuration;
        isCooking = true;
        successfulStirs = 0;
        currentStirPoints = 0;
        onChallengeComplete = callback;

        // Schedule first stir prompt randomly between 3-8 seconds
        nextStirTime = UnityEngine.Random.Range(3f, 8f);
        
        if (stirButton != null)
            stirButton.onClick.AddListener(OnStirButtonPressed);

        UpdateDisplay();
        Debug.Log($"Pot cooking started! {totalStirPrompts} stirs needed in {cookDuration}s");
    }

    public void UpdateChallenge()
    {
        if (!isCooking) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            CompleteCooking();
            return;
        }

        // Check if it's time for next stir prompt
        if (!waitingForStir && (cookDuration - timeRemaining) >= nextStirTime)
        {
            PromptStir();
        }

        // If waiting for stir and time window expires
        if (waitingForStir)
        {
            float timeSincePrompt = Time.time - stirWindowStart;
            if (timeSincePrompt > 5f) // 5+ seconds = bad stir missed
            {
                MissedStir();
            }
        }

        UpdateDisplay();
    }

    private void PromptStir()
    {
        if (successfulStirs >= totalStirPrompts)
        {
            CompleteCooking();
            return;
        }

        waitingForStir = true;
        stirWindowStart = Time.time;
        stirPromptText.text = "STIRnow!";
        stirPromptText.color = Color.yellow;
        Debug.Log($"Stir prompt {successfulStirs + 1}/{totalStirPrompts}!");
    }

    private void OnStirButtonPressed()
    {
        if (!waitingForStir || !isCooking) return;

        float timeSincePrompt = Time.time - stirWindowStart;

        int points = 0;
        string feedback = "";

        if (timeSincePrompt <= 2f)
        {
            // Perfect stir
            points = 13; // 100 points / 8 stirs = 12.5, round to 13
            feedback = "Perfect!";
            stirPromptText.color = Color.green;
        }
        else if (timeSincePrompt <= 5f)
        {
            // OK stir
            points = 6; // Half of perfect
            feedback = "Good!";
            stirPromptText.color = new Color(1f, 0.8f, 0f); // Yellow/orange
        }
        else
        {
            // Bad stir (too late)
            points = 1;
            feedback = "Late...";
            stirPromptText.color = Color.red;
        }

        currentStirPoints += points;
        successfulStirs++;
        stirPromptText.text = feedback;

        waitingForStir = false;

        // Schedule next stir randomly
        if (successfulStirs < totalStirPrompts)
        {
            float currentTime = cookDuration - timeRemaining;
            float delayUntilNext = UnityEngine.Random.Range(3f, 6f);
            nextStirTime = currentTime + delayUntilNext;
        }
        else
        {
            CompleteCooking();
        }

        Debug.Log($"Stir {successfulStirs}/{totalStirPrompts} - {feedback} (+{points} points, Total: {currentStirPoints})");
    }

    private void MissedStir()
    {
        successfulStirs++;
        stirPromptText.text = "Missed!";
        stirPromptText.color = Color.red;
        waitingForStir = false;

        // Schedule next stir
        if (successfulStirs < totalStirPrompts)
        {
            float currentTime = cookDuration - timeRemaining;
            float delayUntilNext = UnityEngine.Random.Range(3f, 6f);
            nextStirTime = currentTime + delayUntilNext;
        }
        else
        {
            CompleteCooking();
        }

        Debug.Log($"Missed stir {successfulStirs}/{totalStirPrompts}");
    }

    private void CompleteCooking()
    {
        isCooking = false;
        waitingForStir = false;
        stirPromptText.text = "Done!";
        stirPromptText.color = Color.green;

        Debug.Log($"Pot cooking complete! Score: {currentStirPoints}/100 ({successfulStirs} stirs)");
        
        // Callback with final score (pot cooking only = stirring score)
        onChallengeComplete?.Invoke(currentStirPoints);
    }

    private void UpdateDisplay()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.RoundToInt(timeRemaining) + "s";

        if (stirCountText != null)
            stirCountText.text = $"Stirs: {successfulStirs}/{totalStirPrompts}";

        if (stirProgressBar != null)
            stirProgressBar.fillAmount = (float)successfulStirs / totalStirPrompts;
    }
}