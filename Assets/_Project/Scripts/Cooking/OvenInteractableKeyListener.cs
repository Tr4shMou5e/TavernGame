using UnityEngine;

public class OvenInteractableKeyListener : MonoBehaviour
{
    private OvenInteractableMiniGame ovenMinigame;
    private bool playerInRange = false;

    private void Awake()
    {
        ovenMinigame = GetComponent<OvenInteractableMiniGame>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }

    private void Update()
    {
        // Check for E key press to start oven minigame
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ovenMinigame.Interact();
        }
    }
}