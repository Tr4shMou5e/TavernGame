using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    private TitleScreenController titleScreenController;

    void Start()
    {
        titleScreenController = GetComponent<TitleScreenController>();
        
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        else
            Debug.LogError("Play Button not assigned!");
            
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        if (titleScreenController != null)
        {
            titleScreenController.PlayGame();
        }
    }

    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}