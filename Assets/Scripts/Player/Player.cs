using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    // Removed DialogueUI reference since it's commented out.
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

    // Removed DialogueUI property
    // public DialogueUI DialogueUI => dialogueUI;  // Commented out

    public Interactable Interactable { get; set; }

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

        knockback = GetComponent<Knockback>();
        currentHealth = maxHealth; // Initialize health

        // Dynamically find DialogueUI - this line is commented out.
        // FindDialogueUI();
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name + ". Reassigning DialogueUI.");
        // FindDialogueUI(); // This line is commented out.
    }

    //private void FindDialogueUI()  // This method is now irrelevant since DialogueUI is removed.
    //{
    //    // DialogUI-related code is commented out here.
    //}

    private void Start()
    {
        ActiveInventory.Instance.EquipStartingWeapon();
        // FindDialogueUI();  // This line is commented out.
    }

    private void OnEnable()
    {
        playerControls.Enable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded; // Register event
    }

    private void OnDisable()
    {
        playerControls.Disable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister event
    }

    private void Update()
    {
        // Removed dialogueUI-related checks.
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
        if (knockback.gettingKnockedBack || PlayerHP.Instance.isDead) { return; }

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
