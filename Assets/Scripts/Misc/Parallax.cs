using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxOffset = -0.15f;

    private Camera Cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)Cam.transform.position - startPos;

    private void Awake()
    {
        Cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxOffset;
    }
}
