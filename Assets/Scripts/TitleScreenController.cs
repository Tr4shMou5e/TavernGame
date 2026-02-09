using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Going to Main Menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
