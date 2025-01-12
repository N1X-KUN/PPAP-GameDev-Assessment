using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool IsOpen { get; private set; }
    private bool isTyping = false; // Add this flag
    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        if (isTyping) return; // Prevent starting new dialogue while typing

        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        isTyping = true; // Set flag to indicate dialogue is active

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            // If this is the last dialogue and the object has responses, break out of the loop
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
                break;

            // Wait for space bar input before showing the next dialogue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // Show responses if available
        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }

        isTyping = false; // Reset flag when dialogue finishes
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        isTyping = false; // Reset flag when dialogue box closes
    }
}
