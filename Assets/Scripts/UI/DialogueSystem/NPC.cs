using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public bool playerIsNear;

    private Player player; 

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Press F to start or reset dialogue
        if (Input.GetKeyDown(KeyCode.F) && playerIsNear)
        {
            if (!dialogueBox.activeSelf)
            {
                dialogueBox.SetActive(true);
                DisablePlayerControls(); // Disable player controls
                StartCoroutine(Typing());
            }
            else
            {
                ResetText();
            }
        }

        // Press Spacebar to progress to the next line
        if (Input.GetKeyDown(KeyCode.Space) && dialogueBox.activeSelf && dialogueText.text == dialogue[index])
        {
            NextLine();
        }
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        // Progress to the next line or reset if it's the last one
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ResetText();
        }
    }

    public void ResetText()
    {
        dialogueText.text = "";
        index = 0;
        dialogueBox.SetActive(false);
        EnablePlayerControls(); // Re-enable player controls
    }

    private void DisablePlayerControls()
    {
        if (player != null)
        {
            player.enabled = false; 
        }
    }

    private void EnablePlayerControls()
    {
        if (player != null)
        {
            player.enabled = true; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            ResetText();
        }
    }
}
