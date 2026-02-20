using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueTextPanel;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] float typeSpeed = 0.2f;
    [SerializeField] Button continueButton;
    [SerializeField] Button[] choices;
    [SerializeField] InkEvent[] storyEvents;
    [SerializeField] HideLockCursor hideLockCursor;
    [SerializeField] PlayerController player;
    [SerializeField] CinemachineInputAxisController inputAxisController;
    [SerializeField] Image dialogueImage;
    
    private const float SkipSpeed = 0f;
    private bool isDialogueDone;
    public bool IsDialogueDone => isDialogueDone;
    private bool dialogueIsPlaying;
    private bool isTypingLine;
    private InputSystem_Actions inputActions;
    private Story currentStory;
    
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;
    
    
    
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogError("DialogueManager already exists!");
            Destroy(gameObject);
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
        
        // if(Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     Debug.Log("Continue");
        //     ContinueDialogue();
        // }
    }
    public void EnterDialogueMode(TextAsset inkJSON, string characterName, Sprite characterImage)
    {
        Debug.Log("Entering Dialogue Mode");
        if(inkJSON == null) return;
        SetupStoryContext(inkJSON, characterName);
        SetupUIDialogue(characterImage);
        ContinueDialogue();
    }

    private void SetupUIDialogue(Sprite characterImage)
    {
        dialogueTextPanel.SetActive(true);
        dialogueImage.sprite = characterImage;
        isDialogueDone = false;
        player.enabled = false;
        hideLockCursor.SetLockState(CursorLockMode.None);
        hideLockCursor.SetVisibility(true);
        inputAxisController.enabled = false;
    }

    private void SetupStoryContext(TextAsset inkJSON, string characterName)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        characterNameText.text = characterName;
        foreach (var storyEvent in storyEvents)
        {
            storyEvent.Bind(currentStory);
        }
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
        choiceButton.onClick.RemoveAllListeners();
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
        dialogueTextPanel.SetActive(false);
        isDialogueDone = true;
        foreach (var storyEvent in storyEvents)
        {
            storyEvent.Unbind(currentStory);
        }
        continueButton.gameObject.SetActive(false);
        player.enabled = true;
        hideLockCursor.SetLockState(CursorLockMode.Locked);
        hideLockCursor.SetVisibility(false);
        inputAxisController.enabled = true;
    }

    IEnumerator DisplayEachLetter(string text)
    {
        // This part makes it to where when you hold the space bar for 0.7s, it skips the dialogue
        isTypingLine = true;
        textComponent.text = string.Empty;
        
        for (var i = 0; i < text.Length; i++)
        {
            textComponent.text += text[i];
            yield return new WaitForSeconds(typeSpeed);
            //Wait for the last letter to be displayed, then enable the continue button
            // if (i == text.Length - 1)
            // {
            //     continueButton.gameObject.SetActive(true);
            //     countText.text = "Normal";
            // }
        }
        continueButton.gameObject.SetActive(true);
        isTypingLine = false;
    }

    void OnSkipPerformed(InputAction.CallbackContext ctx)
    {
        if (!dialogueIsPlaying) return;
        if (!isTypingLine) return;
        if (ctx.interaction is not HoldInteraction) return;

        // Skip current line instantly
        StopAllCoroutines();
        textComponent.text = currentStory.currentText;
        continueButton.gameObject.SetActive(true);
        isTypingLine = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Jump.performed += OnSkipPerformed;
        
        // OnDialogueLockState += hideLockCursor.SetLockState;
        // OnDialogueVisibilityState += hideLockCursor.SetVisibility;
    }
    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Jump.performed -= OnSkipPerformed;
        
        // OnDialogueLockState -= hideLockCursor.SetLockState;
        // OnDialogueVisibilityState -= hideLockCursor.SetVisibility;
    }
}