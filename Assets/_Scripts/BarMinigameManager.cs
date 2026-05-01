using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FoodItem;
using ImprovedTimers;

public class BarMinigameManager : InteractableObject
{


    public GameObject catchUI;
    public GameObject fillUI;
    public GameObject shakeUI;
    public GameObject beginUI;
    public GameObject resultUI;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public TextMeshProUGUI totalScoreText;
    [SerializeField] float maxTime = 5;

    private FoodItem currentOrder;
    private List<ProcessType> steps;
    private int currentStepIndex = 0;
    private int starOneScore = 1000;
    private int starTwoScore = 2500;
    private int starThreeScore = 4000;
    private int totalScore = 0;
    private CountdownTimer timer;
    



    int GetStars()
    {
        if (totalScore >= starThreeScore) return 3;
        if (totalScore >= starTwoScore) return 2;
        if (totalScore >= starOneScore) return 1;
        return 0;
    }

    public override void Awake()
    {
        base.Awake();
        timer = new CountdownTimer(maxTime);
    }

    private void Update()
    {
        
         if(timer.IsFinished)
        {
            timer.Reset();
            
            miniGameParentGameObject.SetActive(false);
        }
        Interact();
    }


    public override void Interact()
    {
        if(miniGameRunning)
            return;
        base.Interact();
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
    }
    public void BeginGame()
    {
        beginUI.SetActive(false);
        StartMinigame();
    }

    void StartMinigame()
    {
        currentOrder = orders.GetCustomerOrderKeys()[0].FoodItem;
        steps = currentOrder.processes;

        Debug.Log("Current Order: " + currentOrder.dishName);

        currentStepIndex = 0;
        RunStep();
    }

    void RunStep()
    {
        beginUI.SetActive(false);
        catchUI.SetActive(false);
        fillUI.SetActive(false);
        shakeUI.SetActive(false);
        resultUI.SetActive(false);

        if (currentStepIndex >= steps.Count)
        {
            Debug.Log("All steps complete!");
            ShowResults();
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

    void ShowResults()
    {
        
        resultUI.SetActive(true);

        int stars = GetStars();


        Image s1 = star1.GetComponent<Image>();
        Image s2 = star2.GetComponent<Image>();
        Image s3 = star3.GetComponent<Image>();

        Color active = Color.white;
        Color inactive = new Color(0.3f, 0.3f, 0.3f);

        s1.color = stars >= 1 ? active : inactive;
        s2.color = stars >= 2 ? active : inactive;
        s3.color = stars >= 3 ? active : inactive;


        totalScoreText.text = "Total Score: " + totalScore;
        ToggleHasWon(true);
        EndMiniGame();
        
    }

    void OnEnable()
    {
        OnMiniGameEnd+=StartTimer;
    }

    void OnDisable()
    {
        OnMiniGameEnd-=StartTimer;
    }

    private void StartTimer()
    {
        timer.Start();
    }
}