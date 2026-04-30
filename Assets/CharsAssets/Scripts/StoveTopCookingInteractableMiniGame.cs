using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class StoveTopCookingInteractableMiniGame : InteractableObject
{
    [SerializeField] private TextMeshProUGUI dishNameText;
    [SerializeField] private Canvas gameplayCanvas;
    [SerializeField] private Canvas resultsCanvas;
    [SerializeField] private Button closeResultsButton;
    
    // Heat Control UI
    [SerializeField] private Slider heatSlider;
    [SerializeField] private TextMeshProUGUI currentTempText;
    [SerializeField] private TextMeshProUGUI targetTempText;
    [SerializeField] private Image heatFillImage;
    [SerializeField] private GameObject heatControlPhaseUI;
    
    // Challenge Phase UI (Pan or Pot)
    [SerializeField] private GameObject panChallengeUI;
    [SerializeField] private GameObject potChallengeUI;
    
    // Results UI
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private List<GameObject> starMeters;
    
    private FoodItem currentFoodItem;
    private FoodItem.CookingType currentCookingType;
    private CookingPhase currentPhase = CookingPhase.HeatControl;
    private float heatControlStartTime;
    private float currentTemperature;
    private float targetTemperature;
    private int totalScore;
    private int heatControlScore;
    private int challengeScore;
    
    private PanFryingChallenge panChallenge;
    private PotCookingChallenge potChallenge;
    
    // Hardcoded recipe data based on cooking type
    private Dictionary<FoodItem.CookingType, CookingRequirements> recipeTemplates;
    
    private enum CookingPhase { HeatControl, Challenge, Complete }
    
    // Scoring thresholds
    private const int STAR_ONE_MIN = 0;
    private const int STAR_TWO_MIN = 51;
    private const int STAR_THREE_MIN = 76;

    public override void Awake()
    {
        base.Awake();
        InitializeRecipeTemplates();
    }

    private void InitializeRecipeTemplates()
    {
        recipeTemplates = new Dictionary<FoodItem.CookingType, CookingRequirements>
        {
            {
                FoodItem.CookingType.Pan,
                new CookingRequirements
                {
                    cookingType = CookingType.Pan,
                    targetTemperature = 375f,
                    flipWindowTime = 7f,
                    cookDuration = 18f,
                    doneLevelTarget = DoneLevel.Medium,
                    ingredientTimings = new List<IngredientAddTiming>()
                }
            },
            {
                FoodItem.CookingType.Pot,
                new CookingRequirements
                {
                    cookingType = CookingType.Pot,
                    targetTemperature = 325f,
                    flipWindowTime = 0f,
                    cookDuration = 45f,
                    doneLevelTarget = DoneLevel.Medium,
                    ingredientTimings = new List<IngredientAddTiming>
                    {
                        new IngredientAddTiming { ingredientName = "Base", timeToAdd = 0f },
                        new IngredientAddTiming { ingredientName = "Seasoning", timeToAdd = 15f },
                        new IngredientAddTiming { ingredientName = "Finishing", timeToAdd = 30f }
                    }
                }
            }
        };
    }
    public override void Interact()
{
    if (miniGameRunning)
        return;

    base.Interact();
    StartStoveMinigame();
}
    private void Update()
    {
        
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

    public void StartStoveMinigame()
{
    
    // Start the minigame UI
    miniGameParentGameObject.SetActive(true);
    playerScript.enabled = false;
    miniGameRunning = true;
    SetupCursor(true, CursorLockMode.None, withCustomCursor);
    
    var inputAxisController = FindFirstObjectByType<CinemachineInputAxisController>();
    if (inputAxisController is not null)
    {
        inputAxisController.enabled = false;
    }

    // Try to get current order, otherwise use test dish
    currentFoodItem = orders?.GetFoodItemFromCustomer();
    if (currentFoodItem == null)
    {
        // Create a test food item
        currentFoodItem = new FoodItem 
        { 
            dishName = "Test Dish",
            cookingType = FoodItem.CookingType.Pan
        };
        Debug.Log("No active order - using test dish");
    }

    currentCookingType = currentFoodItem.cookingType;
    if (currentCookingType == FoodItem.CookingType.None)
    {
        currentCookingType = FoodItem.CookingType.Pan; // Default to pan
    }

    StartCooking();
}

    private bool CheckOrderForProcessing()
    {
        if (orders == null)
        {
            Debug.LogError("Orders manager not initialized!");
            return false;
        }

        var currentOrder = orders.GetFoodItemFromCustomer();
        if (currentOrder == null)
            return false;

        if (currentOrder.processesRequired == null || currentOrder.processesRequired.Count == 0)
            return false;

        var myType = GetType();

        foreach (var process in currentOrder.processesRequired)
        {
            if (process == null)
                continue;

            var processComponent = process.GetComponent<InteractableObject>();
            if (processComponent == null)
                continue;

            if (processComponent.GetType() == myType)
            {
                return true;
            }
        }
        return false;
    }

    private (CustomerOrderKey, System.Type) GetCurrentOrderKey()
    {
        var myType = GetType();
        var item = orders.GetFoodItemFromCustomer();
        if (item == null) return (default, myType);

        var customer = orders.GetCustomer(item);
        return (new CustomerOrderKey(customer, item), myType);
    }

    private void StartCooking()
    {
        totalScore = 0;
        heatControlScore = 0;
        challengeScore = 0;
        currentPhase = CookingPhase.HeatControl;
        
        // Get recipe template based on cooking type
        CookingRequirements recipe = recipeTemplates[currentCookingType];
        targetTemperature = recipe.targetTemperature;
        currentTemperature = 50f; // Start at room temp

        // Setup UI
        dishNameText.text = $"Cooking: {currentFoodItem.dishName}";
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
        
        Debug.Log($"Cooking started: {currentFoodItem.dishName} ({currentCookingType}). Press A/D to adjust heat to {targetTemperature}°F");
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

        CookingRequirements recipe = recipeTemplates[currentCookingType];

        if (currentCookingType == FoodItem.CookingType.Pan)
        {
            panChallengeUI.SetActive(true);
            panChallenge = panChallengeUI.GetComponent<PanFryingChallenge>();
            if (panChallenge != null)
            {
                panChallenge.Initialize(recipe, OnPanChallengeComplete);
                Debug.Log("Pan frying challenge started!");
            }
        }
        else // Pot
        {
            potChallengeUI.SetActive(true);
            potChallenge = potChallengeUI.GetComponent<PotCookingChallenge>();
            if (potChallenge != null)
            {
                potChallenge.Initialize(recipe, OnPotChallengeComplete);
                Debug.Log("Pot cooking challenge started!");
            }
        }
    }

    private void UpdateChallenge()
    {
        if (currentCookingType == FoodItem.CookingType.Pan && panChallenge != null)
        {
            panChallenge.UpdateChallenge();
        }
        else if (currentCookingType == FoodItem.CookingType.Pot && potChallenge != null)
        {
            potChallenge.UpdateChallenge();
        }
    }

    private void OnPanChallengeComplete(int flipScore, int donenessScore)
    {
        challengeScore = flipScore + donenessScore;
        CompleteCooking();
    }

    private void OnPotChallengeComplete(int score)
{
    challengeScore = score;
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
        
        // Play Win Sound
        SoundManager.PlaySound(SoundType.WinSound);
        ToggleHasWon(scorePercentage >= 50); // Win if 50 or higher
        
        Debug.Log($"Final Score: {scorePercentage}/100 - {GetStarRating(scorePercentage)} Stars");
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
        EndMiniGame();
        resultsCanvas.gameObject.SetActive(false);
        miniGameParentGameObject.SetActive(false);
        totalScore = 0;
    }

    private void OnEnable()
    {
        closeResultsButton?.onClick.AddListener(CloseResults);
    }

    private void OnDisable()
    {
        closeResultsButton?.onClick.RemoveListener(CloseResults);
    }
}