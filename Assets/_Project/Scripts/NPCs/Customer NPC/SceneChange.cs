using System;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] SceneNameType sceneName;
    
    private bool isTriggered;
    
    public static event Action<string> OnSceneChange;
    void Update()
    {
        if(!isTriggered) return;
        if(InputManager.Instance.Interact())
        {
            OnSceneChange?.Invoke(sceneName.ToString());
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;
        isTriggered = true;
    }
    void OnTriggerExit(Collider other)
    {
        if(!isTriggered)
            return;
        isTriggered = false;
    }

    private enum SceneNameType
    {
        MainMenu,
        Tavern,
        Home
    }
}