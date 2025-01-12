using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private int maxHealth = 10; // Maximum health

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;

    private bool facingLeft = false;
    private int currentHealth; // Player's current health

    public string transitionName = "DefaultTransition"; // Ensure default value for safety

    public DialogueUI DialogueUI => dialogueUI;

    public Interactable Interactable {get; set; }

    protected override void Awake()
    {
        base.Awake();

        // Find DontDestroyOnLoad and set as parent
        GameObject dontDestroyOnLoad = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyOnLoad != null)
        {
            transform.SetParent(dontDestroyOnLoad.transform);
        }

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();

        Debug.Log("Player initialized: " + transitionName);
        knockback = GetComponent<Knockback>();

        currentHealth = maxHealth; // Initialize health
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        if (dialogueUI.IsOpen) return;

        PlayerInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Interactable != null)
            {
                Interactable.Interact(player: this);
            }
        }
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    // Public method to update transition name dynamically
    public void SetTransitionName(string newName)
    {
        transitionName = newName;
        Debug.Log("Player transitionName updated to: " + transitionName);
    }
}