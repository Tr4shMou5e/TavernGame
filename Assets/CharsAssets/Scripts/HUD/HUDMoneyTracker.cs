using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager dayManager;

    [Header("MoneyPanel UI")]
    [SerializeField] private TMP_Text fundsText;       // e.g. "$42.50"
    [SerializeField] private Slider   goalSlider;      // profit progress bar
    [SerializeField] private TMP_Text goalProgressText;// e.g. "$42 / $100"

    void Update()
    {
        if (dayManager == null) return;

        fundsText.text = $"${dayManager.CurrentFunds:F2}";

        goalSlider.value = dayManager.GoalProgress;    // slider Min=0, Max=1

        goalProgressText.text =
            $"${dayManager.DailyProfit:F0} / ${dayManager.DailyGoal:F0}";
    }
}