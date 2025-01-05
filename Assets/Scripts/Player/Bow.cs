using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("Bow is Shooting?");
        // Add appropriate logic here or a method you need from your codebase
        // ActiveInventory.Instance.SomeMethod(); 
    }
}
