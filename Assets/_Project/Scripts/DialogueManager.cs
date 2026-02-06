using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueTextPanel;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] float typeSpeed = 0.2f;
    [SerializeField] Button continueButton;
    [SerializeField] Button[] choices;
    [SerializeField] InkEvent[] storyEvents;
    
    InputSystem_Actions inputActions;
    private Story currentStory;
    private bool dialogueIsPlaying;

    private static DialogueManager instance;
    public static DialogueManager Instance => instance;
    
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("DialogueManager already exists!");
            return;
        }
        instance = this;
        inputActions = new InputSystem_Actions();
    }

    
    void Update()
    {
        if (!dialogueIsPlaying)
        {
            Debug.Log("Not playing");
            return;
        }
        
        if(inputActions.Player.Jump.WasPressedThisFrame())
        {
            Debug.Log("Continue");
            ContinueDialogue();
        }
    }
    public void EnterDialogueMode(TextAsset inkJSON, string characterName)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        characterNameText.text = characterName;
        foreach (var storyEvent in storyEvents)
        {
            storyEvent.Bind(currentStory);
        }
        ContinueDialogue();
    }
    
    public void ContinueDialogue()
    {
        continueButton.gameObject.SetActive(false);
        if(currentStory.canContinue)
        {
            textComponent.text = string.Empty;
            StartCoroutine(DisplayEachLetter(currentStory.Continue()));
        }
        else if(currentStory.currentChoices.Count > 0)
        {
            DisplayChoice();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    void DisplayChoice()
    {   
        continueButton.gameObject.SetActive(false);
        //Goes through each choice and sets up the buttons
        for(var i = 0; i < currentStory.currentChoices.Count; i++)
        {
            SetupChoices(choices[i], currentStory.currentChoices[i]);
        }
    }
    
    /// <summary>
    /// This function sets and displays all the available choices for the player to choose from.
    /// Then sets the text of the button to the text of the choice and adds a listener to the button.
    /// </summary>
    /// <param name="choiceButton"></param>
    /// <param name="currentChoice"></param>
    void SetupChoices(Button choiceButton, Choice currentChoice)
    {   
        
        choiceButton.gameObject.SetActive(true);
        choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currentChoice.text;
        choiceButton.onClick.AddListener(() =>
        {
            ProcessChoiceAndContinue(currentChoice);
        });
    }
    
    /// <summary>
    /// This function processes the player's choice and continues the dialogue
    /// while also hiding all the choice buttons.
    /// </summary>
    /// <param name="currentChoice"></param>
    void ProcessChoiceAndContinue(Choice currentChoice)
    {
        currentStory.ChooseChoiceIndex(currentChoice.index);
        foreach (var button in choices)
        {
            button.gameObject.SetActive(false);
        }
        ContinueDialogue();
    }

    void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        characterNameText.text = string.Empty;
        textComponent.text = string.Empty;
        foreach (var storyEvent in storyEvents)
        {
            storyEvent.Unbind(currentStory);
        }
        continueButton.gameObject.SetActive(false);
    }

    IEnumerator DisplayEachLetter(string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            textComponent.text += text[i];
            yield return new WaitForSeconds(typeSpeed);
            //Wait for the last letter to be displayed, then enable the continue button
            if (i == text.Length - 1)
            {
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}