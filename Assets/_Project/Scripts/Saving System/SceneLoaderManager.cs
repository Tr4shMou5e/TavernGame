using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoaderManager : PersistentSingleton<SceneLoaderManager>
{
     [SerializeField] List<string> sceneNames;
     void OnEnable()
     {
          SceneManager.activeSceneChanged += OnSceneChanged;
          SceneChange.OnSceneChange += LoadScene;
     }
     void OnDisable()
     {
          SceneManager.activeSceneChanged -= OnSceneChanged;
          SceneChange.OnSceneChange -= LoadScene;
     }

     private void OnSceneChanged(Scene previousScene, Scene currentScene)
     {
            
     }
     private void LoadScene(string sceneName)
     {
          SaveLoadSystem.Instance.SaveGame();
          SceneManager.LoadScene(sceneName);
     }
     
}