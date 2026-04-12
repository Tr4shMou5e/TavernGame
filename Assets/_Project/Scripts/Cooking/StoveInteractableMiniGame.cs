using System;
using System.Collections;
using System.Collections.Generic;
using ImprovedTimers;
using UnityEngine;
using TMPro;
using Cysharp.Text;

[RequireComponent(typeof(BoxCollider))]
public class StoveInteractableMiniGame : InteractableObject
{
    [SerializeField] private float spawnTime = 0.1f;
    [SerializeField] private float maxMiniGameTime = 100f;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FoodItemScore foodItemScore;
    private CountdownTimer timer;
    private MiniGameFoodSpawnerObjectPoolManager miniGameFoodSpawner;
    private float timeSinceLastSpawn;
    private int totalScore;
    
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
        withCustomCursor = true;
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
        
        
        MouseDetection();
        if (!timer.IsFinished) return;
        
        EndMiniGame();
        timer.Reset();
    }

    private void MouseDetection()
    {
        if (miniGameCamera is null) return;
        
        var mouseWorldPos = miniGameCamera.ScreenToWorldPoint(inputManager.GetMousePosition());
        var mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        var hit = Physics2D.OverlapPoint(mousePos2D);
        if(hit is null) return;
        
        if (hit.gameObject.CompareTag("Mini Game Food") && hit.gameObject.TryGetComponent(out SpriteRenderer foodItem))
        {
            var score = foodItemScore.GetScoreForFoodItem(foodItem.sprite);
            totalScore += score;
            scoreText.SetTextFormat("Score: {0}", totalScore);
            
            miniGameFoodSpawner.ReleaseFood(hit.gameObject);
        }
    }
    void StartTimer()
    {
        timer.Start();
    }

    private void OnEnable()
    {
        OnMiniGameStart += StartTimer;
    }
    private void OnDisable()
    {
        OnMiniGameStart -= StartTimer;
    }
}   

