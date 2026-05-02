using System;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
{
    private void LoadScene(string sceneName)
    {
        SaveLoadSystem.Instance.ChangeScene(sceneName);
    }

    private void OnEnable()
    {
        SceneChange.OnSceneChange += LoadScene;
    }

    private void OnDisable()
    {
        SceneChange.OnSceneChange -= LoadScene;
    }
}