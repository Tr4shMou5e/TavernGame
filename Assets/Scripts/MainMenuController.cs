using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("Starting New Game...");
        SceneManager.LoadScene("MainWorld");
    }

    public void LoadGame()
    {
        Debug.Log("Load Game clicked (not implemented yet)");
        // Later: Load save file, then SceneManager.LoadScene("MainGame");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings opened (placeholder)");
        // Later: Enable settings panel
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
