using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CatchGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject previewPanel;
    public GameObject gamePanel;

    public Image ingredient1Image;
    public Image ingredient2Image;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Slots")]
    public Image[] slotImages;
    public Button[] slotButtons;

    [Header("Gameplay")]
    public float previewTime = 2f;
    public float gameTime = 10f;

    public List<IngredientsItems> allIngredients;

    private float timer;
    private bool isPlaying = false;

    private int score = 0;

    private FoodItem currentOrder;
    private BarMinigameManager manager;

    
    private IngredientsItems currentCorrect;

    void Start()
    {
        manager = FindObjectOfType<BarMinigameManager>();
    }

    public void SetOrder(FoodItem order)
    {
        currentOrder = order;

       
        if (currentOrder.ingredients.Count >= 2)
        {
            ingredient1Image.sprite = currentOrder.ingredients[0].icon;
            ingredient2Image.sprite = currentOrder.ingredients[1].icon;
        }

        score = 0;
        UpdateScore();

        previewPanel.SetActive(true);
        gamePanel.SetActive(false);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(previewTime);

        previewPanel.SetActive(false);
        gamePanel.SetActive(true);

        timer = gameTime;
        isPlaying = true;

        GenerateRound();
    }

    void Update()
    {
        if (!isPlaying) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer);

        if (timer <= 0f)
        {
            isPlaying = false;
            manager.NextStep();
        }
    }

    
    void GenerateRound()
    {
        List<IngredientsItems> pool = new List<IngredientsItems>();

        
        currentCorrect = currentOrder.ingredients[Random.Range(0, 2)];
        pool.Add(currentCorrect);

        
        while (pool.Count < slotImages.Length)
        {
            var rand = allIngredients[Random.Range(0, allIngredients.Count)];

           
            if (currentOrder.ingredients.Contains(rand)) continue;

            if (!pool.Contains(rand))
                pool.Add(rand);
        }

        
        for (int i = 0; i < pool.Count; i++)
        {
            int j = Random.Range(i, pool.Count);
            var temp = pool[i];
            pool[i] = pool[j];
            pool[j] = temp;
        }

        
        for (int i = 0; i < slotImages.Length; i++)
        {
            IngredientsItems item = pool[i];

            slotImages[i].sprite = item.icon;
            slotImages[i].color = Color.white;

            int index = i; 

            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => OnSlotClicked(item, index));
        }
    }

    
    void OnSlotClicked(IngredientsItems clicked, int index)
    {
        if (!isPlaying) return;

        if (index < 0 || index >= slotImages.Length) return;

        if (clicked == currentCorrect)
        {
            score++;
            Debug.Log("Correct!");
            slotImages[index].color = Color.green;
        }
        else
        {
            score--;
            Debug.Log("Wrong!");
            slotImages[index].color = Color.red;
        }

        UpdateScore();

        StartCoroutine(NextRoundDelay());
    }

    IEnumerator NextRoundDelay()
    {
        yield return new WaitForSeconds(0.2f);

        GenerateRound();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
