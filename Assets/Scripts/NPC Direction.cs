using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDirection : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private SpriteRenderer spriteRenderer; // Reference to the NPC's sprite renderer

    private void Start()
    {
        // Get the SpriteRenderer component attached to the NPC
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Try to find the player automatically if not assigned
        AssignPlayer();
    }

    private void Update()
    {
        // If the player reference is missing, try to find it
        if (player == null)
        {
            AssignPlayer();
        }

        // Check if the player is on the left or right side of the NPC
        if (player != null)
        {
            if (player.position.x < transform.position.x)
            {
                // Player is on the left side; face left
                spriteRenderer.flipX = true;
            }
            else
            {
                // Player is on the right side; face right
                spriteRenderer.flipX = false;
            }
        }
    }

    private void AssignPlayer()
    {
        // Find the player using its tag (ensure the player has the "Player" tag)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
}
