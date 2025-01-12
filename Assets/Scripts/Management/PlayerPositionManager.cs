using UnityEngine;

public class PlayerPositionManager : Singleton<PlayerPositionManager>
{
    public Vector3 lastScenePosition; // Position where the player should spawn
    public string lastSceneName;      // Name of the last scene
    public bool isFirstScene = true;  // Tracks if the game just started

    // Save player's position and scene name
    public void SavePlayerPosition(Vector3 position, string sceneName)
    {
        lastScenePosition = position;
        lastSceneName = sceneName;
    }

    protected override void Awake()
    {
        base.Awake(); // Ensures Singleton functionality
    }
}
