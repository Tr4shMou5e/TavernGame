using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OvenInteractableMiniGame : InteractableObject
{
    [SerializeField] private GameObject flameControlTarget;
    [SerializeField] private float MaxXPosition = 4.8f;
    [SerializeField] private float MinXPosition = -4.8f;
    [SerializeField] private int score = 100;
    private int Totalscore;
    public override void Interact()
    {
        if (miniGameRunning) return;
        base.Interact();
    }
    
    private void Update()
    {
        Interact();
    }

    private void MiniGame()
    {
        RandomSpawner();
    }

    private void RandomSpawner()
    {
        IncrementScore();
        var randomXPosition = Random.Range(MinXPosition, MaxXPosition);
        flameControlTarget.transform.position = new Vector3(randomXPosition, flameControlTarget.transform.position.y, flameControlTarget.transform.position.z);
    }

    private void IncrementScore()
    {
        Totalscore += score;
    }

    void OnEnable()
    {
        FlameControlTarget.OnFlameControlTarget += MiniGame;
    }
    void OnDisable()
    {
        FlameControlTarget.OnFlameControlTarget -= MiniGame;
    }
}