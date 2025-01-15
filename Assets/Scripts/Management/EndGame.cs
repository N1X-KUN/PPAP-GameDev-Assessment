using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title Page";
    [SerializeField] private float delayBeforeLoad = 2f;

    private bool isEnding = false; // To prevent multiple triggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isEnding)
        {
            isEnding = true; // Prevent triggering multiple times
            StartCoroutine(EndGameRoutine());
        }
    }

    private IEnumerator EndGameRoutine()
    {
        // Wait for the delay before loading the title scene
        yield return new WaitForSeconds(delayBeforeLoad);

        // Reset persistent objects and player state
        ResetGame();

        // Load the title screen
        SceneManager.LoadScene(titleSceneName);
    }

    private void ResetGame()
    {
        // Reset singletons or any objects marked DontDestroyOnLoad
        Singleton<Player>.ResetInstance();
        Singleton<PlayerHP>.ResetInstance();
        Singleton<SceneManagement>.ResetInstance();

        // Reset the GameManager
        GameManager.Instance?.ReturnToTitleScreen();
    }
}
