using System;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
{
    
    private void LoadScene(string sceneName)
    {
        SaveLoadSystem.Instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }

    
    private void OnEnable()
    {
        SceneChange.OnSceneChange += LoadScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneChange.OnSceneChange -= LoadScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveLoadSystem.Instance.LoadGame(scene.name);
    }
}