using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoveTopCookingInteractableMiniGame : MonoBehaviour
{
    [SerializeField] private Canvas gameplayCanvas;
    [SerializeField] private Canvas resultsCanvas;
    [SerializeField] private Button closeResultsButton;
    
    // Heat Control UI
    [SerializeField] private Slider heatSlider;
    [SerializeField] private TextMeshProUGUI currentTempText;
    [SerializeField] private TextMeshProUGUI targetTempText;
    [SerializeField] private Image heatFillImage;
    [SerializeField] private GameObject heatControlPhaseUI;
    [SerializeField] private TextMeshProUGUI dishNameText;
    
    // Challenge Phase UI (Pan or Pot)
    [SerializeField] private GameObject panChallengeUI;
    [SerializeField] private GameObject potChallengeUI;
    
    // Results UI
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private List<GameObject> starMeters;
    
    private CookingRequirements currentRecipe;
    private CookingPhase currentPhase = CookingPhase.HeatControl;
    private float heatControlStartTime;
    private float currentTemperature;
    private float targetTemperature;
    private int totalScore;
    private int heatControlScore;
    private int challengeScore;
    
    private PanFryingChallenge panChallenge;
    private PotCookingChallenge potChallenge;
    
    private bool miniGameRunning = false;
    private bool minigameInitialized = false;
    
    private enum CookingPhase { HeatControl, Challenge, Complete }
    
    // Scoring thresholds
    private const int STAR_ONE_MIN = 0;
    private const int STAR_TWO_MIN = 51;
    private const int STAR_THREE_MIN = 76;

    private void Start()
    {
        // Initialize UI canvases to inactive
        gameplayCanvas.gameObject.SetActive(false);
        resultsCanvas.gameObject.SetActive(false);
    }

    private void StartMinigame()
    {
        // Create a test recipe (Steak - Pan frying)
        currentRecipe = new CookingRequirements
        {
            dishName = "Test Steak",
            cookingType = CookingType.Pan,
            targetTemperature = 400f,
            flipWindowTime = 7f,
            cookDuration = 18f,
            doneLevelTarget = DoneLevel.Medium,
            ingredientTimings = new List<IngredientAddTiming>()
        };

        totalScore = 0;
        heatControlScore = 0;
        challengeScore = 0;
        currentPhase = CookingPhase.HeatControl;
        targetTemperature = currentRecipe.targetTemperature;
        currentTemperature = 50f;
        miniGameRunning = true;

        // Setup UI
        dishNameText.text = $"Cooking: {currentRecipe.dishName}";
        targetTempText.text = $"Target: {Mathf.Round(targetTemperature)}°F";
        
        // Setup heat control
        heatSlider.minValue = 0;
        heatSlider.maxValue = 500;
        heatSlider.value = 50;
        heatControlPhaseUI.SetActive(true);
        panChallengeUI.SetActive(false);
        potChallengeUI.SetActive(false);
        gameplayCanvas.gameObject.SetActive(true);
        resultsCanvas.gameObject.SetActive(false);

        heatControlStartTime = Time.time;
        
        Debug.Log("Minigame started! Press A/D to adjust heat to 400°F");
    }

    private void Update()
    {
        // Check for C key to start minigame
        if (!minigameInitialized && Input.GetKeyDown(KeyCode.C))
        {
            StartMinigame();
            minigameInitialized = true;
            Debug.Log("Press A/D to adjust heat to 400°F");
        }

        // Check for Enter key to close results
        if (currentPhase == CookingPhase.Complete && Input.GetKeyDown(KeyCode.Return))
        {
            CloseResults();
        }

        if (!miniGameRunning) return;

        switch (currentPhase)
        {
            case CookingPhase.HeatControl:
                UpdateHeatControl();
                break;
            case CookingPhase.Challenge:
                UpdateChallenge();
                break;
        }
    }

    private void UpdateHeatControl()
    {
        // Update current temp based on keyboard input (A/D or arrow keys)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentTemperature += 50f * Time.deltaTime; // Increase heat
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentTemperature -= 50f * Time.deltaTime; // Decrease heat
        }

        // Clamp temperature between slider min and max
        currentTemperature = Mathf.Clamp(currentTemperature, 0, 500);
        
        // Update slider to match
        heatSlider.value = currentTemperature;
        currentTempText.text = $"{Mathf.Round(currentTemperature)}°F";

        // Update heat fill color (green when in range, orange when off)
        float tolerance = 25f;
        bool inRange = Mathf.Abs(currentTemperature - targetTemperature) <= tolerance;
        heatFillImage.color = inRange ? Color.green : new Color(1f, 0.65f, 0f); // Orange

        // Check if player hit target temp
        if (inRange)
        {
            float timeTaken = Time.time - heatControlStartTime;
            CalculateHeatControlScore(timeTaken);
            TransitionToChallenge();
        }
    }

    private void CalculateHeatControlScore(float timeTaken)
    {
        // Max 25 points if done in <5 seconds, -5 points per 5 seconds after
        heatControlScore = 25;
        if (timeTaken > 5f)
        {
            float extraTime = timeTaken - 5f;
            int penaltySteps = Mathf.FloorToInt(extraTime / 5f) + 1;
            heatControlScore = Mathf.Max(0, 25 - (penaltySteps * 5));
        }

        Debug.Log($"Heat Control Score: {heatControlScore}/25 (Time: {timeTaken:F2}s)");
    }

    private void TransitionToChallenge()
    {
        currentPhase = CookingPhase.Challenge;
        heatControlPhaseUI.SetActive(false);

        if (currentRecipe.cookingType == CookingType.Pan)
        {
            panChallengeUI.SetActive(true);
            panChallenge = panChallengeUI.GetComponent<PanFryingChallenge>();
            if (panChallenge != null)
            {
                panChallenge.Initialize(currentRecipe, OnPanChallengeComplete);
                Debug.Log("Pan frying challenge started!");
            }
        }
        else // Pot
        {
            potChallengeUI.SetActive(true);
            potChallenge = potChallengeUI.GetComponent<PotCookingChallenge>();
            if (potChallenge != null)
            {
                potChallenge.Initialize(currentRecipe, OnPotChallengeComplete);
                Debug.Log("Pot cooking challenge started!");
            }
        }
    }

    private void UpdateChallenge()
    {
        if (currentRecipe.cookingType == CookingType.Pan && panChallenge != null)
        {
            panChallenge.UpdateChallenge();
        }
        else if (currentRecipe.cookingType == CookingType.Pot && potChallenge != null)
        {
            potChallenge.UpdateChallenge();
        }
    }

    private void OnPanChallengeComplete(int flipScore, int donenessScore)
    {
        challengeScore = flipScore + donenessScore;
        CompleteCooking();
    }

    private void OnPotChallengeComplete(int stirScore, int ingredientScore)
    {
        challengeScore = stirScore + ingredientScore;
        CompleteCooking();
    }

    private void CompleteCooking()
    {
        currentPhase = CookingPhase.Complete;
        totalScore = heatControlScore + challengeScore;
        ShowResults();
    }

    private void ShowResults()
    {
        gameplayCanvas.gameObject.SetActive(false);
        resultsCanvas.gameObject.SetActive(true);
        
        // Convert to 0-100 scale
        int scorePercentage = Mathf.Clamp(totalScore, 0, 100);
        finalScoreText.text = $"Score: {scorePercentage}/100\n{GetStarRating(scorePercentage)}★";
        
        UpdateStarDisplay(scorePercentage);
        
        Debug.Log($"Final Score: {scorePercentage}/100");
    }

    private int GetStarRating(int percentage)
    {
        if (percentage >= STAR_THREE_MIN) return 3;
        if (percentage >= STAR_TWO_MIN) return 2;
        if (percentage >= STAR_ONE_MIN) return 1;
        return 0;
    }

    private void UpdateStarDisplay(int percentage)
    {
        int stars = GetStarRating(percentage);
        for (int i = 0; i < starMeters.Count; i++)
        {
            starMeters[i].SetActive(i < stars);
        }
    }

    private void CloseResults()
    {
        resultsCanvas.gameObject.SetActive(false);
        miniGameRunning = false;
        minigameInitialized = false;
        Debug.Log("Results closed! Press C again to restart the minigame.");
    }

    private void OnEnable()
    {
        // Button listener removed - using Enter key instead
    }

    private void OnDisable()
    {
        // Button listener removed - using Enter key instead
    }
}