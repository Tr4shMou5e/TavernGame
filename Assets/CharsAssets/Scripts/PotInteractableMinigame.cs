using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class PotInteractableMiniGame : InteractableObject
{
    [SerializeField] private PotCookingChallenge potCookingChallenge;
    [SerializeField] private Camera PotMinigameCam;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stirPromptText;
    [SerializeField] private TextMeshProUGUI stirCountText;
    [SerializeField] private Image stirProgressBar;
    [SerializeField] private Button stirButton;
    
    // Heat Control UI
    [SerializeField] private Slider heatSlider;
    [SerializeField] private TextMeshProUGUI currentTempText;
    [SerializeField] private TextMeshProUGUI targetTempText;
    [SerializeField] private Image heatFillImage;
    [SerializeField] private GameObject heatControlPhaseUI;
    [SerializeField] private GameObject potChallengeUI;

    private enum CookingPhase { HeatControl, Challenge, Complete }
    private CookingPhase currentPhase = CookingPhase.HeatControl;
    
    private float heatControlStartTime;
    private float currentTemperature;
    private float targetTemperature;
    private int challengeScore = 0;

    public override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public override void Interact()
    {
        if (miniGameRunning) return;
        base.Interact();
        
        if (!miniGameRunning) return;

        StartCooking();
    }

    private void Update()
    {
        Interact();
        
        if (!miniGameRunning) return;

        switch (currentPhase)
        {
            case CookingPhase.HeatControl:
                UpdateHeatControl();
                break;
            case CookingPhase.Challenge:
                if (potCookingChallenge != null)
                {
                    potCookingChallenge.UpdateChallenge();
                }
                break;
        }
    }

    private void StartCooking()
    {
        currentPhase = CookingPhase.HeatControl;
        targetTemperature = 325f;
        currentTemperature = 50f;
        challengeScore = 0;
        heatControlStartTime = Time.time;
Debug.Log("StartCooking called!");
    // SWITCH CAMERA
    if (miniGameCamera != null)
    {
        Debug.Log("Switching to mini game camera");
        Camera.main.enabled = false;
        miniGameCamera.enabled = true;
    }
    else
    {
        Debug.Log("miniGameCamera is NULL!");
    }


        // Setup UI
        if (targetTempText != null)
            targetTempText.text = $"Target: {Mathf.Round(targetTemperature)}°F";
        
        if (heatSlider != null)
        {
            heatSlider.minValue = 0;
            heatSlider.maxValue = 500;
            heatSlider.value = 50;
        }
        
        if (heatControlPhaseUI != null) heatControlPhaseUI.SetActive(true);
        if (potChallengeUI != null) potChallengeUI.SetActive(false);

        Debug.Log($"Pot cooking started. Heat to {targetTemperature}°F");
    }

    private void UpdateHeatControl()
    {
        // A/D keys to control heat
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentTemperature += 50f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentTemperature -= 50f * Time.deltaTime;
        }

        currentTemperature = Mathf.Clamp(currentTemperature, 0, 500);
        
        if (heatSlider != null) heatSlider.value = currentTemperature;
        if (currentTempText != null) currentTempText.text = $"{Mathf.Round(currentTemperature)}°F";

        float tolerance = 25f;
        bool inRange = Mathf.Abs(currentTemperature - targetTemperature) <= tolerance;
        
        if (heatFillImage != null)
        {
            heatFillImage.color = inRange ? Color.green : new Color(1f, 0.65f, 0f);
        }

        // Once heated correctly, transition to challenge
        if (inRange)
        {
            TransitionToChallenge();
        }
    }

    private void TransitionToChallenge()
    {
        currentPhase = CookingPhase.Challenge;
        
        if (heatControlPhaseUI != null) heatControlPhaseUI.SetActive(false);
        if (potChallengeUI != null) potChallengeUI.SetActive(true);

        var recipe = new CookingRequirements
        {
            cookingType = CookingType.Pot,
            targetTemperature = 325f,
            flipWindowTime = 0f,
            cookDuration = 45f,
            doneLevelTarget = DoneLevel.Medium,
            ingredientTimings = new System.Collections.Generic.List<IngredientAddTiming>()
        };

        if (potCookingChallenge != null)
        {
            potCookingChallenge.Initialize(recipe, OnPotChallengeComplete);
            Debug.Log("Transitioned to pot stirring challenge!");
        }
    }

    private void OnPotChallengeComplete(int score)
    {
        challengeScore = score;
        Debug.Log($"Pot challenge completed with score: {challengeScore}");
        
        // SWITCH BACK TO MAIN CAMERA
        if (PotMinigameCam != null)
        {
            PotMinigameCam.enabled = false;
            Camera.main.enabled = true;
        }
        
        currentPhase = CookingPhase.Complete;
        miniGameParentGameObject.SetActive(false);
        EndMiniGame();
    }

    void OnEnable()
    {
        OnMiniGameStart += StartCooking;
    }

    void OnDisable()
    {
        OnMiniGameStart -= StartCooking;
    }
}