using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AreaExit : MonoBehaviour
{
    public string sceneToLoad;          // Name of the scene to load
    public string sceneTransitionName; // Transition name to match AreaEntrance
    public float waitToLoadTime = 1f;  // Time to wait before loading the scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Save the player's position and current scene
            PlayerPositionManager.Instance.SavePlayerPosition(other.transform.position, SceneManager.GetActiveScene().name);

            // Set the transition name for the next scene
            Player.Instance.transitionName = sceneTransitionName;

            // Start the scene transition
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        // Trigger the fade to black
        UIFade.Instance.FadeToBlack();

        // Wait for the fade and additional delay
        yield return new WaitForSeconds(waitToLoadTime);

        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
