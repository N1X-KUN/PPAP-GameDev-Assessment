using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : Singleton<PlayerHP>
{
    public bool isDead { get; private set; }

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider hPSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "HP Slider";
    const string TOWN_TEXT = "House";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHPSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAi enemy = other.gameObject.GetComponent<EnemyAi>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHPSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShake.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHPSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;

            // Trigger death animation
            GetComponent<Animator>().SetTrigger(DEATH_HASH);

            // Reset game after death
            StartCoroutine(ResetGameRoutine());
        }
    }

    private IEnumerator ResetGameRoutine()
    {
        yield return new WaitForSeconds(2f); // Delay for death animation or effect

        // Destroy persistent objects (e.g., PlayerPositionManager, Inventory, etc.)
        ResetPersistentObjects();

        // Load the Title Screen
        SceneManager.LoadScene("Title Page"); // Replace with the correct name of your Title Screen scene
    }
    private void ResetPersistentObjects()
    {
        // Reset all singleton instances
        Player.ResetInstance();
        PlayerAtk.ResetInstance();
        PlayerHP.ResetInstance();
        PlayerPositionManager.ResetInstance();
        ActiveInventory.ResetInstance();
        SceneManagement.ResetInstance();

        // Optionally destroy other objects marked as DontDestroyOnLoad
        GameObject dontDestroyOnLoad = GameObject.Find("DontDestroyOnLoad");
        if (dontDestroyOnLoad != null)
        {
            Destroy(dontDestroyOnLoad);
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f); // Delay for death animation or effect

        // Destroy player object to clean up the scene
        Destroy(gameObject);

        // Load the title scene
        SceneManager.LoadScene("Title Page"); // Replace "TitlePage" with your actual title scene name
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHPSlider()
    {
        if (hPSlider == null)
        {
            hPSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        hPSlider.maxValue = maxHealth;
        hPSlider.value = currentHealth;
    }
}
