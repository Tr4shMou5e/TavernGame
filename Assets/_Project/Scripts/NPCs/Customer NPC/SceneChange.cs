using System;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private SceneNameType sceneName;

    private bool isTriggered;
    private bool hasChangedScene;

    public static event Action<string> OnSceneChange;

    private void Update()
    {
        if (!isTriggered)
            return;

        if (hasChangedScene)
            return;

        if (InputManager.Instance.Interact())
        {
            hasChangedScene = true;
            OnSceneChange?.Invoke(sceneName.ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isTriggered = false;
        hasChangedScene = false;
    }

    private enum SceneNameType
    {
        MainWorld,
        Tavern1,
        Home
    }
}