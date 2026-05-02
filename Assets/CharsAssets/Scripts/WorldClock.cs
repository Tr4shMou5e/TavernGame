using UnityEngine;

public class WorldClock : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Real seconds per in-game hour. 60 = 1 real minute per hour.")]
    [SerializeField] private float secondsPerHour = 60f;

    public int   DayCount  { get; private set; } = 1;
    public int   Hour      { get; private set; } = 10;  // starts at 10 AM
    public int   Minute    { get; private set; } = 0;

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

    private float _secondTimer = 0f;
    private float _totalGameHours = 10f;

    void Update()
    {
        _secondTimer += Time.deltaTime;

        float minutesPerSecond = 60f / secondsPerHour; 
        float gameMinutesElapsed = (_secondTimer / secondsPerHour) * 60f;

        _totalGameHours = 10f + (Time.time / secondsPerHour); 

        float absoluteHours = 10f + (Time.time / secondsPerHour);

        DayCount = Mathf.FloorToInt(absoluteHours / 24f) + 1;

        Hour   = Mathf.FloorToInt(absoluteHours) % 24;
        Minute = Mathf.FloorToInt((absoluteHours % 1f) * 60f);
    }
}