using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OverviewPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private WorldClock worldClock;

    [Header("Left Page — Day & Profit")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text profitText;
    [SerializeField] private Slider   profitSlider;

    [Header("Right Page — Relationships")]
    [SerializeField] private TMP_Text char1Text;
    [SerializeField] private TMP_Text char2Text;
    [SerializeField] private TMP_Text char3Text;

    public int char1Level = 0;
    public int char2Level = 0;
    public int char3Level = 0;

    void OnEnable() => Refresh();

    void Refresh()
    {
        dayText.text      = worldClock.DayString;
        timeText.text     = worldClock.TimeString;
        profitText.text   = $"${dayManager.DailyProfit:F0}  /  ${dayManager.DailyGoal:F0}";
        profitSlider.value = dayManager.GoalProgress;

        char1Text.text = $"Character 1   {RelationshipLabel(char1Level)}";
        char2Text.text = $"Character 2   {RelationshipLabel(char2Level)}";
        char3Text.text = $"Character 3   {RelationshipLabel(char3Level)}";
    }

    string RelationshipLabel(int level) => level switch
    {
        <= 0  => "Stranger",
        <= 25 => "Acquaintance",
        <= 50 => "Friendly",
        <= 75 => "Trusted",
        _     => "Close Friend"
    };
}