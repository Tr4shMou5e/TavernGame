using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FillGame : MonoBehaviour
{
    [Header("UI")]
    public Image fillBar;
    public Image pourImage;

    private FoodItem currentOrder;
    private BarMinigameManager manager;

    [Header("Fill Settings")]
    public float fillSpeed = 0.6f;
    private float fillAmount = 0f;

    [Header("Target Zone")]
    public float targetMin = 0.723f;
    public float targetMax = 0.813f;

    private bool isFilling = false;

    
    private bool hitTargetZone = false;

    void Start()
    {
        manager = FindObjectOfType<BarMinigameManager>();
    }

    public void SetOrder(FoodItem order)
    {
        currentOrder = order;

        if (currentOrder == null || currentOrder.ingredients == null)
        {
            Debug.LogError("FillGame: Order is null!");
            return;
        }

        if (pourImage == null)
        {
            Debug.LogError("FillGame: pourImage not assigned!");
            return;
        }

        if (currentOrder.ingredients.Count >= 3)
        {
            pourImage.sprite = currentOrder.ingredients[2].icon;
        }

        ResetFill();
    }

    void Update()
    {
        if (currentOrder == null) return;

       
        if (Input.GetMouseButton(0) && IsMouseOverPour())
        {
            isFilling = true;

            fillAmount += fillSpeed * Time.deltaTime;
            fillAmount = Mathf.Clamp01(fillAmount);

            fillBar.fillAmount = fillAmount;

            
            if (fillAmount >= targetMin && fillAmount <= targetMax)
            {
                hitTargetZone = true;
            }

            
            //if (fillAmount >= targetMin && fillAmount <= targetMax)
            //    fillBar.color = Color.green;
            //else
            //    fillBar.color = Color.blue;
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            if (isFilling)
            {
                CheckResult();
                ResetFill();
                isFilling = false;
            }
        }
    }

    bool IsMouseOverPour()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            pourImage.rectTransform,
            Input.mousePosition,
            null
        );
    }

    void CheckResult()
    {
        float center = (targetMin + targetMax) / 2f;
        float distance = Mathf.Abs(fillAmount - center);

        int score;

        if (hitTargetZone)
        {
            score = 1250;
        }
        else
        {
            float normalized = Mathf.Clamp01(1f - (distance / 0.2f));
            score = Mathf.RoundToInt(normalized * 1000);
        }

        Debug.Log("Fill Score: " + score);

        manager.AddScore(score);
        manager.NextStep();
    }

    void ResetFill()
    {
        fillAmount = 0f;
        hitTargetZone = false;
        fillBar.fillAmount = 0f;
        fillBar.color = Color.blue;
    }
}