using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, hP, surpriseEnemy;
    private bool hasDropped = false; // Prevent multiple drops

    public void DropItems()
    {
        if (hasDropped) return; // Prevent multiple drops
        hasDropped = true;

        int randomNum = Random.Range(1, 5);

        if (randomNum == 1)
        {
            Instantiate(hP, transform.position, Quaternion.identity);
        }
        else if (randomNum == 2)
        {
            Instantiate(surpriseEnemy, transform.position, Quaternion.identity);
        }
        else if (randomNum == 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);

            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
