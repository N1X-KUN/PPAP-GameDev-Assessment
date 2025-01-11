using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFound : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the enemy's sprite renderer

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();

        // Automatically find SpriteRenderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        FlipSpriteBasedOnDirection(); // Flip sprite based on movement direction
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }

    private void FlipSpriteBasedOnDirection()
    {
        // Flip sprite depending on whether moving left or right
        if (moveDir.x > 0) // Moving right
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDir.x < 0) // Moving left
        {
            spriteRenderer.flipX = true;
        }
    }
}
