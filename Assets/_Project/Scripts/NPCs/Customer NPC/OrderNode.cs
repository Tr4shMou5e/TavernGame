using System;
using UnityEngine;

[Serializable]
public class OrderNode : MonoBehaviour
{
    public Vector3 position;
    public bool isOccupied;

    void Awake()
    {
        position = transform.position;
        isOccupied = false;
    }
}