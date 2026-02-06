using UnityEngine;
public class StartDialogue : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    
   
    void Start()
    {
       DialogueManager.Instance.EnterDialogueMode(dialogue.textAsset, dialogue.characterName);
    }
    
}