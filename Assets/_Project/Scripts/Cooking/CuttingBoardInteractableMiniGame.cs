using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ImprovedTimers;
using UnityEngine;
using TMPro;
using Cysharp.Text;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class CuttingBoardInteractableMiniGame : InteractableObject
{
    [SerializeField] private float spawnTime = 0.1f;
    [SerializeField] private float maxMiniGameTime = 100f;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FoodItemScore foodItemScore;
    [SerializeField] private List<GameObject> starMeters;
    [SerializeField] private List<GameObject> endStarMeters;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private Canvas endScoreCanvas;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI timerText;
    private CountdownTimer timer;
    private MiniGameFoodSpawnerObjectPoolManager miniGameFoodSpawner;
    private float timeSinceLastSpawn;
    
    private bool isStarMeterReset = false;
    
    private int totalScore;
    private int starOneScore = 500;
    private int starTwoScore = 1000;
    private int starThreeScore = 2000;
    
    public static event Action<GameObject> OnFoodSpawned;  
    
    public override void Awake()
    {
        base.Awake();
        timer = new CountdownTimer(maxMiniGameTime);
        miniGameFoodSpawner = MiniGameFoodSpawnerObjectPoolManager.Instance;
    }
    /// <summary>
    /// Make Cursor visible and make it as a basket to collect food.
    /// Have to make rotten food for player to lose points.
    /// Score depends on how much food is collected.
    /// Depending on the score, the player will get a ranking.
    /// </summary>
    public override void Interact()
    {
        if (miniGameRunning) return;
        base.Interact();

        if (!isStarMeterReset) return;
        
        ResetStarMeter(starMeters, true);
        ResetStarMeter(endStarMeters);
        isStarMeterReset = true;
    }

    private void ResetStarMeter(List<GameObject> starList, bool active = false)
    {
        foreach (var starMeter in starList)
        {
            starMeter.SetActive(active);
        }
    }

    private void Update()
    {
        Interact();
        MiniGame();
    }
    void MiniGame()
    {
        if (!miniGameRunning) return;
        if (miniGameFoodSpawner is null) return;
        
        if (Time.time > timeSinceLastSpawn)
        { 
            var foodItem = miniGameFoodSpawner?.GetFood(); 
            OnFoodSpawned?.Invoke(foodItem);
            timeSinceLastSpawn = Time.time + spawnTime;
        }
        UpdateTimer();
        UpdateStarMeter(starMeters);
        MouseDetection();
        if (!timer.IsFinished) return;

        TallyScore();
        ShowEndScoreScreen();
        timer.Reset();
    }

    private void TallyScore()
    {
        ToggleHasWon(totalScore >= starOneScore);
    }

    private void ShowEndScoreScreen()
    {
        miniGameRunning = false;
        SetupCursor(true, CursorLockMode.None, withCustomCursor, true);
        endScoreCanvas.gameObject.SetActive(true);
        endScoreText.SetTextFormat("Your Score: {0}", totalScore);
        Debug.Log(endStarMeters.Count);
        UpdateStarMeter(endStarMeters, true);
        
        // Play Win Sound
        SoundManager.PlaySound(SoundType.WinSound);
        
        if (withScoreCanvas)
        {
            scoreCanvas.gameObject.SetActive(false);
        }
    }

    private void CloseEndScoreScreen()
    {
        EndMiniGame();
        endScoreCanvas.gameObject.SetActive(false);
        miniGameParentGameObject.SetActive(false);
        UpdateStarMeter(starMeters, true);
        UpdateStarMeter(endStarMeters);
        isStarMeterReset = false;
        totalScore = 0;
    }

    private void UpdateTimer()
    {
        timerText.SetTextFormat("Time: {0}", (float) Math.Round(timer.CurrentTime, 2));
    }

    private void UpdateStarMeter(List<GameObject> starMetersUI, bool active = false)
    {
        if(totalScore >= starOneScore && totalScore < starTwoScore)
        {
            starMetersUI[0].SetActive(active);
        }
        else if(totalScore >= starTwoScore && totalScore < starThreeScore)
        {
            starMetersUI[1].SetActive(active);
        }
        else if (totalScore >= starThreeScore)
        {
            starMetersUI[2].SetActive(active);
        }
    }

    private void MouseDetection()
    {
        if (miniGameCamera is null) return;
        
        var mouseWorldPos = miniGameCamera.ScreenToWorldPoint(inputManager.GetMousePosition());
        var mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        var hit = Physics2D.CircleCast(mousePos2D, 0.5f, Vector2.zero);
        if(hit.collider is null) return;
        
        if (hit.collider.CompareTag("Mini Game Food") && hit.collider.TryGetComponent(out SpriteRenderer foodItem))
        {
            var score = foodItemScore.GetScoreForFoodItem(foodItem.sprite);
            totalScore += score;
            if(totalScore < 0)
            {
                totalScore = 0;
            }
            
            scoreText.SetTextFormat("Score: {0}", totalScore);
            
            miniGameFoodSpawner.ReleaseFood(hit.collider.gameObject);
            SoundManager.PlaySound(SoundType.ScoreSound, 0.3f);
        }
    }
    void StartTimer()
    {
        timer.Start();
        Debug.Log(timer.CurrentTime);
    }

    private void OnEnable()
    {
        OnMiniGameStart += StartTimer;
        closeButton.onClick.AddListener(CloseEndScoreScreen);
    }
    private void OnDisable()
    {
        OnMiniGameStart -= StartTimer;
        closeButton.onClick.RemoveListener(CloseEndScoreScreen);
    }
}