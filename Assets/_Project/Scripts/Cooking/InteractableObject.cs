using UnityEngine;
public abstract class InteractableObject : MonoBehaviour, Interactable
{
    protected InputManager inputManager;
    [SerializeField] protected PlayerController player;
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    public virtual void Interact()
    {
        if (inputManager.Interact())
        {
            player.gameObject.SetActive(false);
        }
    }
}