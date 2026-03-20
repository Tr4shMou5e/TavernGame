using UnityEngine;

public class BarDoorInteraction : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float crossFadeDuration = 0.5f;
    private readonly int doorOpenHash = Animator.StringToHash("Bar_Door_Open");
    private readonly int doorCloseHash = Animator.StringToHash("Bar_Door_Close");
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        animator.CrossFade(doorOpenHash, crossFadeDuration);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        animator.CrossFade(doorCloseHash, crossFadeDuration); 
    }
}