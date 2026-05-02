using UnityEngine;

public class HUDToggleOnMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject persistentHUD;
    
    private void OnEnable()
    {
        InteractableObject.OnMiniGameEnd += ShowHUD;
    }

    private void OnDisable()
    {
        InteractableObject.OnMiniGameEnd -= ShowHUD;
    }

    private void Update()
    {
        bool anyMinigameRunning = IsAnyMinigameRunning();
        
        if (anyMinigameRunning)
        {
            HideHUD();
        }
    }

    private bool IsAnyMinigameRunning()
    {
        Canvas gameplayCanvas = FindCanvasByName("GameplayCanvas");
        if (gameplayCanvas != null && gameplayCanvas.gameObject.activeInHierarchy)
            return true;

        Canvas endScoreCanvas = FindCanvasByName("EndScoreCanvas");
        if (endScoreCanvas != null && endScoreCanvas.gameObject.activeInHierarchy)
            return true;

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