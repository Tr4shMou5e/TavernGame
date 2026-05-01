using UnityEngine;

public class PotMiniGameDebug : MonoBehaviour
{
    [SerializeField] private PotInteractableMiniGame potMinigame;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera miniGameCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugCameraStatus();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DebugCanvasStatus();
        }
    }

    private void DebugCameraStatus()
    {
        Debug.Log("=== CAMERA DEBUG ===");
        
        if (mainCamera == null)
            Debug.LogError("Main Camera is NULL!");
        else
            Debug.Log($"Main Camera: Enabled={mainCamera.enabled}, Position={mainCamera.transform.position}, Depth={mainCamera.depth}");

        if (miniGameCamera == null)
            Debug.LogError("Mini Game Camera is NULL!");
        else
            Debug.Log($"Mini Game Camera: Enabled={miniGameCamera.enabled}, Position={miniGameCamera.transform.position}, Depth={miniGameCamera.depth}");

        Camera[] allCameras = FindObjectsOfType<Camera>();
        Debug.Log($"Total cameras in scene: {allCameras.Length}");
        foreach (Camera cam in allCameras)
        {
            Debug.Log($"  - {cam.gameObject.name}: Enabled={cam.enabled}, Depth={cam.depth}, ClearFlags={cam.clearFlags}");
        }
    }

    private void DebugCanvasStatus()
    {
        Debug.Log("=== CANVAS DEBUG ===");
        
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        Debug.Log($"Total canvases in scene: {allCanvases.Length}");
        
        foreach (Canvas canvas in allCanvases)
        {
            Debug.Log($"Canvas: {canvas.gameObject.name}");
            Debug.Log($"  - Active: {canvas.gameObject.activeSelf}");
            Debug.Log($"  - Render Mode: {canvas.renderMode}");
            Debug.Log($"  - Sort Order: {canvas.sortingOrder}");
            
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Debug.Log($"  - Render Camera: {(canvas.worldCamera != null ? canvas.worldCamera.name : "NULL")}");
            }
        }
    }
}