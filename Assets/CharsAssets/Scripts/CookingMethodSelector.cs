using UnityEngine;
using UnityEngine.UI;

public class CookingMethodSelector : MonoBehaviour
{
    [SerializeField] private Canvas selectionCanvas;
    [SerializeField] private Button stoveButton;
    [SerializeField] private Button ovenButton;
    [SerializeField] private StoveTopCookingInteractableMiniGame stoveMinigame;
    [SerializeField] private OvenInteractableMiniGame ovenMinigame;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        HideSelectionScreen();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowSelectionScreen();
        }
    }

    private void ShowSelectionScreen()
    {
        if (selectionCanvas != null)
            selectionCanvas.gameObject.SetActive(true);
    }

    private void HideSelectionScreen()
    {
        if (selectionCanvas != null)
            selectionCanvas.gameObject.SetActive(false);
    }

    private void OnStovePressed()
    {
        HideSelectionScreen();
        if (stoveMinigame != null)
            stoveMinigame.Interact();
    }

    private void OnOvenPressed()
    {
        HideSelectionScreen();
        if (ovenMinigame != null)
            ovenMinigame.Interact();
    }

    private void OnEnable()
    {
        stoveButton?.onClick.AddListener(OnStovePressed);
        ovenButton?.onClick.AddListener(OnOvenPressed);
    }

    private void OnDisable()
    {
        stoveButton?.onClick.RemoveListener(OnStovePressed);
        ovenButton?.onClick.RemoveListener(OnOvenPressed);
    }
}