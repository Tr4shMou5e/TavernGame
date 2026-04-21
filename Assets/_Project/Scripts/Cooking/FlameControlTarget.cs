using System;
using System.Collections;
using UnityEngine;

public class FlameControlTarget : MonoBehaviour
{
    public static event Action OnFlameControlTarget;
    [SerializeField] private float restDuration = 1f;
    private InputManager inputManager;
    
    private bool oscillatorInside;
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void Update()
    {
        if (!oscillatorInside) return;

        if (inputManager.PlayerPressedSpace())
        {
            Debug.Log("Target Hit");
            OnFlameControlTarget?.Invoke();
            SoundManager.PlaySound(SoundType.Oven);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Oscillator")) return;

        oscillatorInside = true;
        Debug.Log("Flame Control Target");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Oscillator")) return;

        StartCoroutine(WaitForRest());
    }

    IEnumerator WaitForRest()
    {
        yield return new WaitForSeconds(restDuration);
        oscillatorInside = false;
    }
}