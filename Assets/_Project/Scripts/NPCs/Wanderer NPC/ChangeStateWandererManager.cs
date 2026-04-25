using System;
using ImprovedTimers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class ChangeStateWandererManager : MonoBehaviour
{
    [SerializeField] private float shopTime = 10f;
    private CountdownTimer shopTimer;
    public CountdownTimer ShopTimer => shopTimer;
    private bool isShoppingDone;
    public bool IsShoppingDone { get => isShoppingDone; set => isShoppingDone = value;}
    private bool isShopWaitTimeDone;
    public bool IsShopWaitTimeDone {get => isShopWaitTimeDone; set => isShopWaitTimeDone = value;}
    private bool isDialogueStateStarted;
    public bool IsDialogueStateStarted { get => isDialogueStateStarted; set => isDialogueStateStarted = value;}
    private void Start()
    {
        shopTime = Random.Range(20f, 35f);
        shopTimer = new CountdownTimer(shopTime);
        shopTimer.Start();
    }

    private void Update()
    {
        if (!IsShopWaitTimeDone) return;
        shopTimer.Reset();
        shopTimer.Start();
        IsShopWaitTimeDone = false;
    }
    
    public bool ExitForShopState()
    {
        return shopTimer.IsFinished;
    }
    public bool ExitForWandererState()
    {
        return isShoppingDone;
    }

    public bool ExitForDialogueState()
    {
        return isDialogueStateStarted;
    }

    public bool DialogueIsDone()
    {
        return DialogueManager.Instance.IsDialogueDone;
    }

    private void OnDestroy()
    {
        shopTimer.Dispose();
    }
}