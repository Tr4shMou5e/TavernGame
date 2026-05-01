using UnityEngine;

public class WorldClock : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Real seconds per in-game hour. 60 = 1 real minute per hour.")]
    [SerializeField] private float secondsPerHour = 60f;

    // ── Public state ──────────────────────────────────────────────────────────
    public int   DayCount  { get; private set; } = 1;
    public int   Hour      { get; private set; } = 10;  // starts at 10 AM
    public int   Minute    { get; private set; } = 0;

    // 0.0 → 1.0 through the 10AM–10PM window (for sun/sky later)
    public float DayProgress => Mathf.Clamp01((Hour - 10f + Minute / 60f) / 12f);

    public string TimeString
    {
        get
        {
            int  h    = Hour % 12 == 0 ? 12 : Hour % 12;
            string ap = Hour < 12 ? "AM" : "PM";
            return $"{h}:{Minute:00} {ap}";
        }
    }

    public string DayString => $"Day {DayCount}";

    // ── Private ───────────────────────────────────────────────────────────────
    private float _secondTimer = 0f;
    private float _totalGameHours = 10f; // start at hour 10

    void Update()
    {
        _secondTimer += Time.deltaTime;

        // Accumulate real seconds into in-game minutes
        float minutesPerSecond = 60f / secondsPerHour; // how many game-minutes pass per real second
        float gameMinutesElapsed = (_secondTimer / secondsPerHour) * 60f;

        _totalGameHours = 10f + (Time.time / secondsPerHour); // hours since 10AM

        // Calculate absolute in-game time
        float absoluteHours = 10f + (Time.time / secondsPerHour);

        // Day count: each 24-hour cycle advances the day
        // Day 1 = 0–24h elapsed, Day 2 = 24–48h, etc.
        DayCount = Mathf.FloorToInt(absoluteHours / 24f) + 1;

        // Hour of day (0–23)
        Hour   = Mathf.FloorToInt(absoluteHours) % 24;
        Minute = Mathf.FloorToInt((absoluteHours % 1f) * 60f);
    }
}