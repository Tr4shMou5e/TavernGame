using UnityEngine;
using TMPro;

public class ClockHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldClock worldClock;

    [Header("DayTimePanel UI")]
    [SerializeField] private TMP_Text dayText;    // "Day" child
    [SerializeField] private TMP_Text clockText;  // "Clock" child

    void Update()
    {
        if (worldClock == null) return;
        dayText.text   = worldClock.DayString;   // "Day 1"
        clockText.text = worldClock.TimeString;  // "10:00 AM"
    }
}