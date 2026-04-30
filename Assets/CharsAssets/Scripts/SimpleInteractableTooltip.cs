using TMPro;
using UnityEngine;

public class SimpleInteractableTooltip : MonoBehaviour
{
    [SerializeField] private Canvas tooltipCanvas;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private string tooltipMessage = "Press E to interact";
    [SerializeField] private float detectionRadius = 5f;
    
    private bool playerInRange = false;

    private void Awake()
    {
        if (tooltipCanvas != null)
        {
            tooltipCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for player in range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        bool foundPlayer = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                foundPlayer = true;
                break;
            }
        }

        if (foundPlayer && !playerInRange)
        {
            ShowTooltip();
            playerInRange = true;
        }
        else if (!foundPlayer && playerInRange)
        {
            HideTooltip();
            playerInRange = false;
        }
    }

    private void ShowTooltip()
    {
        if (tooltipCanvas != null)
        {
            tooltipCanvas.gameObject.SetActive(true);
            if (tooltipText != null)
            {
                tooltipText.text = tooltipMessage;
            }
        }
    }

    private void HideTooltip()
    {
        if (tooltipCanvas != null)
        {
            tooltipCanvas.gameObject.SetActive(false);
        }
    }
}