using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : Singleton<PlayerAtk>
{
    public MonoBehaviour CurrentPlayerAtk { get; private set; }

    private PlayerControls playerControls;

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
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentPlayerAtk = newWeapon;
    }

    public void WeaponNull()
    {
        CurrentPlayerAtk = null;
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
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
            isAttacking = true;
            (CurrentPlayerAtk as IWeapon)?.Attack();
        }
    }
}
