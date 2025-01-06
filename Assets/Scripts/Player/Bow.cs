using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    readonly int FIRE = Animator.StringToHash("Fire");

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, PlayerAtk.Instance.transform.rotation);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
