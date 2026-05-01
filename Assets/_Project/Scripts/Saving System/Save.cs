using UnityEngine;

public class Save : MonoBehaviour
{
    private bool playerInRange;
    

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;

        playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {   
        if(!other.CompareTag("Player"))
            return;

        playerInRange = false;
    }

    private void Update()
    {
        if(!playerInRange)
            return;
        if(InputManager.Instance.Interact())
        {
            SaveLoadSystem.Instance.SaveGame();
        }
    }
}
