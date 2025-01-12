using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivate : MonoBehaviour, Interactable
{
    [SerializeField] private DialogueObject dialogueObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            if (player.Interactable is DialogueActivate dialogueActivate && dialogueActivate == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        if (!player.DialogueUI.IsOpen) // Only allow interaction if no dialogue is active
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
    }
}
