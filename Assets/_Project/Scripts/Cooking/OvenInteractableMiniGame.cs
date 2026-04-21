using System;
using System.Collections.Generic;
using Cysharp.Text;
using ImprovedTimers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class OvenInteractableMiniGame : InteractableObject
{
    [SerializeField] private GameObject flameControlTarget;
    [SerializeField] private GameObject flamePointer;
    [SerializeField] private SpriteRenderer flameVisual;
    [SerializeField] private Gradient flameControlGradient;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private Canvas endScoreCanvas;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private Button closeButton;
    [SerializeField] private float heatDuration = 10f;
    [SerializeField] private float miniGameTime = 100f;
    [SerializeField] private int score = 100;
    [SerializeField] private List<GameObject> endStarMeters;
    
    private CountdownTimer heatTimer;
    private CountdownTimer miniGameTimer;
    private Renderer flameVisualRenderer;
    private Vector3 flamePointerPosition;
    private int totalScore;
    private const float MaxHeight = 3.75f;
    private const float MaxXPosition = 4.8f;
    private const float MinXPosition = -4.8f;
    
    // Increase difficulty by decreasing the heat duration at certain total score threshold
    private int pointsToIncreaseDifficulty = 1000;
    private int pointsToIncreaseDifficulty2 = 2000;
    private int pointsToIncreaseDifficulty3 = 3000;
    private int pointsToIncreaseDifficulty4 = 4000;
    private int pointsToIncreaseDifficulty5 = 5000;

    private float decrementTime = 1.5f;
    public override void Awake()
    {
        base.Awake();
        flameVisualRenderer = flameVisual.GetComponent<Renderer>();
        flamePointerPosition = flamePointer.transform.position;
        heatTimer = new CountdownTimer(heatDuration);
        miniGameTimer = new CountdownTimer(miniGameTime);
    }
    public override void Interact()
    {
        if (miniGameRunning) return;
        base.Interact();
        RandomSpawner();
    }
    
    private void Update()
    {
        Interact();
        GradualColorChange();
        ChangePointerHeight();
        UpdateTimer();
        CheckTimers();
    }

    private void CheckTimers()
    {
        if(!miniGameRunning) return;
        if (heatTimer.IsFinished)
        {
            ShowEndScoreScreen("You Lose!");
            SoundManager.PlaySound(SoundType.LoseSound);
        }
        if (miniGameTimer.IsFinished)
        {
            ShowEndScoreScreen("You Win!");
            SoundManager.PlaySound(SoundType.WinSound);
        }
    }

    void ShowEndScoreScreen(string message = "")
    {
        miniGameRunning = false;
        SetupCursor(true, CursorLockMode.None, withCustomCursor, true);
        endScoreCanvas.gameObject.SetActive(true);
        winLoseText.SetText(message);
        endScoreText.SetTextFormat("Your Score: {0}", totalScore);
        UpdateStarMeter(endStarMeters, true);
        if (withScoreCanvas)
        {
            scoreCanvas.gameObject.SetActive(false);
        }
        closeButton.onClick.AddListener(() =>
        {
            EndMiniGame();
            endScoreCanvas.gameObject.SetActive(false);
            miniGameParentGameObject.SetActive(false);
            UpdateStarMeter(endStarMeters);
            totalScore = 0;
        });
    }
    private void IncreaseDifficulty()
    {
        if (totalScore == pointsToIncreaseDifficulty && totalScore < pointsToIncreaseDifficulty2 || 
            totalScore == pointsToIncreaseDifficulty2 && totalScore < pointsToIncreaseDifficulty3 || 
            totalScore == pointsToIncreaseDifficulty3 && totalScore < pointsToIncreaseDifficulty4 || 
            totalScore == pointsToIncreaseDifficulty4 && totalScore < pointsToIncreaseDifficulty5 ||
            totalScore == pointsToIncreaseDifficulty5)
        {
            heatDuration -= decrementTime;
            heatTimer = new CountdownTimer(heatDuration);
            StartHeatTimer();
        }
    }

    private void ChangePointerHeight()
    {
        if (!miniGameRunning) return;
        var normalizedTime = Mathf.Clamp01(heatTimer.CurrentTime / heatDuration);
        var currentHeight = Mathf.Lerp(flamePointerPosition.y,  MaxHeight, normalizedTime);
        flamePointer.transform.position = new Vector3(flamePointerPosition.x, currentHeight, flamePointerPosition.z);
    }

    private void MiniGame()
    {
        RandomSpawner();
    }

    private void RandomSpawner()
    {
        if (!miniGameRunning) return;
        IncrementScore();
        var randomXPosition = Random.Range(MinXPosition, MaxXPosition);
        flameControlTarget.transform.position = new Vector3(randomXPosition, flameControlTarget.transform.position.y, flameControlTarget.transform.position.z);
    }

    private void IncrementScore()
    {
        if (!miniGameRunning) return;
        totalScore += score;
        scoreText.SetTextFormat("Score: {0}", totalScore);
        IncreaseDifficulty();
    }

    private void GradualColorChange()
    {
        if (!miniGameRunning) return;
        var percentage = Mathf.Clamp01(1f - (heatTimer.CurrentTime / heatDuration));
        flameVisualRenderer.material.color = flameControlGradient.Evaluate(percentage);
    }
    private void UpdateStarMeter(List<GameObject> starMetersUI, bool active = false)
    {
        if(totalScore >= pointsToIncreaseDifficulty && totalScore < pointsToIncreaseDifficulty2)
        {
            starMetersUI[0].SetActive(active);
        }
        else if(totalScore >= pointsToIncreaseDifficulty2 && totalScore < pointsToIncreaseDifficulty4)
        {
            starMetersUI[1].SetActive(active);
        }
        else if (totalScore >= pointsToIncreaseDifficulty5)
        {
            starMetersUI[2].SetActive(active);
        }
    }
    void StartHeatTimer()
    {
        heatTimer.Reset();
        heatTimer.Start();
    }

    void ResetHeatTimer()
    {
        heatTimer.Reset();
        heatTimer.Start();
    }
    void StartMiniGameTimer()
    {
        miniGameTimer.Reset();
        miniGameTimer.Start();
    }
    private void UpdateTimer()
    {
        timerText.SetTextFormat("Time: {0}", (float) Math.Round(miniGameTimer.CurrentTime, 2));
    }
    void OnEnable()
    {
        OnMiniGameStart += StartHeatTimer;
        OnMiniGameStart += StartMiniGameTimer;
        FlameControlTarget.OnFlameControlTarget += MiniGame;
        FlameControlTarget.OnFlameControlTarget += ResetHeatTimer;
    }
    void OnDisable()
    {
        OnMiniGameStart -= StartHeatTimer;
        OnMiniGameStart -= StartMiniGameTimer;
        FlameControlTarget.OnFlameControlTarget -= MiniGame;
        FlameControlTarget.OnFlameControlTarget -= ResetHeatTimer;
    }
}