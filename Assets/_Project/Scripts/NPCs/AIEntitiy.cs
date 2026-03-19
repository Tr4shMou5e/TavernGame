using System;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public abstract class AIEntitiy : SerializedMonoBehaviour, IBind<AIEntityData>
{
    [SerializeField] protected float interactDistance = 2.4f;
    [SerializeField] protected float radius = 0.25f;
    [SerializeField] protected Transform player;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected CustomerName customerMaleNames;
    [SerializeField] protected CustomerName customerFemaleNames;

    [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
    protected Transform rayOrigin;
    protected bool isPlayerInRange;
    protected AIEntityData data;
    public Dialogue dialogue;
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
        if (mainCamera == null) return false;
        
        Vector3 toNpc = (transform.position - mainCamera.transform.position).normalized;
        float dot = Vector3.Dot(mainCamera.transform.forward, toNpc);
        
        return dot > 0.9f;
    }

    private void OnDrawGizmos()
    {
        if (mainCamera == null) return;

        Vector3 cameraPos = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 toNpc = (transform.position - cameraPos).normalized;

        float dot = Vector3.Dot(cameraForward, toNpc);
        float drawDistance = interactDistance;

        // Camera forward ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cameraPos, cameraPos + cameraForward * drawDistance);

        // Direction from camera to NPC
        Gizmos.color = dot > 0.9f ? Color.green : Color.yellow;
        Gizmos.DrawLine(cameraPos, transform.position);

        // Small sphere at NPC target point
        Gizmos.DrawSphere(transform.position, 0.15f);
    }

    
    public virtual void Bind(AIEntityData data)
    {
        // noop
    }
}

[Serializable]
public class AIEntityData : ISaveable
{
    [field: SerializeField] public SerializableGuid Id { get; set; }
    public CustomerStateType currentState;
    public Vector3 position;
    public Quaternion rotation;
}

public enum CustomerStateType
{
    WaitList,
    Order,
    Sit,
    Eat,
    Exit
}