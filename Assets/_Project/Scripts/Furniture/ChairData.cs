using System;
using UnityEngine;

public class ChairData : MonoBehaviour
{
    public bool isOccupied;
    public Vector3 position;

    private void OnEnable()
    {
        isOccupied = false;
        position = transform.position;
    }
}
