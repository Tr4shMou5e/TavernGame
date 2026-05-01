using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    [Header("Goal Settings")]
    [SerializeField] private float startingGoal   = 100f;
    [SerializeField] private float goalIncreasePerDay = 25f;

    // ── Public state ──────────────────────────────────────────────────────────
    public float CurrentFunds  { get; private set; } = 0f;
    public float DailyProfit   { get; private set; } = 0f;
    public float DailyGoal     { get; private set; }
    public float GoalProgress  => Mathf.Clamp01(DailyProfit / DailyGoal);

    public UnityEvent onGoalReached;

    private bool _goalReachedThisDay = false;

    void Awake()
    {
        DailyGoal = startingGoal;
    }

    /// <summary>Call this whenever the player earns money (e.g. selling a dish).</summary>
    public void AddProfit(float amount)
    {
        CurrentFunds += amount;
        DailyProfit  += amount;

        if (!_goalReachedThisDay && DailyProfit >= DailyGoal)
        {
            _goalReachedThisDay = true;
            onGoalReached?.Invoke();
        }
    }

    /// <summary>Call this at the start of each new day (hook to WorldClock or a day-end trigger).</summary>
    public void StartNewDay(int dayCount)
    {
        DailyGoal           = startingGoal + (dayCount - 1) * goalIncreasePerDay;
        DailyProfit         = 0f;
        _goalReachedThisDay = false;
    }
}