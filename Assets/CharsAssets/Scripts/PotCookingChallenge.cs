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

        // If waiting for stir, check for input
        if (waitingForStir)
        {
            float timeSincePrompt = Time.time - stirWindowStart;
            
            // Check for Space key or button press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnStirButtonPressed();
            }
            
            // If 8+ seconds = missed
            if (timeSincePrompt > 8f)
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
        
        // Start countdown immediately
        StartCountdown();
        
        Debug.Log($"Stir prompt {successfulStirs + 1}/{totalStirPrompts}!");
    }

    private void StartCountdown()
    {
        // Countdown: Ready (0-1s) -> Set (1-2s) -> Stir Now! (2-3s)
        stirPromptText.text = "Ready...";
        stirPromptText.color = Color.red;
        
        // Schedule "Set" after 1 second
        Invoke("ShowSet", 1f);
        // Schedule "Stir Now!" after 2 seconds
        Invoke("ShowStirNow", 2f);
    }

    private void ShowSet()
    {
        if (!waitingForStir) return;
        stirPromptText.text = "Set...";
        stirPromptText.color = Color.yellow;
    }

    private void ShowStirNow()
    {
        if (!waitingForStir) return;
        stirPromptText.text = "Stir Now!";
        stirPromptText.color = Color.green;
    }

    private void OnStirButtonPressed()
    {
        if (!waitingForStir || !isCooking) return;

        CancelInvoke(); // Cancel any pending countdown messages
        
        float timeSincePrompt = Time.time - stirWindowStart;

        int points = 0;
        string feedback = "";

        if (timeSincePrompt > 2f && timeSincePrompt <= 4f)
        {
            // Perfect stir (2-4s)
            points = 13;
            feedback = "Perfect!";
            stirPromptText.color = Color.green;
        }
        else if (timeSincePrompt > 4f && timeSincePrompt <= 6f)
        {
            // Good stir (4-6s)
            points = 6;
            feedback = "Good!";
            stirPromptText.color = new Color(1f, 0.8f, 0f); // Yellow/orange
        }
        else if (timeSincePrompt > 6f && timeSincePrompt <= 8f)
        {
            // Bad stir (6-8s)
            points = 1;
            feedback = "Bad!";
            stirPromptText.color = Color.red;
        }
        else if (timeSincePrompt > 8f)
        {
            // Missed stir (8s+)
            points = 0;
            feedback = "Missed!";
            stirPromptText.color = Color.red;
        }
        else
        {
            // Too early (before 2s)
            points = 0;
            feedback = "Too Early!";
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
        CancelInvoke(); // Cancel any pending countdown messages
        
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
        CancelInvoke(); // Cancel any pending countdown messages
        stirPromptText.text = "Done!";
        stirPromptText.color = Color.green;

        Debug.Log($"Pot cooking complete! Score: {currentStirPoints}/100 ({successfulStirs} stirs)");
        
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