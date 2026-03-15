using System;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public abstract class AIEntitiy : SerializedMonoBehaviour
{
    public Dialogue dialogue;
    protected Transform rayOrigin;
    [SerializeField] protected float interactDistance = 1f;
    protected bool isPlayerInRange;
    public bool IsPlayerInRange => isPlayerInRange;
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
    protected bool IsPlayerLookingAtMe()
    {
        if (Camera.main != null) rayOrigin = Camera.main.transform;
        if (rayOrigin == null) return false;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hitInfo, interactDistance))
        {
            AIEntitiy npc = hitInfo.collider.GetComponentInParent<AIEntitiy>();
            return npc == this;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + rayOrigin.forward * interactDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rayOrigin.position + rayOrigin.forward * interactDistance, 0.2f);
    }
}