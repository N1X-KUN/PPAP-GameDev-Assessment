using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Damage>() || other.gameObject.GetComponent<Projectile>())
        {
            PickUpSpawn pickUpSpawn = GetComponent<PickUpSpawn>();
            if (pickUpSpawn != null)
            {
                pickUpSpawn.DropItems();
            }
            else
            {
                Debug.LogWarning("PickUpSpawn component is missing on " + gameObject.name);
            }

            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
