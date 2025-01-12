using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicSource; // Reference to your AudioSource component

    // Called when the player clicks the "Play Game" button
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Called when the player clicks the "Quit Game" button
    public void QuitGame()
    {
        Application.Quit();
    }

    // Called when the player clicks the "Music" button
    public void ToggleMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.Play();
        }
    }
}
