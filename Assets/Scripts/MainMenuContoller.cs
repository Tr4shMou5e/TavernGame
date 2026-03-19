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
    }

    public void OpenSettings()
    {
        Debug.Log("Settings opened (placeholder)");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}