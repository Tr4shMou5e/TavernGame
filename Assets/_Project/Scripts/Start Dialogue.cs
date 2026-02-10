using UnityEngine;
public class StartDialogue : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    
   
    void Start()
    {
       DialogueManager.Instance.EnterDialogueMode(dialogue.textAsset, dialogue.characterName);
    }
    
}
//Every NPC should have a Dialogue component,
//To access that dialogue, the player should be in range of the NPC and interact with it
