using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShakeGame : MonoBehaviour
{
    [Header("UI")]
    public RectTransform shakeButton;   
    public Button button;

    public RectTransform playArea;      

    public Image fillBar;
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float fillPerClick = 0.1f;
    public float gameTime = 5f;

    private float fillAmount = 0f;
    private float timer;

    private bool isPlaying = false;

    private BarMinigameManager manager;

    public void SetOrder(FoodItem order)
    {
        fillAmount = 0f;
        timer = gameTime;
        isPlaying = true;

        UpdateUI();
        MoveButton();
    }

    void Start()
    {
        manager = FindObjectOfType<BarMinigameManager>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void Update()
    {
        if (!isPlaying) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer);

        if (timer <= 0f)
        {
            isPlaying = false;

            Debug.Log(fillAmount >= 1f ? "Shake Success!" : "Shake Failed!");

            manager.NextStep();
        }
    }

    void OnClick()
    {
        if (!isPlaying) return;

        fillAmount += fillPerClick;
        fillAmount = Mathf.Clamp01(fillAmount);

        UpdateUI();
        MoveButton();

        
        if (fillAmount >= 1f)
        {
            isPlaying = false;
            Debug.Log("Shake Complete!");
            manager.NextStep();
        }
    }


    void MoveButton()
    {
        float padding = 20f;

        float areaWidth = playArea.rect.width;
        float areaHeight = playArea.rect.height;

        float buttonWidth = shakeButton.rect.width;
        float buttonHeight = shakeButton.rect.height;

        float minX = -areaWidth / 2 + buttonWidth / 2 + padding;
        float maxX = areaWidth / 2 - buttonWidth / 2 - padding;

        float minY = -areaHeight / 2 + buttonHeight / 2 + padding;
        float maxY = areaHeight / 2 - buttonHeight / 2 - padding;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        shakeButton.anchoredPosition = new Vector2(x, y);
    }

    void UpdateUI()
    {
        if (fillBar != null)
            fillBar.fillAmount = fillAmount;
    }
}