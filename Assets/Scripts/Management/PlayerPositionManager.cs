using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager Instance { get; private set; }

    public Vector3 lastScenePosition; // Position where the player should spawn
    public string lastSceneName;      // Name of the last scene
    public bool isFirstScene = true; // New: Tracks if the game just started

    private void Awake()
    {
        // Ensure there's only one instance of this class
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    // Save player's position and scene name
    public void SavePlayerPosition(Vector3 position, string sceneName)
    {
        lastScenePosition = position;
        lastSceneName = sceneName;
    }
}
