using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : Singleton<PlayerAtk>
{
    public MonoBehaviour CurrentPlayerAtk { get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Attack.Attack.started += _ => StartAttacking();
        playerControls.Attack.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentPlayerAtk = newWeapon;

        AttackCooldown();
        timeBetweenAttacks = (CurrentPlayerAtk as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull()
    {
        CurrentPlayerAtk = null;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            AttackCooldown();
            (CurrentPlayerAtk as IWeapon).Attack();
        }
    }
}
