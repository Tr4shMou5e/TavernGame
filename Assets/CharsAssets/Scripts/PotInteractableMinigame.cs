using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PotInteractableMiniGame : InteractableObject
{
    [SerializeField] private PotCookingChallenge potCookingChallenge;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stirPromptText;
    [SerializeField] private TextMeshProUGUI stirCountText;
    [SerializeField] private Image stirProgressBar;
    [SerializeField] private Button stirButton;

    private int challengeScore = 0;

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
        base.Interact();
    }

    private void Update()
    {
        if (miniGameRunning && potCookingChallenge != null)
        {
            potCookingChallenge.UpdateChallenge();
        }
    }

    /// <summary>
    /// This is called by the order system when the player interacts with the cauldron.
    /// It needs to know what recipe/cooking requirements are for the current order.
    /// </summary>
    public void StartPotCooking()
    {
        var currentOrder = orders.GetFoodItemFromCustomer();
        if (currentOrder == null)
        {
            Debug.LogError("No current order found!");
            return;
        }

        // Create a CookingRequirements from the FoodItem
        var cookingRequirements = new CookingRequirements
        {
            cookDuration = 45f, // Default, you may want to vary this by dish
            dishName = currentOrder.dishName
        };

        if (potCookingChallenge != null)
        {
            potCookingChallenge.Initialize(cookingRequirements, OnPotChallengeComplete);
        }
        else
        {
            Debug.LogError("PotCookingChallenge not assigned!");
        }
    }

    private void OnPotChallengeComplete(int score)
    {
        challengeScore = score;
        Debug.Log($"Pot challenge completed with score: {challengeScore}");
        
        // End the minigame and return to normal gameplay
        miniGameRunning = false;
        miniGameParentGameObject.SetActive(false);
        StopComponents();
    }

    private void StopComponents()
    {
        EndMiniGame();
    }
}