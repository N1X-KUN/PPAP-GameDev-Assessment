using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCam; // Assign this in the Inspector for each room

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);

            // Assign Follow to ensure proper tracking
            var vc = virtualCam.GetComponent<CinemachineVirtualCamera>();
            if (vc != null && Player.Instance != null)
            {
                vc.Follow = Player.Instance.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}
