using System;
using UnityEngine;

public class SpriteOscillation : MonoBehaviour
{
    // This is the formula variable you use to control the oscillation
    [Header("Oscillation Settings")]
    [SerializeField] private float amplitude;
    [SerializeField] private float phaseShift;
    [Tooltip("This is what you use to control the speed of the oscillation")]
    [SerializeField] private float period; // T
    
    [Header("Bounds")]
    [SerializeField] private float maxPos;
    [SerializeField] private float minPos;
    
    private float angularFrequency; // This is omega (w) in the formula = 2 * pi * f
    private float frequency; // 1 / T
    
    private Vector3 position;
    private void OnValidate()
    {
        frequency = 1 / period;
        angularFrequency = 2 * Mathf.PI * frequency;
    }

    void Start()
    {
        position = transform.position;
    }
    void Update()
    {
        var x = amplitude * Mathf.Cos(angularFrequency * Time.time + phaseShift);
        x = Mathf.Clamp(x, minPos, maxPos);
        transform.position =  position + new Vector3(x, 0, 0);
    }
}