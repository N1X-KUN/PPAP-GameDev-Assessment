using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player player; // Drag your Player GameObject here in the Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Ensure the GameManager persists across scenes
    }

    public void ReturnToTitleScreen()
    {
        // Reset all persistent Singletons
        ResetAllSingletons();

        // Load the Title Screen scene
        SceneManager.LoadScene("Title Screen");
    }

    public void StartNewGame()
    {
        // Reset all persistent Singletons
        ResetAllSingletons();

        // Load the first game scene
        SceneManager.LoadScene("Scene 1");

        // Reset the player's state (spawn, health, etc.)
        if (player != null)
        {
            player.ResetPlayerState();
        }
    }

    private void ResetAllSingletons()
    {
        Singleton<ActiveInventory>.ResetInstance();
        Singleton<PlayerAtk>.ResetInstance();
        Singleton<CurrencyPoints>.ResetInstance();
        // Add other Singletons you need to reset here
    }
}
