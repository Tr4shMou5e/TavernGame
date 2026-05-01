using UnityEngine;

public class HUDToggleOnMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject persistentHUD;
    
    private void OnEnable()
    {
        // Subscribe to minigame events from InteractableObject
        InteractableObject.OnMiniGameEnd += ShowHUD;
    }

    private void OnDisable()
    {
        // Unsubscribe when script is disabled
        InteractableObject.OnMiniGameEnd -= ShowHUD;
    }

    private void Update()
    {
        // Check if any minigame is running by looking for active minigame canvases
        bool anyMinigameRunning = IsAnyMinigameRunning();
        
        if (anyMinigameRunning)
        {
            HideHUD();
        }
    }

    private bool IsAnyMinigameRunning()
    {
        // Check if GameplayCanvas is active (stove minigame)
        Canvas gameplayCanvas = FindCanvasByName("GameplayCanvas");
        if (gameplayCanvas != null && gameplayCanvas.gameObject.activeInHierarchy)
            return true;

        // Check if End Score Canvas is active (oven minigame)
        Canvas endScoreCanvas = FindCanvasByName("EndScoreCanvas");
        if (endScoreCanvas != null && endScoreCanvas.gameObject.activeInHierarchy)
            return true;

        // Check if results canvas is active (stove results)
        Canvas resultsCanvas = FindCanvasByName("ResultsCanvas");
        if (resultsCanvas != null && resultsCanvas.gameObject.activeInHierarchy)
            return true;

        return false;
    }

    private Canvas FindCanvasByName(string canvasName)
    {
        Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var canvas in allCanvases)
        {
            if (canvas.gameObject.name == canvasName)
                return canvas;
        }
        return null;
    }

    private void HideHUD()
    {
        if (persistentHUD != null && persistentHUD.activeInHierarchy)
        {
            persistentHUD.SetActive(false);
        }
    }

    private void ShowHUD()
    {
        if (persistentHUD != null && !persistentHUD.activeInHierarchy)
        {
            persistentHUD.SetActive(true);
        }
    }
}