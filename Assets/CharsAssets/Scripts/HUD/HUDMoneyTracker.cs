using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager dayManager;

    [Header("MoneyPanel UI")]
    [SerializeField] private TMP_Text fundsText;       
    [SerializeField] private Slider   goalSlider;     
    [SerializeField] private TMP_Text goalProgressText;

    void Update()
    {
        if (dayManager == null) return;

        fundsText.text = $"${dayManager.CurrentFunds:F2}";

        goalSlider.value = dayManager.GoalProgress;   

        goalProgressText.text =
            $"${dayManager.DailyProfit:F0} / ${dayManager.DailyGoal:F0}";
    }
}