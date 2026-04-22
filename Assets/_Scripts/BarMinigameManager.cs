using System.Collections.Generic;
using UnityEngine;
using static FoodItem;

public class BarMinigameManager : MonoBehaviour
{
    public MenuData menuData;

    public GameObject catchUI;
    public GameObject fillUI;
    public GameObject shakeUI;

    private FoodItem currentOrder;
    private List<ProcessType> steps;
    private int currentStepIndex = 0;


    void Start()
    {
        StartMinigame();
    }

    void StartMinigame()
    {
        currentOrder = menuData.SelectRandomMenuItem();
        steps = currentOrder.processes;

        Debug.Log("Current Order: " + currentOrder.dishName);

        currentStepIndex = 0;
        RunStep();
    }

    void RunStep()
    {
        
        catchUI.SetActive(false);
        fillUI.SetActive(false);
        shakeUI.SetActive(false);

        if (currentStepIndex >= steps.Count)
        {
            Debug.Log("All steps complete!");
            return;
        }

        ProcessType step = steps[currentStepIndex];

        Debug.Log("Running Step: " + step);

        switch (step)
        {
            case ProcessType.Catch:
                catchUI.SetActive(true);

                var catchGame = catchUI.GetComponent<CatchGame>();
                catchGame.SetOrder(currentOrder);

                break;

            case ProcessType.Fill:
                fillUI.SetActive(true);

                var fillGame = fillUI.GetComponent<FillGame>();
                fillGame.SetOrder(currentOrder);

                break;

            case ProcessType.Shake:
                shakeUI.SetActive(true);

                var shakeGame = shakeUI.GetComponent<ShakeGame>();
                shakeGame.SetOrder(currentOrder);

                break;
        }

        if (catchUI == null || fillUI == null || shakeUI == null)
        {
            Debug.LogError("UI references not set in Manager!");
            return;
        }

        if (currentOrder == null || currentOrder.processes == null)
        {
            Debug.LogError("Order or processes missing!");
            return;
        }
    }

   
    public void NextStep()
    {
        currentStepIndex++;
        RunStep();
    }


}