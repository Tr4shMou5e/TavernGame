using UnityEngine;

public enum SoundType
{
    WinSound,
    ScoreSound,
    Oven
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static void PlaySound(SoundType soundType, float volume = 1f)
    {
        // Stub - just logs for now
        Debug.Log($"Playing sound: {soundType}");
    }
}
