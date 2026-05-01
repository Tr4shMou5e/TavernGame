using System;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] SceneNameType sceneName;
     
    private bool isTriggered;
    
    public static event Action<string> OnSceneChange;
    private void Update()
    {
        if (!isTriggered) return;
        if(InputManager.Instance.Interact())
        {
            OnSceneChange?.Invoke(sceneName.ToString());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;
        isTriggered = true;
        
    }
    void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;
        isTriggered = false;
    }

    private enum SceneNameType
    {
        MainWorld,
        Tavern,
        Home
    }
}