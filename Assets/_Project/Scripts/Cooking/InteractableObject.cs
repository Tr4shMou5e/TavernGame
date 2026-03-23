using UnityEngine;
public abstract class InteractableObject : MonoBehaviour, Interactable
{
    [SerializeField] protected PlayerController player;
    [SerializeField] protected GameObject miniGame;
    protected InputManager inputManager;
    private bool startMiniGame;
    
    public virtual void Interact()
    {
        if (!startMiniGame) return;
        if (inputManager.Interact())
        { 
            StartComponents();
        }
    }
    
    /// <summary>
    /// Starts or stops the mini-game.
    /// Make sure to call this method when the player interacts with the object.
    /// Place this at the top of the Interact() method.
    /// </summary>
    protected void StartStopMiniGame()
    {
        startMiniGame = !startMiniGame;
    }
    /// <summary>
    /// This reverts the main game to normal after the mini-game is over.
    /// </summary>
    protected void StopComponents()
    {
        miniGame.SetActive(false);
        player.enabled = true;
    }
    
    private void StartComponents()
    {
        miniGame.SetActive(true);
        player.enabled = false;
    }
}