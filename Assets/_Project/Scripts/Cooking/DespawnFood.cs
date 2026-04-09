using System;
using UnityEngine;
using ImprovedTimers;
public class DespawnFood : MonoBehaviour
{
    MiniGameFoodSpawnerObjectPoolManager miniGameFoodSpawner;
    private CountdownTimer timer;
    private readonly float timeToDespawn = 10f;
    private void Awake()
    {
        timer = new CountdownTimer(timeToDespawn);
        miniGameFoodSpawner = MiniGameFoodSpawnerObjectPoolManager.Instance;
    }

    private void StartDespawn(GameObject foodItem)
    {
        if (foodItem != gameObject) return;
        
        timer.Reset();
        timer.Start();
    }

    private void Update()
    {
        if (!timer.IsFinished) return;
        
        miniGameFoodSpawner.ReleaseFood(gameObject);
    }
    private void OnEnable()
    {
        StoveInteractableMiniGame.OnFoodSpawned += StartDespawn;
    }

    private void OnDisable()
    {
        StoveInteractableMiniGame.OnFoodSpawned -= StartDespawn;
    }
}