using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private bool followAssigned = false;

    private void Start()
    {
        AssignPlayerToAllVirtualCameras(); // Assign on start (for scene reloads)
    }

    public void AssignPlayerToAllVirtualCameras()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance not found! Ensure Player is active and tagged correctly.");
            return;
        }

        var virtualCameras = FindObjectsOfType<CinemachineVirtualCamera>(true); // Include inactive cameras
        foreach (var vc in virtualCameras)
        {
            if (vc != null)
            {
                vc.Follow = Player.Instance.transform;
                Debug.Log($"{vc.name} now follows Player.");
            }
        }
    }

    private void OnEnable()
    {
        // Subscribe to scene load event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene load event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // Reassign Follow targets after a new scene loads
        AssignPlayerToAllVirtualCameras();
    }
}
