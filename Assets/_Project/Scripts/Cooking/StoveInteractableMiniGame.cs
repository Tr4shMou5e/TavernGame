using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class StoveInteractableMiniGame : InteractableObject
{
    [SerializeField] private float spawnTime = 0.1f;
    private MiniGameFoodSpawnerObjectPoolManager miniGameFoodSpawner;
    private float timeSinceLastSpawn;
    void Awake()
    {
        miniGameFoodSpawner = MiniGameFoodSpawnerObjectPoolManager.Instance;
        inputManager = InputManager.Instance;
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
        base.Interact();
        if (miniGameFoodSpawner == null) return;
        if (Time.time > timeSinceLastSpawn)
        {
            miniGameFoodSpawner?.GetFood();
            timeSinceLastSpawn = Time.time + spawnTime;
        }
    }

    private void Update()
    {
        Interact();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered");
        StartStopMiniGame();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartStopMiniGame();
        //if(Game is done)
        //StopComponents();
    }
    
}   