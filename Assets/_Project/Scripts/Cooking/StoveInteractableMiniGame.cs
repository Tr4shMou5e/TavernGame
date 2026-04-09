using System;
using System.Collections;
using System.Collections.Generic;
using ImprovedTimers;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class StoveInteractableMiniGame : InteractableObject
{
    [SerializeField] private float spawnTime = 0.1f;
    [SerializeField] private float maxMiniGameTime = 100f;
    private CountdownTimer timer;
    private MiniGameFoodSpawnerObjectPoolManager miniGameFoodSpawner;
    private float timeSinceLastSpawn;
    
    public static event Action<GameObject> OnFoodSpawned;  
    public override void Awake()
    {
        base.Awake();
        timer = new CountdownTimer(maxMiniGameTime);
        miniGameFoodSpawner = MiniGameFoodSpawnerObjectPoolManager.Instance;
    }
    /// <summary>
    /// Figure out how to make a win condition.
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
        
        if (!timer.IsFinished) return;
        
        EndMiniGame();
        timer.Reset();
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

