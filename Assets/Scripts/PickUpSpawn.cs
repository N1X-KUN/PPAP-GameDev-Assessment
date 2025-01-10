using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour
{
    [SerializeField] private GameObject goinPrefab;

    public void DropItems()
    {
        Instantiate(goinPrefab, transform.position, Quaternion.identity);
    }
}
