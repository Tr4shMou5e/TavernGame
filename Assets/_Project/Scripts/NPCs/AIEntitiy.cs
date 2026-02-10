using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public abstract class AIEntitiy : MonoBehaviour
{
    public Dialogue dialogue;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (InputManager.Instance.Interact())
        {
            DialogueManager.Instance.EnterDialogueMode(dialogue.textAsset, dialogue.characterName);
        }
    }
}