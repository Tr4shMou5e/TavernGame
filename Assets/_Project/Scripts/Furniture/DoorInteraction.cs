using System;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
   [SerializeField] private Animator animator;
   [SerializeField] private float crossFadeDuration = 0.5f;
   private readonly int doorOpenHash = Animator.StringToHash("Open_Door");
   private readonly int doorCloseHash = Animator.StringToHash("Close_Door");
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