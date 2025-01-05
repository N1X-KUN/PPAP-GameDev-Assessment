using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName; // Transition name to match AreaExit

    private void Start()
    {
        // Check if it's the first scene load
        if (PlayerPositionManager.Instance != null && PlayerPositionManager.Instance.isFirstScene)
        {
            // Reset the flag to indicate the game has started
            PlayerPositionManager.Instance.isFirstScene = false;
            return; // Do nothing; let the player remain at their initial position (e.g., the bed)
        }

        // Handle normal scene transitions
        if (PlayerPositionManager.Instance != null &&
            !string.IsNullOrEmpty(PlayerPositionManager.Instance.lastSceneName))
        {
            if (PlayerPositionManager.Instance.lastSceneName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name &&
                Player.Instance.transitionName == transitionName)
            {
                // Restore player's position
                Player.Instance.transform.position = PlayerPositionManager.Instance.lastScenePosition;

                // Fade in the scene after positioning
                UIFade.Instance.FadeToClear();
                return; // Stop here since we've already handled everything
            }
        }

        // Default fallback: position the player at the AreaEntrance's transform position
        Player.Instance.transform.position = transform.position;

        // Fade in the scene
        UIFade.Instance.FadeToClear();
    }
}
