using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // Assign Follow for all Virtual Cameras in the new scene
        CameraController.Instance.AssignPlayerToAllVirtualCameras();

        // Optionally activate the correct room camera if needed
        foreach (var room in FindObjectsOfType<Room>())
        {
            if (room.virtualCam.activeSelf)
            {
                var vc = room.virtualCam.GetComponent<CinemachineVirtualCamera>();
                if (vc != null && Player.Instance != null)
                {
                    vc.Follow = Player.Instance.transform;
                }
            }
        }
    }
}
