using System;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public abstract class AIEntitiy : SerializedMonoBehaviour
{
    public Dialogue dialogue;
    protected bool isPlayerInRange;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerInRange = false;
    }
}