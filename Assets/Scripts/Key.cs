using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject bigWild;
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("You got Big's Key!");
            bigWild.GetComponent<Collider2D>().enabled = false;

            this.gameObject.SetActive(false);
        }
    }
}
